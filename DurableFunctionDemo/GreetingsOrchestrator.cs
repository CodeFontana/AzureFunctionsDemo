using DurableFunctionDemo.ActivityFunctions;
using DurableFunctionDemo.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace DurableFunctionDemo
{
    public static class GreetingsOrchestrator
    {
        [Function(nameof(GreetingsOrchestrator))]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] TaskOrchestrationContext context,
            string[] names)
        {
            ILogger logger = context.CreateReplaySafeLogger(nameof(GreetingsOrchestrator));
            logger.LogInformation("Starting orchestrator.");
            List<string> outputs = new();

            foreach (string name in names)
            {
                string greeting = await context.CallActivityAsync<string>(
                    nameof(GenerateGreetingActivity.GenerateGreeting), name);

                GreetingModel greetingData = new(name, greeting);

                string filename = await context.CallActivityAsync<string>(
                    nameof(SaveGreetingToFileActivity.SaveGreetingToFile), greetingData);

                await context.CallActivityAsync(
                    nameof(SaveGreetingToStorageBlobActivity.SaveGreetingToStorageBlob), filename);

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

            // Deserialize the request content to get the array of names
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            string[]? names = JsonSerializer.Deserialize<string[]>(requestBody);

            // Function input comes from the request content
            // Pass the array of names as input to the orchestrator
            string instanceId = await client.ScheduleNewOrchestrationInstanceAsync(
                nameof(GreetingsOrchestrator), names);

            logger.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);

            // Returns an HTTP 202 response with an instance management payload
            // See https://learn.microsoft.com/azure/azure-functions/durable/durable-functions-http-api#start-orchestration
            return client.CreateCheckStatusResponse(req, instanceId);
        }
    }
}
