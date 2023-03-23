using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace DurableFunctionDemo.ActivityFunctions;

public static class SaveGreetingToFileActivity
{
    [Function(nameof(SaveGreetingToFile))]
    public static async Task<string> SaveGreetingToFile([ActivityTrigger] string greeting, FunctionContext executionContext)
    {
        ILogger log = executionContext.GetLogger(nameof(SaveGreetingToFile));
        log.LogInformation($"Saving greeting to a file: {greeting}");
        // Save the greeting to a file and return the file path
        string filePath = $"greeting_{DateTime.UtcNow.Ticks}.txt";
        await File.WriteAllTextAsync(filePath, greeting);
        return filePath;
    }
}
