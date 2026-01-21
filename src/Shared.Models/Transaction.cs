using System;

namespace Shared.Models;

public class Transaction
{
    public Guid Id { get; set; }
    public Guid MerchantId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "INR";
    public DateTime CreatedAt { get; set; }
}