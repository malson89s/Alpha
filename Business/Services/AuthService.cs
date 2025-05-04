using Data.Entities;
using Domain;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Core.Models;

namespace Business.Services
{
    public interface IAuthService
    {
        Task<ServiceResult<bool>> LoginAsync(MemberLoginForm form);
        Task LogoutAsync();
        Task SignInAsync(MemberLoginForm form);
        Task<ServiceResult<bool>> SignUpAsync(MemberSignUpForm form);
    }

    public class AuthService(SignInManager<MemberEntity> signInManager, UserManager<MemberEntity> userManager) : IAuthService
    {
        private readonly SignInManager<MemberEntity> _signInManager = signInManager;
        private readonly UserManager<MemberEntity> _userManager = userManager;

        // Login implementation
        public async Task<ServiceResult<bool>> LoginAsync(MemberLoginForm loginForm)
        {
            var result = await _signInManager.PasswordSignInAsync(loginForm.Email, loginForm.Password, false, false);

            return new ServiceResult<bool>
            {
                Succeeded = result.Succeeded,
                StatusCode = result.Succeeded ? 200 : 401,
                Error = result.Succeeded ? null : "Invalid credentials.",
                Result = result.Succeeded
            };
        }

        // Logout implementation
        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        // SignIn implementation
        public async Task SignInAsync(MemberLoginForm form)
        {
            var user = await _userManager.FindByEmailAsync(form.Email);

            if (user != null)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
            }
            else
            {
                throw new InvalidOperationException("User not found.");
            }
        }

        // SignUp implementation
        public async Task<ServiceResult<bool>> SignUpAsync(MemberSignUpForm signupForm)
        {
            if (string.IsNullOrEmpty(signupForm.Email))
            {
                return new ServiceResult<bool>
                {
                    Succeeded = false,
                    StatusCode = 400,
                    Error = "Email is required.",
                    Result = false
                };
            }

            var existingUser = await _userManager.FindByEmailAsync(signupForm.Email);
            if (existingUser != null)
            {
                return new ServiceResult<bool>
                {
                    Succeeded = false,
                    StatusCode = 409,
                    Error = "Email already in use.",
                    Result = false
                };
            }

            var memberEntity = new MemberEntity
            {
                UserName = signupForm.Email,
                FirstName = signupForm.FirstName,
                LastName = signupForm.LastName,
                Email = signupForm.Email,
                PhoneNumber = signupForm.Phone
            };

            var result = await _userManager.CreateAsync(memberEntity, signupForm.Password);

            if (result.Succeeded)
            {
                await SignInAsync(new MemberLoginForm { Email = signupForm.Email, Password = signupForm.Password });

                return new ServiceResult<bool>
                {
                    Succeeded = true,
                    StatusCode = 201,
                    Result = true
                };
            }

            var errorMessages = result.Errors.Select(e => e.Description);
            return new ServiceResult<bool>
            {
                Succeeded = false,
                StatusCode = 500,
                Error = string.Join("; ", errorMessages),
                Result = false
            };
        }
    }
}
