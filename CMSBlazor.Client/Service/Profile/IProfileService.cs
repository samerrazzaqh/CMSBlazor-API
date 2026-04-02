using System;
using CMSBlazor.Shared.ViewModels.Account;
using CMSBlazor.Shared.ViewModels.Post;
using CMSBlazor.Shared.ViewModels.Profile;

namespace CMSBlazor.Client.Service.Profile
{
	public interface IProfileService
	{
        Task<InfoProfile?> GetInfoProfile(string userId);
        Task<InfoProfile?> EditInfoProfile(InfoProfile infoProfile);
        Task<EditAccount?> EditAccount(EditAccount editAccount);
    }
}

