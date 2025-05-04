using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Domain.Models;
using Core.Models;
using Data.Models;


namespace Data.Repositories;

public interface IClientRepository : IBaseRepository<ClientEntity, Client>
{
    Task<ServiceResult<bool>> UpdateAsync(ClientEntity entity);
}

public class ClientRepository : BaseRepository<ClientEntity, Client>, IClientRepository
{
    public ClientRepository(DataContext context) : base(context)
    {
    }

    public async Task<ServiceResult<bool>> UpdateAsync(ClientEntity entity)
    {
        _context.Clients.Update(entity);
        await _context.SaveChangesAsync();

        return new ServiceResult<bool>
        {
            Succeeded = true,
            StatusCode = 200,
            Result = true
        };
    }
}
