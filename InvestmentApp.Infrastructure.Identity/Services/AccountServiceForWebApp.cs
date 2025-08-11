using InvestmentApp.Core.Application.Dtos.User;
using InvestmentApp.Core.Application.Interfaces;
using InvestmentApp.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace InvestmentApp.Infrastructure.Identity.Services
{
    public class AccountServiceForWebApp : BaseAccountService, IAccountServiceForWebApp
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger _accountServiceWebAppLogger;
        public AccountServiceForWebApp(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService, ILoggerFactory loggerFactory)
            : base(userManager,emailService, loggerFactory.CreateLogger<AccountServiceForWebApp>()) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _accountServiceWebAppLogger = loggerFactory.CreateLogger<AccountServiceForWebApp>();
        }
        public async Task<LoginResponseDto> AuthenticateAsync(LoginDto loginDto)
        {
            LoginResponseDto response = new()
            {
                Email = "",
                Id = "",
                LastName = "",
                Name = "",
                UserName = "",
                HasError = false,
                Errors = []
            };

            _accountServiceWebAppLogger.LogInformation("Authenticating user {UserName} for web app access", loginDto.UserName);
            var user = await _userManager.FindByNameAsync(loginDto.UserName);

            _accountServiceWebAppLogger.LogInformation("Attempting to find user by username: {UserName}", loginDto.UserName);
            if (user == null)
            {
                _accountServiceWebAppLogger.LogWarning("No account registered with username: {UserName}", loginDto.UserName);
                response.HasError = true;
                response.Errors.Add($"There is no acccount registered with this username: {loginDto.UserName}");
                return response;
            }

            _accountServiceWebAppLogger.LogInformation("User found: {UserName} - EmailConfirmed: {EmailConfirmed}",
                user.UserName, user.EmailConfirmed);
            if (!user.EmailConfirmed)
            {
                _accountServiceWebAppLogger.LogWarning("Account {UserName} is not active, email confirmation required", loginDto.UserName);
                response.HasError = true;
                response.Errors.Add($"This account {loginDto.UserName} is not active, you should check your email");
                return response;
            }

            _accountServiceWebAppLogger.LogInformation("Attempting to sign in user: {UserName}", user.UserName);
            var result = await _signInManager.PasswordSignInAsync(user.UserName ?? "", loginDto.Password, false, true);

            _accountServiceWebAppLogger.LogInformation("Sign in result for user {UserName}: {Result}", user.UserName, result.Succeeded);
            if (!result.Succeeded)
            {
                response.HasError = true;
                if (result.IsLockedOut)
                {
                    response.Errors.Add($"Your account {loginDto.UserName} has been locked due to multiple failed attempts." +
                        $" Please try again in 10 minutes. If you don’t remember your password, you can go through the password " +
                        $"reset process.");
                }
                else
                {
                    response.Errors.Add($"these credentials are invalid for this user: {user.UserName}");
                }                    
                return response;
            }

            var rolesList = await _userManager.GetRolesAsync(user);

            response.Id = user.Id;
            response.Email = user.Email ?? "";
            response.UserName = user.UserName ?? "";
            response.Name = user.Name;
            response.LastName = user.LastName;
            response.IsVerified = user.EmailConfirmed;
            response.Roles = rolesList.ToList();

            return response;
        }
        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }       
    }
}
