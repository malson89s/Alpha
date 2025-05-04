using Business.Models;
using Data.Repositories;
using Domain.Models;
using Core.Models;
namespace Business.Services;

public interface IStatusService
{
    Task<ServiceResult<Status>> GetStatusByIdAsync(int id);
    Task<ServiceResult<Status>> GetStatusByNameAsync(string statusName);
    Task<ServiceResult<IEnumerable<Status>>> GetStatusesAsync();
}

public class StatusService(IStatusRepository statusRepository) : IStatusService
{
    private readonly IStatusRepository _statusRepository = statusRepository;

    public async Task<ServiceResult<IEnumerable<Status>>> GetStatusesAsync()
    {
        var result = await _statusRepository.GetAllAsync();
        return result.Succeeded
            ? new ServiceResult<IEnumerable<Status>> { Succeeded = true, StatusCode = 200, Result = result.Result }
            : new ServiceResult<IEnumerable<Status>> { Succeeded = false, StatusCode = result.StatusCode ?? 500, Error = result.Error };
    }

    public async Task<ServiceResult<Status>> GetStatusByNameAsync(string statusName)
    {
        var result = await _statusRepository.GetAsync(x => x.StatusName == statusName);
        return result.Succeeded
            ? new ServiceResult<Status> { Succeeded = true, StatusCode = 200, Result = result.Result }
            : new ServiceResult<Status> { Succeeded = false, StatusCode = result.StatusCode ?? 500, Error = result.Error };
    }

    public async Task<ServiceResult<Status>> GetStatusByIdAsync(int id)
    {
        var result = await _statusRepository.GetAsync(x => x.Id == id);
        return result.Succeeded
            ? new ServiceResult<Status> { Succeeded = true, StatusCode = 200, Result = result.Result }
            : new ServiceResult<Status> { Succeeded = false, StatusCode = result.StatusCode ?? 500, Error = result.Error };
    }
}
