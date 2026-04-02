using System;
using CMSBlazor.Data;
using CMSBlazor.Shared.Models;
using CMSBlazor.Shared.ViewModels.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CMSBlazor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ProfileController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment env;
        private readonly UserManager<ApplicationUser> userManager;
        public ProfileController(AppDbContext context, IWebHostEnvironment env,
                                 UserManager<ApplicationUser> userManager)
		{
            _context = context;
            this.env = env;
            this.userManager = userManager;
        }


        [HttpPost]
        [Route("Index")]
        public async Task<ActionResult> GetInfoProfile(InfoProfile infoProfile)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == infoProfile.UserId);
                if (user == null)
                {
                    return Ok(new { success = false, message = "The ID " + user!.UserName + " Not Found" });
                }

                var aboutUser = await _context.AboutUsers
                    .Include(a => a.ApplicationUsers)
                    .FirstOrDefaultAsync(m => m.ApplicationUsers!.UserName == user.UserName);
                if (aboutUser == null)
                {
                    return Ok(new { success = false, message = "The aboutUser " + user.UserName + " Not Found" });
                }
                return Ok(new { success = true, message = "The aboutUser", aboutUser });
            }
            return Ok(new { success = false, message = "ModelState IsValid" });

        }




        [HttpPost]
        [Route("EditProfile")]
        public async Task<ActionResult> EditProfile(InfoProfile infoProfile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var aboutUserId = _context.AboutUsers.Where(e => e.ApplicationUsers!.Id == infoProfile.UserId)
                        .Select(e => e.AboutUserId).FirstOrDefault();
                    if (aboutUserId == null)
                    {
                        return Ok(new { success = false, message = "AboutUser Null" });
                    }


                    string nameImgProfile = string.Empty;
                    if (infoProfile!.UrlImageProfile != null) {
                        string extension = Path.GetExtension(infoProfile!.UrlImageProfileName!);
                        nameImgProfile = $"{aboutUserId}ImageProfile{Guid.NewGuid()}{extension}";
                        var buf = Convert.FromBase64String(infoProfile!.UrlImageProfile!);
                        await System.IO.File.WriteAllBytesAsync(env.ContentRootPath + "/wwwroot/profile/" + nameImgProfile, buf);
                        if (infoProfile.UrlImageProfileNameOld !=null) {
                            System.IO.File.Delete(env.ContentRootPath + "/wwwroot/profile/" + infoProfile.UrlImageProfileNameOld);
                        }
                    }


                    string nameImgCover = string.Empty;
                    if (infoProfile!.UrlImageCover != null)
                    {
                        string extension = Path.GetExtension(infoProfile!.UrlImageCoverName!);
                        nameImgCover = $"{aboutUserId}ImageCover{Guid.NewGuid()}{extension}";
                        var buf = Convert.FromBase64String(infoProfile!.UrlImageCover!);
                        await System.IO.File.WriteAllBytesAsync(env.ContentRootPath + "/wwwroot/profile/" + nameImgCover, buf);
                        if (infoProfile.UrlImageCoverNameOld != null)
                        {
                            System.IO.File.Delete(env.ContentRootPath + "/wwwroot/profile/" + infoProfile.UrlImageCoverNameOld);
                        }
                    }



                    var _aboutUser = await _context.AboutUsers.FindAsync(aboutUserId);
                    if (_aboutUser != null)
                    {
                        _aboutUser.Profession = infoProfile.Profession;
                        _aboutUser.DateOfBurth = infoProfile.DateOfBurthEdite;
                        _aboutUser.Location = infoProfile.Location;
                        _aboutUser.Skills = infoProfile.Skills;
                        _aboutUser.WorkLink = infoProfile.WorkLink;
                        _aboutUser.Experience = infoProfile.Experience;

                        if (infoProfile!.UrlImageProfile !=null)
                        {
                            _aboutUser.UrlImageProfile = nameImgProfile;
                        }
                        if (infoProfile!.UrlImageCover != null)
                        {
                            _aboutUser.UrlImageCover = nameImgCover;
                        }


                        _context.AboutUsers.Attach(_aboutUser);
                        _context.Entry(_aboutUser).Property(x => x.Profession).IsModified = true;
                        _context.Entry(_aboutUser).Property(x => x.DateOfBurth).IsModified = true;
                        _context.Entry(_aboutUser).Property(x => x.Location).IsModified = true;
                        _context.Entry(_aboutUser).Property(x => x.Skills).IsModified = true;
                        _context.Entry(_aboutUser).Property(x => x.WorkLink).IsModified = true;
                        _context.Entry(_aboutUser).Property(x => x.Experience).IsModified = true;

                        if (infoProfile!.UrlImageProfile != null)
                        {
                            _context.Entry(_aboutUser).Property(x => x.UrlImageProfile).IsModified = true;
                        }
                        if (infoProfile!.UrlImageCover != null)
                        {
                            _context.Entry(_aboutUser).Property(x => x.UrlImageCover).IsModified = true;
                        }

                        await _context.SaveChangesAsync();
                    }
                    return Ok(new { success = true, message = "Succeeded Update" });
                }
                catch (Exception ex)
                {
                    return Ok(new { exception = ex.Message.ToString() });
                }
            }
            return Ok(new { success = false, message = "ModelState IsValid" });
        }




        [HttpPost]
        [Route("EditAccount")]
        public async Task<IActionResult> EditAccount(EditAccount model)
        {
            var userid = _context.Users.Where(e => e.Id == model.UserId)
               .Select(e => e.Id).FirstOrDefault();

            if (userid == null)
            {
                return Ok(new { success = false, message = "User with Id " + userid + " cannot be found" });
            }
            var user = await userManager.FindByIdAsync(userid);
            if (user == null)
            {
                return Ok(new { success = false, message = "User with Id " + userid + " cannot be found" });
            }
            else
            {
                if (user.Email == model.Email && user.PhoneNumber == model.PhoneNumber)
                {
                    user.Email = model.Email;
                    user.UserName = model.UserName;
                    user.PhoneNumber = model.PhoneNumber;
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    if (user.PasswordHash != null)
                    {
                        var password = userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password!);
                        if (password != PasswordVerificationResult.Success)
                        {
                            return Ok(new { success = false, message = "Password Error" });
                        }
                    }

                    var result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return Ok(new { success = true, message = "Succeeded Update", user });
                    }

                    foreach (var error in result.Errors)
                    {
                        return Ok(new { success = false, message = error.Description });
                    }
                }
                else
                {
                    if (user.Email != model.Email)
                    {
                        if (userManager.Users.Any(e => e.Email == model.Email) == true)
                        {
                            return Ok(new { success = false, message = "Email " + model.Email + " is already taken." });
                        }
                    }
                    if (user.PhoneNumber != model.PhoneNumber)
                    {
                        if (userManager.Users.Any(e => e.PhoneNumber == model.PhoneNumber) == true && model.PhoneNumber != null)
                        {
                            return Ok(new { success = false, message = "PhoneNumber " + model.PhoneNumber + " is already taken." });
                        }
                    }
                    user.Email = model.Email;
                    user.UserName = model.UserName;
                    user.PhoneNumber = model.PhoneNumber;
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;

                    if (user.PasswordHash != null)
                    {
                        var password = userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password!);
                        if (password != PasswordVerificationResult.Success)
                        {
                            return Ok(new { success = false, message = "Password Error" });
                        }
                    }
                    var result = await userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        return Ok(new { success = true, message = "Succeeded Update", user });
                    }

                    foreach (var error in result.Errors)
                    {
                        return Ok(new { success = false, message = error.Description });
                    }
                }
            }
            return Ok(new { success = false, message = "ModelState IsValid" });

        }






    }
}

