using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace HttpTriggerTimeoutDemo;

public class Function1
{
    private readonly ILogger<Function1> _logger;

    public Function1(ILogger<Function1> logger)
    {
        _logger = logger;
    }

    [Function("TimeoutDemoOne")]
    public async Task<IActionResult> TimeoutDemoOneAsync([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        await Task.Delay(7000);
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
    }

    [Function("TimeoutDemoTwo")]
    public async Task<IActionResult> TimeoutDemoTwoAsync([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        _ = Task.Run(async () =>
        {
            await Task.Delay(7000);
            _logger.LogInformation("C# HTTP trigger function processed a request.");
        });

        return new AcceptedResult();
    }
}