using System;
using AutoMapper;
using CMSBlazor.Shared.ViewModels.Administration;
using CMSBlazor.Shared.ViewModels.Post;
using CMSBlazor.Shared.ViewModels.Profile;

namespace CMSBlazor.Client.Models
{
	public class SettingsProfile : Profile
    {
        public SettingsProfile()
		{
            CreateMap<InfoProfile, EditProfile>();
            CreateMap<EditProfile, InfoProfile>();


            CreateMap<InfoProfile, EditAccount>();
            CreateMap<EditAccount, InfoProfile>();



            CreateMap<CategoryViewModel, EditeCategoryModel>();
            CreateMap<EditeCategoryModel, CategoryViewModel>();


            CreateMap<RoleViewModel, EditRoleViewModel>();
            CreateMap<EditRoleViewModel, RoleViewModel>();



            CreateMap<UserRoleViewModel, EditUserRoleViewModel>();
            CreateMap<EditUserRoleViewModel, UserRoleViewModel>();


            CreateMap<UserViewModel, EditUserViewModel>();
            CreateMap<EditUserViewModel, UserViewModel>();


            CreateMap<UserClaimsViewModel, EditUserClaimsViewModel>();
            CreateMap<EditUserClaimsViewModel, UserClaimsViewModel>();


            CreateMap<PostViewModel, EditPostViewModel>();
            CreateMap<EditPostViewModel, PostViewModel>();


        }
	}
}

