using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();
builder.Services.AddLogging(logging =>
 {
     logging.AddConsole();
     logging.AddFilter("Azure", LogLevel.Warning);
     logging.AddFilter("Microsoft.Extensions.Diagnostics", LogLevel.Warning);
     logging.AddFilter("HttpTriggerTimeoutDemo", LogLevel.Trace);

 });

builder.Build().Run();
