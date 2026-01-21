using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Shared.Models;

namespace Shared.Auth
{
    public class AuditClient : IAuditClient
    {
        private readonly HttpClient _http;

        public AuditClient(HttpClient http)
        {
            _http = http;
        }

        public async Task LogAsync(AuditEvent auditEvent)
        {
            try
            {
                await _http.PostAsJsonAsync("/api/audit", auditEvent);
            }
            catch
            {
                // IMPORTANT:
                // Audit failure must NEVER break business flow
                // Log locally if needed
            }
        }
    }
}
