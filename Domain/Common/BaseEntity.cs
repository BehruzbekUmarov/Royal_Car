namespace Domain.Common;

public class BaseEntity : IHasAudit, IHasIsDeleted
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? CreatedAtUtc { get; set; }
    public string? CreatedById { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }
    public string? UpdatedById { get; set; }
}
