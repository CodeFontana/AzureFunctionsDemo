using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace DurableFunctionDemo.ActivityFunctions;

public static class SaveGreetingToStorageBlobActivity
{
    // Activity 3: Save greeting to Azure Storage Blob
    [Function(nameof(SaveGreetingToStorageBlob))]
    public static void SaveGreetingToStorageBlob([ActivityTrigger] string blobName, FunctionContext executionContext)
    {
        ILogger log = executionContext.GetLogger(nameof(SaveGreetingToStorageBlob));
        log.LogInformation("Save greeting to storage blob: {blobName}", blobName);
        // Save blob logic
    }
}
