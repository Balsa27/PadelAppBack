namespace PadelApp.Persistance.Outbox;

public class OutboxMessage
{
    public Guid Id { get; set; }
    public string Type { get; set; } = null!;
    public string Content { get; set; } = null!;
    public DateTime OccurredOn { get; set; }
    public DateTime? ProcessedDate { get; set; }
    public string Error { get; set; } = string.Empty;
}