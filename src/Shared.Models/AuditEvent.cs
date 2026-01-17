using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class AuditEvent
    {
        public string EntityType { get; set; } = default!;
        public string EntityId { get; set; } = default!;
        public string Action { get; set; } = default!;
        public string PerformedBy { get; set; } = default!;
        public DateTime Timestamp { get; set; }
        public object? Metadata { get; set; }
    }
}
