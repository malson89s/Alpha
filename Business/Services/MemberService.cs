using System.Diagnostics;
using Core.Models;
using Data.Entities;
using Data.Repositories;
using Domain.Dtos;
using Domain.Dtos.Extensions;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Business.Services;

public interface IMemberService
{
    Task<ServiceResult<IEnumerable<Member>>> GetMembersAsync();
    Task<ServiceResult<bool>> AddMemberToRole(string memberId, string roleName);
    Task<ServiceResult<bool>> CreateMemberAsync(SignUpFormData formData, string roleName = "Member");
}

public class MemberService(IMemberRepository memberRepository, UserManager<MemberEntity> memberManager, RoleManager<IdentityRole> roleManager) : IMemberService
{
    private readonly IMemberRepository _memberRepository = memberRepository;
    private readonly UserManager<MemberEntity> _memberManager = memberManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;

    public async Task<ServiceResult<bool>> CreateMemberAsync(SignUpFormData formData, string roleName = "Member")
    {
        if (formData == null)
            return new ServiceResult<bool> { Succeeded = false, StatusCode = 400, Error = "Form data cannot be null.", Result = false };

        var existsResult = await _memberRepository.ExistsAsync(m => m.Email == formData.Email);
        if (existsResult.Succeeded)
            return new ServiceResult<bool> { Succeeded = false, StatusCode = 409, Error = "Email already exists.", Result = false };

        try
        {
            var memberEntity = formData.MapTo<MemberEntity>();
            memberEntity.UserName = formData.Email;

            var result = await _memberManager.CreateAsync(memberEntity, formData.Password);
            if (result.Succeeded)
            {
                var addToRoleResult = await _memberManager.AddToRoleAsync(memberEntity, roleName);
                return addToRoleResult.Succeeded
                    ? new ServiceResult<bool> { Succeeded = true, StatusCode = 201, Result = true }
                    : new ServiceResult<bool> { Succeeded = false, StatusCode = 201, Error = "Member created, but not added to role.", Result = false };
            }

            return new ServiceResult<bool> { Succeeded = false, StatusCode = 500, Error = "Unable to create member.", Result = false };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new ServiceResult<bool> { Succeeded = false, StatusCode = 500, Error = ex.Message, Result = false };
        }
    }

    public async Task<ServiceResult<IEnumerable<Member>>> GetMembersAsync()
    {
        var response = await _memberRepository.GetAllAsync();

        return new ServiceResult<IEnumerable<Member>>
        {
            Succeeded = response.Succeeded,
            StatusCode = response.StatusCode ?? 500,
            Error = response.Error,
            Result = response.Result?.Select(m => m.MapTo<Member>()).ToList() ?? []
        };
    }

    public async Task<ServiceResult<bool>> AddMemberToRole(string memberId, string roleName)
    {
        if (!await _roleManager.RoleExistsAsync(roleName))
            return new ServiceResult<bool> { Succeeded = false, StatusCode = 404, Error = "Role doesn't exist.", Result = false };

        var member = await _memberManager.FindByIdAsync(memberId);
        if (member == null)
            return new ServiceResult<bool> { Succeeded = false, StatusCode = 404, Error = "Member doesn't exist.", Result = false };

        var result = await _memberManager.AddToRoleAsync(member, roleName);
        return result.Succeeded
            ? new ServiceResult<bool> { Succeeded = true, StatusCode = 200, Result = true }
            : new ServiceResult<bool> { Succeeded = false, StatusCode = 500, Error = "Unable to add member to role.", Result = false };
    }
}




