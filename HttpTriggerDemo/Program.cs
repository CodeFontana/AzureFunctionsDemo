using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

IHost host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureLogging(logging =>
    {
        logging.AddConsole();
        logging.AddFilter("Host.Results", LogLevel.Error);
        logging.AddFilter("Host.Aggregator", LogLevel.Trace);
        logging.AddFilter("Function", LogLevel.Error);
        logging.AddFilter("Azure", LogLevel.Warning);
        logging.AddFilter("Microsoft.Extensions.Diagnostics", LogLevel.Warning);
        logging.AddFilter("HttpTriggerDemo", LogLevel.Trace);
    })
    .Build();

host.Run();
