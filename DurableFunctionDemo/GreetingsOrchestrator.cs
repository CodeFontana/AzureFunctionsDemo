using DurableFunctionDemo.ActivityFunctions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;

namespace DurableFunctionDemo
{
    public static class GreetingsOrchestrator
    {
        [Function(nameof(GreetingsOrchestrator))]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] TaskOrchestrationContext context)
        {
            ILogger logger = context.CreateReplaySafeLogger(nameof(GreetingsOrchestrator));
            logger.LogInformation("Starting orchestrator.");
            List<string> outputs = new();
            string[] names = new[] { "Judy", "Brian", "Piper", "Pepper" };

            foreach (string name in names)
            {
                string greeting = await context.CallActivityAsync<string>(
                    nameof(GenerateGreetingActivity.GenerateGreeting), name);

                string filePath = await context.CallActivityAsync<string>(
                    nameof(SaveGreetingToFileActivity.SaveGreetingToFile), greeting);

                await context.CallActivityAsync(
                    nameof(SaveGreetingToStorageBlobActivity.SaveGreetingToStorageBlob), filePath);

                outputs.Add(greeting);
            }

            return outputs;
        }

        [Function("GreetingsOrchestrator_HttpStart")]
        public static async Task<HttpResponseData> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req,
            [DurableClient] DurableTaskClient client,
            FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("GreetingsOrchestrator_HttpStart");

            // Function input comes from the request content.
            string instanceId = await client.ScheduleNewOrchestrationInstanceAsync(
                nameof(GreetingsOrchestrator));

            logger.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);

            // Returns an HTTP 202 response with an instance management payload.
            // See https://learn.microsoft.com/azure/azure-functions/durable/durable-functions-http-api#start-orchestration
            return client.CreateCheckStatusResponse(req, instanceId);
        }
    }
}
