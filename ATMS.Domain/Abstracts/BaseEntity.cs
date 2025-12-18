
using ATMS.Domain.Common;

namespace ATMS.Domain.Abstracts;

public abstract class BaseEntity<TId> : IEquatable<BaseEntity<TId>>, IHasDomainEvents where TId : notnull
{
    private readonly List<IDomainEvent> _domainEvents = new();
    public TId Id { get; set; }
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
    protected BaseEntity(TId id)
    {
        Id = id;
    }
    protected BaseEntity() { }
    public override bool Equals(object? obj)
    {
        if (obj is not BaseEntity<TId> entity)
            return false;

        // هر دو Id null هستند
        if (Id is null && entity.Id is null)
            return true;

        // فقط یکی از Idها null است
        if (Id is null || entity.Id is null)
            return false;

        // هر دو Id مقدار دارند
        return Id.Equals(entity.Id);
    }

    public bool Equals(BaseEntity<TId>? other)
    {
        return Equals((object?)other);
    }

    public static bool operator ==(BaseEntity<TId> left, BaseEntity<TId> right)
    {
        return left.Equals(right);
    }
    public static bool operator !=(BaseEntity<TId> left, BaseEntity<TId> right)
    {
        return !left.Equals(right);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
