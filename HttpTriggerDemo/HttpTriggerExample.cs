using System.Net;
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
        public HttpResponseData GetWelcome([HttpTrigger(AuthorizationLevel.Function, "get", Route = "Hello")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            response.WriteString("Welcome to Azure Functions!");
            return response;
        }

        [Function("Hello")]
        public async Task<HttpResponseData> GenerateGreeting([HttpTrigger(AuthorizationLevel.Function, "post", Route = "Hello")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            if (string.IsNullOrWhiteSpace(requestBody))
            {
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }

            response.WriteString($"Hello, {requestBody}!");
            return response;
        }
    }
}
