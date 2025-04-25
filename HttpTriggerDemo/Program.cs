using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
    })
    .ConfigureLogging(logging =>
    {
        logging.AddConsole();
        logging.AddFilter("Host.Results", LogLevel.Error);
        logging.AddFilter("Host.Aggregator", LogLevel.Trace);
        logging.AddFilter("Function", LogLevel.Error);
        logging.AddFilter("Azure", LogLevel.Warning);
        logging.AddFilter("Microsoft.Extensions.Diagnostics", LogLevel.Warning);
        logging.AddFilter("HttpTriggerDemo", LogLevel.Trace);

        logging.AddFilter<ApplicationInsightsLoggerProvider>("Host.Results", LogLevel.Error);
        logging.AddFilter<ApplicationInsightsLoggerProvider>("Host.Aggregator", LogLevel.Trace);
        logging.AddFilter<ApplicationInsightsLoggerProvider>("Function", LogLevel.Error);
        logging.AddFilter<ApplicationInsightsLoggerProvider>("Azure", LogLevel.Warning);
        logging.AddFilter<ApplicationInsightsLoggerProvider>("Microsoft.Extensions.Diagnostics", LogLevel.Warning);
        logging.AddFilter<ApplicationInsightsLoggerProvider>("HttpTriggerDemo", LogLevel.Trace);
    })
    .Build();

host.Run();
