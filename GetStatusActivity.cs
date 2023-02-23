using Microsoft.Extensions.Logging;
using Dapr.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alegeus.Next.Workflow.Monitor.Models;

namespace Alegeus.Next.Workflow.Monitor.Activities
{
    public class GetStatusActivity : WorkflowActivity<ReimbursementRequest, object>
    {
        
        readonly ILogger logger;
        public GetStatusActivity(ILoggerFactory loggerFactory)
        {
            this.logger = loggerFactory.CreateLogger<GetStatusActivity>();
        }

        public override async Task<object> RunAsync(WorkflowActivityContext context, ReimbursementRequest req)
        {
            
            this.logger.LogInformation(
                "Processing GetStatus Activity",
                req.RequestId,
                req.ScheduleType
                );

            // Simulate slow processing
            await Task.Delay(TimeSpan.FromSeconds(10));

            this.logger.LogInformation(
                "Processing reimbursements...",
                req.RequestId);
                
            ReimbursementResult res = new ReimbursementResult(true);
            
            return res.Processed;
        }



    }
}
