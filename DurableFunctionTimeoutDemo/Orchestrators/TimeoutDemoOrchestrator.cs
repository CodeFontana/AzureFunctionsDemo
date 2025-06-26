using DurableFunctionTimeoutDemo.ActivityFunctions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;

namespace DurableFunctionTimeoutDemo.Orchestrators;

internal static class TimeoutDemoOrchestrator
{
    [Function(nameof(TimeoutDemoOrchestrator))]
    public static async Task RunOrchestrator(
        [OrchestrationTrigger] TaskOrchestrationContext context)
    {
        int input = context.GetInput<int>();
        int taskDelay = input == 0 ? 5000 : input;

        ILogger logger = context.CreateReplaySafeLogger(nameof(TimeoutDemoOrchestrator));
        logger.LogInformation("TimeoutDemoOrchestrator started at {StartTime}", context.CurrentUtcDateTime);

        DateTime endTime = context.CurrentUtcDateTime.AddSeconds(60);
        int i = 0;

        while (context.CurrentUtcDateTime < endTime)
        {
            ProcessDataInput activityInput = new(i, taskDelay);
            await context.CallActivityAsync("ProcessData", activityInput);
            i += 1;
        }

        logger.LogInformation("TimeoutDemoOrchestrator completed at {EndTime}", context.CurrentUtcDateTime);
    }

    [Function($"{nameof(TimeoutDemoOrchestrator)}_HttpStart")]
    public static async Task<HttpResponseData> HttpStart(
        [HttpTrigger(
            AuthorizationLevel.Function,
            "post",
            Route = $"{nameof(TimeoutDemoOrchestrator)}_HttpStart/{{taskDelay:int}}")
        ] HttpRequestData req,
        int taskDelay,
        [DurableClient] DurableTaskClient client,
        FunctionContext executionContext)
    {
        ILogger logger = executionContext.GetLogger($"{nameof(TimeoutDemoOrchestrator)}_HttpStart");
        string instanceId = await client.ScheduleNewOrchestrationInstanceAsync(nameof(TimeoutDemoOrchestrator), taskDelay);
        logger.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);

        HttpResponseData response = req.CreateResponse(System.Net.HttpStatusCode.Accepted);
        await response.WriteStringAsync($"Started orchestration with ID: {instanceId}");
        return response;
    }
}
