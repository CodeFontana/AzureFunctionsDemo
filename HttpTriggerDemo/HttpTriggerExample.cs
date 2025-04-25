using System.Net;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace HttpTriggerDemo
{
    public class HttpTriggerExample
    {
        private readonly ILogger _logger;

        public HttpTriggerExample(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<HttpTriggerExample>();
        }

        [Function("Welcome")]
        public IActionResult GetWelcome([HttpTrigger(AuthorizationLevel.Function, "get", Route = "Hello")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult($"Welcome to Azure Functions!");
        }

        [Function("Hello")]
        public async Task<IActionResult> GenerateGreeting([HttpTrigger(AuthorizationLevel.Function, "post", Route = "Hello")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function is processing a request.");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            if (string.IsNullOrWhiteSpace(requestBody))
            {
                return new BadRequestObjectResult("Request body cannot be empty.");
            }

            _logger.LogTrace("Hello trace, {name}", requestBody);
            _logger.LogDebug("Happy debug, {name}", requestBody);
            _logger.LogInformation("Hello, {name}", requestBody);
            _logger.LogWarning("Core temperature rising, {name}", requestBody);
            _logger.LogError("I'm afraid I can't do that, {name}", requestBody);
            _logger.LogCritical("Meltdown imminent, {name}", requestBody);

            return new OkObjectResult($"Hello, {requestBody}!");
        }
    }
}
