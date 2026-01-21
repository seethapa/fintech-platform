using Shared.Models;

namespace Shared.Auth
{
    public interface IAuditClient
    {
        Task LogAsync(AuditEvent auditEvent);
    }
}