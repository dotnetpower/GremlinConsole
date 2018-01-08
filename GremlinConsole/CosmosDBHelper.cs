namespace GremlinConsole
{
    using Microsoft.Azure.Documents.Client;
    using System;

    public class CosmosDBHelper
    {
        public static string DatabaseId { get; set; }
        public static string CollectionId { get; set; }
        private static string EndpointUri { get; set; }
        private static string AuthKey { get; set; }

        private static Lazy<DocumentClient> client = new Lazy<DocumentClient>(() =>
        {
            return new DocumentClient(new Uri(EndpointUri), AuthKey, new ConnectionPolicy
            {
                ConnectionMode = ConnectionMode.Direct,
                ConnectionProtocol = Protocol.Tcp,
                MaxConnectionLimit = 1000
            });
        });

        public static DocumentClient Client
        {
            get
            {
                return client.Value;
            }
        }

        /// <summary>
        /// init from Global.asax.cs 
        /// </summary>
        /// <param name="endpointUri"></param>
        /// <param name="authKey"></param>
        /// <param name="databaseId"></param>
        /// <param name="collectionId"></param>
        public static void Initialize(string endpointUri, string authKey, string databaseId)
        {
            EndpointUri = endpointUri;
            AuthKey = authKey;
            DatabaseId = databaseId;

            // https://docs.microsoft.com/ko-kr/azure/cosmos-db/performance-tips
            Client.OpenAsync();
        }
    }
}
