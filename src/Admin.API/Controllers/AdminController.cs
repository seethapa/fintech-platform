using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Data;
using Shared.Models;
using Shared.Auth;
using System.Security.Claims;

namespace Admin.API.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Policy = "AdminOnly")]
public class AdminController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IAuditClient _audit;

    public AdminController(AppDbContext db, IAuditClient audit)
    {
        _db = db;
        _audit = audit;
    }

    [HttpPost("approve/{id}")]
    public async Task<IActionResult> Approve(Guid id)
    {
        var merchant = await _db.Merchants.FindAsync(id);

        if (merchant == null)
            return NotFound(new { message = "Merchant not found" });

        if (merchant.Status == "APPROVED")
            return BadRequest(new { message = "Merchant already approved" });

        merchant.Status = "APPROVED";
        await _db.SaveChangesAsync();

        // 🔍 Who performed the action
        var user = User.FindFirst(ClaimTypes.Email)?.Value
                   ?? User.Identity?.Name
                   ?? "unknown";

        // 🔐 Audit event (non-blocking)
        await _audit.LogAsync(new AuditEvent
        {
            EntityType = "Merchant",
            EntityId = merchant.Id.ToString(),
            Action = "APPROVED",
            PerformedBy = user,
            Timestamp = DateTime.UtcNow,
            Metadata = new
            {
                previousStatus = "PENDING",
                newStatus = "APPROVED"
            }
        });

        return Ok(new
        {
            message = "Merchant approved successfully",
            merchantId = merchant.Id
        });
    }
}
