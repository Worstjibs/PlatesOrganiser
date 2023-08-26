namespace PlatesOrganiser.Domain.Repositories;

public interface IUnitOfWork
{
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
}
