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

    public async Task CreateSupportMessageAsync(SupportMessage message)
    {
        await _container.CreateItemAsync(
            message,
            new PartitionKey(message.Category));
    }
    
    
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
}