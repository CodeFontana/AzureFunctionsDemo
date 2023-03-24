using DurableFunctionDemo.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace DurableFunctionDemo.ActivityFunctions;

public static class SaveGreetingToFileActivity
{
    [Function(nameof(SaveGreetingToFile))]
    public static async Task<string> SaveGreetingToFile([ActivityTrigger] GreetingModel greetingData, FunctionContext executionContext)
    {
        ILogger log = executionContext.GetLogger(nameof(SaveGreetingToFile));
        string filename = $"geeting_{greetingData.Name}_{DateTime.UtcNow.Ticks}.txt";
        log.LogInformation("Saving greeting to a file: {filename}", filename);
        await File.WriteAllTextAsync(filename, greetingData.Name);
        return filename;
    }
}
