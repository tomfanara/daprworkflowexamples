using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alegeus.Next.Workflow.Monitor.Models
{
    public record Notification(string Message);
    public record ReimbursementPayload(string ScheduleType);
    public record ReimbursementRequest(string RequestId, string ScheduleType);
    public record ReimbursementResult(bool Processed);
}
