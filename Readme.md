# AzureFunctionsDemo

Small collection of Azure Functions demos for HTTP triggers and Durable Functions orchestration patterns.

## Projects

- `HttpTriggerDemo`: Basic HTTP-triggered functions.
- `HttpTriggerTimeoutDemo`: HTTP trigger timeout behavior demo (`host.json` sets `functionTimeout` to `00:00:05`).
- `DurableFunctionDemo`: Durable Functions orchestration with multiple activity calls.
- `DurableFunctionTimeoutDemo`: Durable timeout demo (`host.json` sets `functionTimeout` to `00:00:10` for demonstration).

## Durable Function note

For longer workflows, split work into smaller activity calls (and, when needed, sub-orchestrations) rather than one long-running step. This keeps units of work short and more resilient, and each scheduled activity/sub-orchestration execution gets its own runtime window instead of relying on a single long execution path.

## HTTP trigger note

Avoid fire-and-forget/background thread work from an HTTP-triggered function. If the function returns before that work completes, runtime shutdown can interrupt it and leave work in an unknown state. See Microsoft guidance: [Make sure background tasks complete](https://learn.microsoft.com/en-us/azure/azure-functions/performance-reliability#make-sure-background-tasks-complete).
