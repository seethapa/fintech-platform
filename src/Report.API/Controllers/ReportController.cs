using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Data;

namespace Report.API.Controllers;

[ApiController]
[Route("api/report")]
[Authorize(Policy = "ReportAccess")] // SUPER_ADMIN + AUDITOR
public class ReportController : ControllerBase
{
    private readonly AppDbContext _db;

    public ReportController(AppDbContext db)
    {
        _db = db;
    }

    // GET api/report/transactions
    [HttpGet("transactions")]
    public async Task<IActionResult> GetTransactions()
    {
        var data = await _db.Transactions
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => new
            {
                t.Id,
                t.MerchantId,
                t.Amount,
                t.Currency,
                t.CreatedAt
            })
            .ToListAsync();

        return Ok(data);
    }

    // GET api/report/summary
    [HttpGet("summary")]
    public async Task<IActionResult> Summary()
    {
        var total = await _db.Transactions.SumAsync(t => t.Amount);
        var count = await _db.Transactions.CountAsync();

        return Ok(new
        {
            totalAmount = total,
            transactionCount = count
        });
    }
}