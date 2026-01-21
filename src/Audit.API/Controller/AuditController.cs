using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Shared.Models;

namespace Audit.API.Controllers;

[ApiController]
[Route("api/audit")]
public class AuditController : ControllerBase
{
    private readonly IMongoCollection<AuditEvent> _collection;

    public AuditController(IMongoClient client)
    {
        _collection = client
             .GetDatabase("auditdb")
             .GetCollection<AuditEvent>("logs");
    }

    [HttpPost]
    public async Task<IActionResult> Log([FromBody] AuditEvent auditEvent)
    {
        auditEvent.Timestamp = DateTime.UtcNow;
        await _collection.InsertOneAsync(auditEvent);
        return Ok();
    }
}
