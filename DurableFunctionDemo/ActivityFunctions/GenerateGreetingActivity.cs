using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace DurableFunctionDemo.ActivityFunctions;

public static class GenerateGreetingActivity
{
    // Activity 1: Generate a greeting message
    [Function(nameof(GenerateGreeting))]
    public static string GenerateGreeting([ActivityTrigger] string name, FunctionContext executionContext)
    {
        ILogger log = executionContext.GetLogger(nameof(GenerateGreeting));
        log.LogInformation($"Generating greeting for {name}.");
        return $"Hello, {name}!";
    }
}
