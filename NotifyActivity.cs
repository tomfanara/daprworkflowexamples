using Microsoft.Extensions.Logging;
using Dapr.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alegeus.Next.Worflow.Scheduler.Activities
{

    public record Notification(string Message);
    public class NotifyActivity : WorkflowActivity<Notification, object>
    {
        readonly ILogger logger;

        public NotifyActivity(ILoggerFactory loggerFactory)
        {
            this.logger = loggerFactory.CreateLogger<NotifyActivity>();
        }

        public override Task<object> RunAsync(WorkflowActivityContext context, Notification notification)
        {
            this.logger.LogInformation(notification.Message);

            return Task.FromResult<object>(null);
        }
    }
}
