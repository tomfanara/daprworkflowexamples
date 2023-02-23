using Alegeus.Next.Workflow.Monitor.Activities;
using Alegeus.Next.Workflow.Monitor.Models;
using Dapr.Workflow;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alegeus.Next.Workflow.Monitor.Workflows
{

    public class MonitorWorkflow : Workflow<ReimbursementPayload, ReimbursementResult >
    {
       
        public override async Task<ReimbursementResult> RunAsync(WorkflowContext context, ReimbursementPayload input)
        {
            TimeSpan nextSleepInterval;

            ReimbursementResult result = await context.CallActivityAsync<ReimbursementResult>(
               nameof(GetStatusActivity),
               new ReimbursementRequest(RequestId: context.InstanceId, input.ScheduleType));


            // Put the workflow to sleep until the determined time
            nextSleepInterval = TimeSpan.FromMilliseconds(10000);
            await context.CreateTimer(nextSleepInterval);

            // Restart from the beginning with the updated state
            
            context.ContinueAsNew(input);

            return result;
        }
    }

}
