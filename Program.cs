using Alegeus.Next.Worflow.Scheduler.Activities;
using Alegeus.Next.Workflow.Monitor.Activities;
using Alegeus.Next.Workflow.Monitor.Models;
using Alegeus.Next.Workflow.Monitor.Workflows;
using Dapr.Client;
using Dapr.Workflow;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


// The workflow host is a background service that connects to the sidecar over gRPC
var builder = Host.CreateDefaultBuilder(args).ConfigureServices(services =>
{
    services.AddDaprWorkflow(options =>
    {
        // Note that it's also possible to register a lambda function as the workflow
        // or activity implementation instead of a class.
        options.RegisterWorkflow<MonitorWorkflow>();

        // These are the activities that get invoked by the workflow(s).
        options.RegisterActivity<NotifyActivity>();
        options.RegisterActivity<GetStatusActivity>();

    });
});

// Dapr uses a random port for gRPC by default. If we don't know what that port
// is (because this app was started separate from dapr), then assume 4001.
if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DAPR_GRPC_PORT")))
{
    Environment.SetEnvironmentVariable("DAPR_GRPC_PORT", "4002");
}

Console.ForegroundColor = ConsoleColor.White;
Console.WriteLine("*** Welcome to the Dapr Workflow console app Monitor!");
Console.WriteLine("*** Using this app, you can place reimbursement batches that start workflows.");
Console.WriteLine("*** Ensure that Dapr is running in a separate terminal window using the following command:");
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("        dapr run --dapr-grpc-port 4002 --app-id wfapp");
Console.WriteLine();
Console.ResetColor();

// Start the app - this is the point where we connect to the Dapr sidecar to
// listen for workflow work-items to execute.
using var host = builder.Build();
host.Start();

using var daprClient = new DaprClientBuilder().Build();

// Wait for the sidecar to become available
while (!await daprClient.CheckHealthAsync())
{
    Thread.Sleep(TimeSpan.FromSeconds(5));
}

// Wait one more second for the workflow engine to finish initializing.
// This is just to make the log output look a little nicer.
Thread.Sleep(TimeSpan.FromSeconds(1));

// NOTE: WorkflowEngineClient will be replaced with a richer version of DaprClient
//       in a subsequent SDK release. This is a temporary workaround.
WorkflowEngineClient workflowClient = host.Services.GetRequiredService<WorkflowEngineClient>();

//Console.WriteLine($"Starting while loop");

while (true)
{
    Console.WriteLine($"Enter initialized state for monitor workflow");

    string? init = Console.ReadLine()?.Trim();

    if (string.IsNullOrEmpty(init))
    {
        continue;
    }

    string monitorId = $"{Guid.NewGuid().ToString()[..8]}";
    var initInfo = new ReimbursementPayload(init.ToLowerInvariant());

    //Start the workflow using the reimb ID as the workflow ID
    Console.WriteLine($"Starting monitor workflow");
    await workflowClient.ScheduleNewWorkflowAsync(
        name: nameof(MonitorWorkflow),
        instanceId: monitorId,
        input: initInfo);
    Console.WriteLine($"Started monitor workflow");
    Console.WriteLine();
}
