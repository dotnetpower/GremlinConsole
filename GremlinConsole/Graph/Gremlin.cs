namespace GremlinConsole.Graph
{
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.Documents.Linq;
    using Microsoft.Azure.Graphs;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class Gremlin
    {
        private const int DEFAULT_RU = 400;

        private DocumentClient client;

        private Dictionary<string, DocumentCollection> dicGraphCollection = new Dictionary<string, DocumentCollection>();

        public Gremlin(string endpointUri, string authKey, string databaseId)
        {
            CosmosDBHelper.Initialize(endpointUri, authKey, databaseId);
            client = CosmosDBHelper.Client;
        }

        private async Task<DocumentCollection> GetDocumentCollection(string collectionId)
        {
            DocumentCollection coll = new DocumentCollection();

            if (!dicGraphCollection.ContainsKey(collectionId))
            {
                coll = await client.CreateDocumentCollectionIfNotExistsAsync(
                    UriFactory.CreateDatabaseUri(CosmosDBHelper.DatabaseId),
                    new DocumentCollection { Id = collectionId },
                    new RequestOptions { OfferThroughput = DEFAULT_RU });

                dicGraphCollection.Add(collectionId, coll);
            }
            else
            {
                coll = dicGraphCollection[collectionId];
            }

            return coll;
        }

        public async Task<List<T>> GremlinQuery<T>(string collectionId, string query)
        {
            List<T> results = new List<T>();

            DocumentCollection graph = await GetDocumentCollection(collectionId);

            IDocumentQuery<T> queryResult = client.CreateGremlinQuery<T>(graph, query);

            while (queryResult.HasMoreResults)
            {
                results.AddRange(await queryResult.ExecuteNextAsync<T>());
            }

            return results;
        }
    }
}
