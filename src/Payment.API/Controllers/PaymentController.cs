using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.Models;

namespace Payment.API.Controllers;

[ApiController]
[Route("api/payment")]
[Authorize] // Authenticated services / merchants
public class PaymentController : ControllerBase
{
    private readonly AppDbContext _db;

    public PaymentController(AppDbContext db)
    {
        _db = db;
    }

    // POST api/payment/charge
    [HttpPost("charge")]
    public async Task<IActionResult> Charge([FromBody] Transaction transaction)
    {
        // Validate merchant
        var merchant = await _db.Merchants
            .FirstOrDefaultAsync(m => m.Id == transaction.MerchantId && m.Status == "APPROVED");

        if (merchant == null)
            return BadRequest("Merchant not approved");

        transaction.Id = Guid.NewGuid();
        transaction.CreatedAt = DateTime.UtcNow;

        _db.Transactions.Add(transaction);
        await _db.SaveChangesAsync();

        return Ok(transaction);
    }

    // GET api/payment/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTransaction(Guid id)
    {
        var tx = await _db.Transactions.FindAsync(id);
        if (tx == null)
            return NotFound();

        return Ok(tx);
    }
}
