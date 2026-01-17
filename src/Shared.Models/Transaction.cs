using System;

namespace Shared.Models;

public class Transaction
{
    public Guid Id { get; set; }
    public Guid MerchantId { get; set; }
    public decimal Amount { get; set; }
    public decimal Currency { get; set; }
    public DateTime CreatedAt { get; set; }
}