using System;
using CMSBlazor.Client.Models;
using CMSBlazor.Shared.ViewModels.Account;
using CMSBlazor.Shared.ViewModels.Post;

namespace CMSBlazor.Client.Service.Account
{
    public interface IAccountService
    {
        Task<RegisterViewModel?> Register(RegisterViewModel registerModel);
        Task<LoginViewModel?> Login(LoginViewModel loginModel);
        void LoginStorage(LocalStorageModel localStorageModel);
        Task Logout();
        Task<ConfirmEmailViewModel?> ConfirmEmail(ConfirmEmailViewModel confirmEmailViewModel);
        Task<SendTokenEmail?> SendConfirmationEmail(SendTokenEmail sendTokenEmail);
        Task<SendTokenEmail?> ForgotPasswordbyEmail(SendTokenEmail sendTokenEmail);
        Task<ConfirmEmailViewModel?> PasswordConfirmEmail(ConfirmEmailViewModel confirmEmailViewModel);
        Task<ResetPasswordbyEmailViewModel?> ResetPasswordbyEmail(ResetPasswordbyEmailViewModel resetPasswordbyEmail);

        Task<RegisterExternalViewModel?> ExternalLogin(RegisterExternalViewModel registerExternalViewModel);


    }
}

