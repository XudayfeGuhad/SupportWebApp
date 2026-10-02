using Microsoft.Azure.Cosmos;
using SupportWebApp.Models;

namespace SupportWebApp.Services;

public class CosmosDbService
{
    private readonly Container _container;

    public CosmosDbService(IConfiguration configuration)
    {
        var connectionString = configuration["CosmosDb:ConnectionString"];

        var client = new CosmosClient(connectionString);
        var database = client.GetDatabase("IBasSupportDB");
        _container = database.GetContainer("ibassupport");
    }

    // Henter alle supporthenvendelser
    public async Task<List<SupportMessage>> GetSupportMessagesAsync()
    {
        var messages = new List<SupportMessage>();

        var query = _container.GetItemQueryIterator<SupportMessage>(
            new QueryDefinition("SELECT * FROM c"));

        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();
            messages.AddRange(response);
        }

        return messages;
    }

    // Henter kun supporthenvendelser fra én kategori
    public async Task<List<SupportMessage>> GetSupportMsgByCategoryAsync(string category)
    {
        var queryDef = new QueryDefinition(
                "SELECT * FROM c WHERE c.category = @category")
            .WithParameter("@category", category);

        var query = _container.GetItemQueryIterator<SupportMessage>(
            queryDef,
            requestOptions: new QueryRequestOptions
            {
                PartitionKey = new PartitionKey(category)
            });

        var results = new List<SupportMessage>();

        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();
            results.AddRange(response);
        }

        return results;
    }

    // Opretter en ny supporthenvendelse
    public async Task CreateSupportMessageAsync(SupportMessage message)
    {
        await _container.CreateItemAsync(
            message,
            new PartitionKey(message.Category));
    }
}