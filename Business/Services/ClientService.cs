using Business.Models;
using Core.Models;
using Data.Entities;
using Data.Repositories;
using Domain.Dtos.Extensions;
using Domain.Models;

namespace Business.Services;

public interface IClientService
{
    Task<ServiceResult<IEnumerable<Client>>> GetClientsAsync();
    Task<ServiceResult<bool>> AddClientAsync(AddClientForm form);
    Task<ServiceResult<bool>> UpdateClientAsync(EditClientForm form);
}

public class ClientService : IClientService
{
    private readonly IClientRepository _clientRepository;

    public ClientService(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<ServiceResult<IEnumerable<Client>>> GetClientsAsync()
    {
        var result = await _clientRepository.GetAllAsync();
        return new ServiceResult<IEnumerable<Client>>
        {
            Succeeded = result.Succeeded,
            StatusCode = result.StatusCode ?? 500,
            Error = result.Error,
            Result = result.Result
        };
    }

    public async Task<ServiceResult<bool>> AddClientAsync(AddClientForm form)
    {
        var entity = new ClientEntity
        {
            ClientName = form.ClientName,
            Email = form.Email
        };

        var result = await _clientRepository.AddAsync(entity);

        return new ServiceResult<bool>
        {
            Succeeded = result.Succeeded,
            StatusCode = result.StatusCode ?? 500,
            Error = result.Error,
            Result = result.Succeeded
        };
    }

    public async Task<ServiceResult<bool>> UpdateClientAsync(EditClientForm form)
    {
        var getResult = await _clientRepository.GetAsync(x => x.Id == form.Id.ToString());
        if (!getResult.Succeeded || getResult.Result == null)
        {
            return new ServiceResult<bool>
            {
                Succeeded = false,
                StatusCode = 404,
                Error = "Client not found",
                Result = false
            };
        }

        var entity = form.MapTo<ClientEntity>();
        entity.Id = form.Id.ToString(); 

        var updateResult = await _clientRepository.UpdateAsync(entity);

        return new ServiceResult<bool>
        {
            Succeeded = updateResult.Succeeded,
            StatusCode = updateResult.StatusCode,
            Error = updateResult.Error,
            Result = updateResult.Succeeded
        };
    }
}

