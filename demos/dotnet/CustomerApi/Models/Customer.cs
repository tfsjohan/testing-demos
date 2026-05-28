namespace CustomerApi.Models;

// Domänobjektet vi jobbar med genom hela demot.
public class Customer
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public bool IsActive { get; set; } = true;
    public DateTime RegisteredAt { get; set; }
}
