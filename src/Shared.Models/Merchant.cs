using System;

namespace Shared.Models;

public class Merchant
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Status { get; set; } // PENDING / APPROVED
    public DateTime CreatedAt { get; set; }
}