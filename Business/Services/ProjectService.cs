using System.Linq.Expressions;
using Core.Models;
using Data.Entities;
using Data.Repositories;
using Domain.Dtos;
using Domain.Dtos.Extensions;
using Domain.Models;

namespace Business.Services;

public interface IProjectService
{
    Task<ServiceResult<bool>> CreateProjectAsync(AddProjectFormData formData);
    Task<ServiceResult<IEnumerable<Project>>> GetProjectsAsync();
    Task<ServiceResult<Project>> GetProjectAsync(string id);
    Task<ServiceResult<bool>> AddProjectAsync(AddProjectData dto);
}

public class ProjectService(IProjectRepository projectRepository, IStatusService statusService) : IProjectService
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IStatusService _statusService = statusService;

    public async Task<ServiceResult<bool>> CreateProjectAsync(AddProjectFormData formData)
    {
        if (formData == null)
            return new ServiceResult<bool> { Succeeded = false, StatusCode = 400, Error = "Not all required fields are supplied." };

        var projectEntity = formData.MapTo<ProjectEntity>();

        var statusResult = await _statusService.GetStatusByIdAsync(1);
        if (!statusResult.Succeeded || statusResult.Result == null)
            return new ServiceResult<bool> { Succeeded = false, StatusCode = 500, Error = "Status could not be resolved." };

        // 🔧 Fix för int? till int
        projectEntity.StatusId = statusResult.Result.Id;

        var result = await _projectRepository.AddAsync(projectEntity);

        return new ServiceResult<bool>
        {
            Succeeded = result.Succeeded,
            StatusCode = result.Succeeded ? 201 : 500,
            Result = result.Succeeded,
            Error = result.Error
        };
    }

    public async Task<ServiceResult<IEnumerable<Project>>> GetProjectsAsync()
    {
        Expression<Func<ProjectEntity, object>>[] includes = {
        p => p.Member!,
        p => p.Status!,
        p => p.Client!
    };

        var response = await _projectRepository.GetAllAsync(
            orderByDecending: true,
            sortBy: s => s.Created,
            includes: includes
        );

        return new ServiceResult<IEnumerable<Project>>
        {
            Succeeded = response.Succeeded,
            StatusCode = response.StatusCode ?? 500,
            Error = response.Error,
            Result = response.Result?.Select(p => p.MapTo<Project>()) ?? Enumerable.Empty<Project>()
        };
    }

    public async Task<ServiceResult<Project>> GetProjectAsync(string id)
    {
        Expression<Func<ProjectEntity, object>>[] includes = [
            p => p.Member!,
            p => p.Status!,
            p => p.Client!
        ];

        var response = await _projectRepository.GetAsync(
            where: x => x.Id == id,
            includes: includes
        );

        return new ServiceResult<Project>
        {
            Succeeded = response.Succeeded,
            StatusCode = response.Succeeded ? 200 : 404,
            Result = response.Result,
            Error = response.Succeeded ? null : $"Project '{id}' not found."
        };
    }

    public async Task<ServiceResult<bool>> AddProjectAsync(AddProjectData dto)
    {
        var entity = new ProjectEntity
        {
            ProjectName = dto.ProjectName,
            Description = dto.Description,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Budget = dto.Budget,
            ImageUrl = dto.ImageUrl,
            Client = new ClientEntity { ClientName = dto.Client }
        };

        var result = await _projectRepository.AddAsync(entity);

        return new ServiceResult<bool>
        {
            Succeeded = result.Succeeded,
            StatusCode = result.StatusCode ?? 500,
            Error = result.Error,
            Result = result.Succeeded
        };
    }
}

// help from chatgpt

