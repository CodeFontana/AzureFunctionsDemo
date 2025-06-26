using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace DurableFunctionTimeoutDemo.ActivityFunctions;

public record ProcessDataInput(int ExecutionIndex, int TaskDelay);

internal static class TimeoutDemoActivities
{
    [Function("ProcessData")]
    public static async Task ProcessDataAsync([ActivityTrigger] ProcessDataInput input, FunctionContext executionContext)
    {
        ILogger logger = executionContext.GetLogger(nameof(TimeoutDemoActivities));
        logger.LogInformation("Begin processing data for index={index}, with delay={delay}...", input.ExecutionIndex, input.TaskDelay);
        await Task.Delay(input.TaskDelay);
        logger.LogInformation("Data processing completed for index={index}.", input.ExecutionIndex);
    }
}
