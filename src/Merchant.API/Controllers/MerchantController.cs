using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.Models;

namespace Merchant.API.Controllers;

[ApiController]
[Route("api/merchant")]
[Authorize] // Any authenticated merchant
public class MerchantController : ControllerBase
{
    private readonly AppDbContext _db;

    public MerchantController(AppDbContext db)
    {
        _db = db;
    }

    // POST api/merchant/register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] Shared.Models.Merchant merchant)
    {
        merchant.Id = Guid.NewGuid();
        merchant.Status = "PENDING";
        merchant.CreatedAt = DateTime.UtcNow;

        _db.Merchants.Add(merchant);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = merchant.Id }, merchant);
    }

    // GET api/merchant/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var merchant = await _db.Merchants.FindAsync(id);
        if (merchant == null)
            return NotFound();

        return Ok(merchant);
    }

    // GET api/merchant
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _db.Merchants.ToListAsync());
    }
}
