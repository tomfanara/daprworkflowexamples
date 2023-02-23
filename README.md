# daprworkflowexamples
for testing only and debugging

I'm running these in multi-app as follows but I'm on windoes so I don't use the dapr.yml yet but run as below in terminal command prompt

dotnet run


dapr run --app-id AlegeusNextWorkflowScheduler --app-port 80 --dapr-grpc-port 4001 --dapr-http-port 3500 --log-level debug

dapr run --app-id AlegeusNextWorkflowMonitor --app-port 80 --dapr-grpc-port 4002 --dapr-http-port 3501 --log-level debug

dapr run --app-id AlegeusNextWorkflowFanout --app-port 80 --dapr-grpc-port 4003 --dapr-http-port 3502 --log-level debug
