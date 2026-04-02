using System;
using CMSBlazor.Data;
using CMSBlazor.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CMSBlazor.Shared.ViewModels.Account;
using CMSBlazor.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;

namespace CMSBlazor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = "Bearer")]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<AccountController> logger;
        private readonly AppDbContext _context;
        private readonly JWTSettings _jwtsettings;

        //string Idp, Emailp, FirstNamep, LastNamep, FullNamep;

        public AccountController(
                               IConfiguration configuration,
                               AppDbContext context,
                               RoleManager<IdentityRole> roleManager,
                               UserManager<ApplicationUser> userManager,
                               SignInManager<ApplicationUser> signInManager,
                               ILogger<AccountController> logger,
                               IOptions<JWTSettings> jwtsettings)
        {
            _configuration = configuration;
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            _context = context;
            _jwtsettings = jwtsettings.Value;

        }





        [HttpPost]
        [Route("Register")]
        [AllowAnonymous]
        public async Task<ActionResult<RegisterViewModel>> Register(RegisterViewModel model)
        {
            string tokenfromEmail, subject, messageBody;
            //HttpResponseMessage
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                };
                // Check Email is Found
                var checkemail = userManager.Users.Any(e => e.Email == model.Email);
                if (checkemail)
                {
                    return Ok(new { success = false, message = "Email '" + model.Email + "' is already taken." });
                }
                // Check Email is Found
                var checkusername = userManager.Users.Any(e => e.UserName == model.UserName);
                if (checkusername)
                {
                    return Ok(new { success = false, message = "UserName '" + model.UserName + "' is already taken." });
                }


                if (_context.Roles.Count() <= 0)
                {
                    //create a new role
                    IdentityRole identityRole = new IdentityRole
                    {
                        Name = "SuperAdmin",

                    };
                    await roleManager.CreateAsync(identityRole);
                }
                if (_context.Roles.Count() == 1)
                {
                    //create a new role
                    IdentityRole identityRole = new IdentityRole
                    {
                        Name = "Admin"

                    };
                    await roleManager.CreateAsync(identityRole);
                }
                if (_context.Roles.Count() == 2)
                {
                    //create a new role
                    IdentityRole identityRole = new IdentityRole
                    {
                        Name = "User"

                    };
                    await roleManager.CreateAsync(identityRole);
                }
                //==============================================================================
                subject = "Confirm your email";
                messageBody = "Hello " + model.UserName + "<br />";
                messageBody += "Please get this code below to activate your account" + "<br />" + "<br />";
                tokenfromEmail = SendEmail.sendEmail(model.Email!, subject, messageBody)!;
                if (tokenfromEmail != null)
                {
                    await userManager.CreateAsync(user, model.Password!);
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    //==============================================================================

                    if (_context.Users.Count() == 1)
                    {
                        //Create new User Role For new User
                        await userManager.AddToRoleAsync(user, "SuperAdmin");
                        //Add all the claims that are selected on the UI
                        await userManager.AddClaimAsync(user, new Claim("Edit Role", "true"));
                        await userManager.AddClaimAsync(user, new Claim("Delete Role", "true"));
                        await userManager.AddClaimAsync(user, new Claim("Create Role", "true"));

                    }
                    if (_context.Users.Count() == 2)
                    {
                        //Create new User Role For new User
                        await userManager.AddToRoleAsync(user, "Admin");
                        //Add all the claims that are selected on the UI
                        await userManager.AddClaimAsync(user, new Claim("Edit Role", "false"));
                        await userManager.AddClaimAsync(user, new Claim("Delete Role", "false"));
                        await userManager.AddClaimAsync(user, new Claim("Create Role", "false"));
                    }
                    if (_context.Users.Count() > 2)
                    {
                        //Create new User Role For new User
                        await userManager.AddToRoleAsync(user, "User");
                        //Add all the claims that are selected on the UI
                        await userManager.AddClaimAsync(user, new Claim("Edit Role", "false"));
                        await userManager.AddClaimAsync(user, new Claim("Delete Role", "false"));
                        await userManager.AddClaimAsync(user, new Claim("Create Role", "false"));
                    }

                    //------------------------------------------
                    var about = new AboutUser
                    {
                        UserId = user.Id,
                        DateOfBurth= DateTime.Now
                    };
                    _context.Add(about);
                    await _context.SaveChangesAsync();
                    //---------------Token---------------------------
                    var generateToken = new GenerateToken
                    {
                        UserId = user.Id,
                        TokenEmailConfirmation = token,
                        TokenFromEmail = tokenfromEmail
                    };
                    _context.Add(generateToken);
                    await _context.SaveChangesAsync();

                    //------------------------------------------

                    if (signInManager.IsSignedIn(User) && User.IsInRole("SuperAdmin"))
                    {
                        return Ok(new
                        {
                            success = true,
                            token,
                            model.FirstName,
                            email = model.Email,
                            message = "User Register Successfully With IsSignedIn"
                        });

                    }
                    else
                    {
                        return Ok(new
                        {
                            success = true,
                            token,
                            model.FirstName,
                            email = model.Email,
                            message = "User Register Successfully"
                        });
                    }

                }
                else
                {
                    return Ok(new { success = false, message = "Check the connection" });
                }


            }

            return Ok(new { success = false, Message = "(" + model.Email + ") or (" + model.UserName + ") Erroe" });
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginViewModel>> Login(LoginViewModel model)
        {

            //model.ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var user = await userManager.Users.FirstOrDefaultAsync(
                    u => u.UserName == model.EmailOrUserName ||
                    u.Email == model.EmailOrUserName);


                if (user == null)
                {
                    return Ok(new { success = false, message = "Not Found Email Or UserName" });
                }

                //var user = await userManager.FindByEmailAsync(model.Email);

                if (user != null && !user.EmailConfirmed &&
                                    (await userManager.CheckPasswordAsync(user, model.Password!)))
                {
                    //IsConfim = true;
                    return Ok(new { success = false, message = "Email not confirmed yet" });
                }

                if (user!.Block)
                {
                    return Ok(new { success = false, user.Email, message = "your account is Blocked" });
                }
                //if (DateTime.Now <= user.BlockTime)
                //{
                //    if (user.Block == true)
                //    {
                //        ModelState.AddModelError(string.Empty, "Your Account is Block ! ");
                //        return View(model);
                //    }
                //}
                //else
                //{
                //    user.Block = false;
                //    user.BlockTime = null;
                //    await userManager.UpdateAsync(user);
                //}

                var result = await signInManager.PasswordSignInAsync(user!, model.Password!,
                                        model.RememberMe, true);

                if (result.Succeeded)
                {
                    var aboutuserid = _context.AboutUsers.Where(e => e.ApplicationUsers!.Id == user!.Id)
                                       .Select(e => e.AboutUserId).FirstOrDefault();
                    var aboutuserimgprofile = _context.AboutUsers.Where(e => e.ApplicationUsers!.Id == user!.Id)
                                       .Select(e => e.UrlImageProfile).FirstOrDefault();
                    var aboutuserimgcover = _context.AboutUsers.Where(e => e.ApplicationUsers!.Id == user!.Id)
                                       .Select(e => e.UrlImageCover).FirstOrDefault();

                    var roleId = _context.UserRoles.Where(e => e.UserId == user!.Id)
                                      .Select(e => e.RoleId).FirstOrDefault();

                    var roleName = await roleManager.FindByIdAsync(roleId!);


                    //sign your token here here..
                    
                    // var user = new ApplicationUser
                    // {
                    //     UserName = model.UserName,
                    //     Email = model.Email,
                    //     FirstName = model.FirstName,
                    //     LastName = model.LastName,
                    // };
                    
                    var refreshToken = new RefreshTokenRequestDto
                    {
                        UserId = user.Id,
                        RefreshToken = user.RefreshToken,
                    };
                    
                    // var token = RefreshTokensAsync(refreshToken);
                    var token = GenerateAccessToken(user!.Email!, roleName!.ToString());
                    return Ok(new { success = true, message = "SignedIn", token, user.UserName, user.FirstName, user.LastName, user.Email, user.Id, aboutuserid, aboutuserimgprofile , aboutuserimgcover });
                }

                if (user!.AccessFailedCount > 0)
                {
                    int attempts = 5 - user.AccessFailedCount;
                    return Ok(new { success = false, message = "The remaining  (" + attempts + ")  attempts to block your account" });
                }

                if (result.IsLockedOut)
                {
                    return Ok(new { success = false, message = "Block your account!! Wait a day or use forgotten password" });
                }

            }

            return Ok(new { success = false, Message = "Check Your Information Accout !" });
        }

        [HttpPost]
        [Route("ExternalLogin")]
        [AllowAnonymous]
        public async Task<ActionResult<RegisterExternalViewModel>> ExternalLogin(RegisterExternalViewModel model)
        {
            string tokenfromEmail, subject, messageBody;
            if (ModelState.IsValid)
            {
                if (model.Email == null)
                {
                    return Ok(new { success = false, message = "This social account does not contain an Email " });
                }

                var user = await userManager.Users.FirstOrDefaultAsync(u => u.Email == model.Email);


                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = model.Email,
                        Email = model.Email,
                    };
                   

                    //==============================================================================
                    subject = "Confirm your email";
                    messageBody = "Hello " + user.UserName + "<br />";
                    messageBody += "Please get this code below to activate your account" + "<br />" + "<br />";
                    tokenfromEmail = SendEmail.sendEmail(user.Email!, subject, messageBody)!;
                    if (tokenfromEmail != null)
                    {


                        if (_context.Roles.Count() <= 0)
                        {
                            //create a new role
                            IdentityRole identityRole = new IdentityRole
                            {
                                Name = "SuperAdmin"
                            };
                            await roleManager.CreateAsync(identityRole);
                        }
                        if (_context.Roles.Count() == 1)
                        {
                            //create a new role
                            IdentityRole identityRole = new IdentityRole
                            {
                                Name = "Admin"
                            };
                            await roleManager.CreateAsync(identityRole);
                        }
                        if (_context.Roles.Count() == 2)
                        {
                            //create a new role
                            IdentityRole identityRole = new IdentityRole
                            {
                                Name = "User"
                            };
                            await roleManager.CreateAsync(identityRole);
                        }
                        //==============================================================================

                        await userManager.CreateAsync(user);
                        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                        //==============================================================================

                        if (_context.Users.Count() == 1)
                        {
                            //Create new User Role For new User
                            await userManager.AddToRoleAsync(user, "SuperAdmin");
                            //Add all the claims that are selected on the UI
                            await userManager.AddClaimAsync(user, new Claim("Edit Role", "true"));
                            await userManager.AddClaimAsync(user, new Claim("Delete Role", "true"));
                            await userManager.AddClaimAsync(user, new Claim("Create Role", "true"));

                        }
                        if (_context.Users.Count() == 2)
                        {
                            //Create new User Role For new User
                            await userManager.AddToRoleAsync(user, "Admin");
                            //Add all the claims that are selected on the UI
                            await userManager.AddClaimAsync(user, new Claim("Edit Role", "false"));
                            await userManager.AddClaimAsync(user, new Claim("Delete Role", "false"));
                            await userManager.AddClaimAsync(user, new Claim("Create Role", "false"));
                        }
                        if (_context.Users.Count() > 2)
                        {
                            //Create new User Role For new User
                            await userManager.AddToRoleAsync(user, "User");
                            //Add all the claims that are selected on the UI
                            await userManager.AddClaimAsync(user, new Claim("Edit Role", "false"));
                            await userManager.AddClaimAsync(user, new Claim("Delete Role", "false"));
                            await userManager.AddClaimAsync(user, new Claim("Create Role", "false"));
                        }

                        //------------------------------------------
                        var about = new AboutUser
                        {
                            UserId = user.Id,
                            DateOfBurth = DateTime.Now
                        };
                        _context.Add(about);
                        await _context.SaveChangesAsync();
                        //---------------Token---------------------------
                        var generateToken = new GenerateToken
                        {
                            UserId = user.Id,
                            TokenEmailConfirmation = token,
                            TokenFromEmail = tokenfromEmail
                        };
                        _context.Add(generateToken);
                        await _context.SaveChangesAsync();

                        //------------------------------------------
                        return Ok(new
                        {
                            success = false,
                            email = user.Email,
                            message = "Register"
                        });

                    }
                    else
                    {
                        return Ok(new { success = false, message = "Check the connection" });
                    }

                }//user null-----------------------------------------------------

                //var user = await userManager.FindByEmailAsync(model.Email);

                if (!user.EmailConfirmed)
                {
                    return Ok(new { success = false, user.Email, message = "You must confirm your email" });
                }

                if (user.Block)
                {
                    return Ok(new { success = false, user.Email, message = "your account is Blocked" });
                }



                if (user != null)
                {
                    var aboutuserid = _context.AboutUsers.Where(e => e.ApplicationUsers!.Id == user.Id)
                                       .Select(e => e.AboutUserId).FirstOrDefault();

                    var roleId = _context.UserRoles.Where(e => e.UserId == user!.Id)
                                      .Select(e => e.RoleId).FirstOrDefault();

                    var roleName = await roleManager.FindByIdAsync(roleId!);

                    //sign your token here here..
                    var token = GenerateAccessToken(user!.Email!, roleName!.ToString());
                    return Ok(new { success = true, message = "SignedIn", token, user.UserName, user.FirstName, user.LastName, user.Email, user.Id, aboutuserid });
                }


            }

            return Ok(new { success = false, Message = "Check Your Information Accout !" });
        }


        [HttpGet]
        [Route("Logout")]
        [AllowAnonymous]
        public async Task<ActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Ok(new { success = true, message = "Logout" });
        }


        [HttpPost]
        [Route("SendConfirmationEmail")]
        [AllowAnonymous]
        public async Task<ActionResult> SendConfirmationEmail(SendTokenEmail model)
        {
            string tokenfromEmail, subject, messageBody;
            if (ModelState.IsValid)
            {
                // Find the user by email
                var user = await userManager.FindByEmailAsync(model.Email!);

                if (user == null)
                {
                    return Ok(new { success = false, message = "Email not found" });
                }
                // If the user is found AND Email is confirmed
                if (user != null && !await userManager.IsEmailConfirmedAsync(user))
                {
                    // Generate the reset password token
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var generateTokenId = _context.GenerateTokens.Where(e => e.ApplicationUsers!.Id == user.Id)
                      .Select(e => e.GenerateTokenId).FirstOrDefault();
                    if (generateTokenId == null)
                    {
                        return Ok(new { success = false, message = "generateTokenId Null" });
                    }
                    var _generateTokens = await _context.GenerateTokens.FindAsync(generateTokenId);

                    subject = "Confirm your email again";
                    messageBody = "Hello " + user.UserName + "<br />";
                    messageBody += "Please get code below to activate your account" + "<br />" + "<br />";
                    tokenfromEmail = SendEmail.sendEmail(model.Email!, subject, messageBody)!;
                    if (tokenfromEmail != null)
                    {
                        if (_generateTokens != null)
                        {
                            _generateTokens.TokenEmailConfirmation = token;
                            _generateTokens.TokenFromEmail = tokenfromEmail;

                            _context.GenerateTokens.Attach(_generateTokens);
                            _context.Entry(_generateTokens).Property(x => x.TokenEmailConfirmation).IsModified = true;
                            _context.Entry(_generateTokens).Property(x => x.TokenFromEmail).IsModified = true;
                            await _context.SaveChangesAsync();
                        }
                        return Ok(new { success = true, email = model.Email, message = "Send Confirmation Email Successfully" });
                    }
                    else
                    {
                        return Ok(new { success = false, message = "Send failed !! Check the connection" });
                    }

                }
                return Ok(new { success = false, message = "Your Email Confirmed" });
            }
            return Ok(new { success = false, message = "ModelState IsValid" });
        }


        [HttpPost]
        [Route("ConfirmEmail")]
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(ConfirmEmailViewModel ConfirmEmailViewModel)
        {

            if (ConfirmEmailViewModel.EmailOrUserName == null || ConfirmEmailViewModel.TokenFromEmail == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "ConfirmEmail UnSuccessfully. UserId or token error not found  "
                });
            }
            var user = await userManager.Users.FirstOrDefaultAsync(u => u.UserName == ConfirmEmailViewModel.EmailOrUserName || u.Email == ConfirmEmailViewModel.EmailOrUserName);
            //var user = await userManager.FindByIdAsync(userId);


            if (user == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "ConfirmEmail unSuccessfully. User not found  "
                });
            }
            var generateTokenId = _context.GenerateTokens.Where(e => e.ApplicationUsers!.Email == user.Email || e.ApplicationUsers.UserName==user.UserName)
                       .Select(e => e.GenerateTokenId).FirstOrDefault();
            if (generateTokenId == null)
            {
                return Ok(new { success = false, message = "generateTokenId Null" });
            }
            var tokenEC = await _context.GenerateTokens.FindAsync(generateTokenId);

            if (tokenEC?.TokenFromEmail != ConfirmEmailViewModel.TokenFromEmail)
            {
                return Ok(new { success = false, message = "Code Error" });
            }

            var result = await userManager.ConfirmEmailAsync(user, tokenEC.TokenEmailConfirmation!);

            if (result.Succeeded)
            {
                //return View();
                return Ok(new
                {
                    success = true,
                    message = "ConfirmEmail Successfully"
                });
            }
            return BadRequest();
        }



        [HttpPost]
        [Route("ForgotPasswordbyEmail")]
        [AllowAnonymous]
        public async Task<ActionResult> ForgotPasswordbyEmail(SendTokenEmail model)
        {
            string tokenfromEmail, subject, messageBody;
            if (ModelState.IsValid)
            {
                // Find the user by email
                var user = await userManager.FindByEmailAsync(model.Email!);

                if (user == null)
                {
                    return Ok(new { success = false, message = "Email not found" });
                }
                // If the user is found AND Email is confirmed
                if (user != null && await userManager.IsEmailConfirmedAsync(user))
                {
                    // Generate the reset password token
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    var generateTokenId = _context.GenerateTokens.Where(e => e.ApplicationUsers!.Id == user.Id)
                      .Select(e => e.GenerateTokenId).FirstOrDefault();
                    if (generateTokenId == null)
                    {
                        return Ok(new { success = false, message = "generateTokenId Null" });
                    }
                    var _generateTokens = await _context.GenerateTokens.FindAsync(generateTokenId);

                    subject = "Reset your Password";
                    messageBody = "Hello " + user.UserName + "<br />";
                    messageBody += "Please get code below to Reset your Password" + "<br />" + "<br />";
                    tokenfromEmail = SendEmail.sendEmail(model.Email!, subject, messageBody)!;
                    // Build the password reset link
                    if (tokenfromEmail != null)
                    {
                        if (_generateTokens != null)
                        {
                            _generateTokens.TokenPasswordReset = token;
                            _generateTokens.TokenFromEmail = tokenfromEmail;

                            _context.GenerateTokens.Attach(_generateTokens);
                            _context.Entry(_generateTokens).Property(x => x.TokenPasswordReset).IsModified = true;
                            _context.Entry(_generateTokens).Property(x => x.TokenFromEmail).IsModified = true;
                            await _context.SaveChangesAsync();
                        }
                        return Ok(new { success = true, email = model.Email, message = "Password Reset Successfully" });
                    }
                    else
                    {
                        return Ok(new { success = false, message = "Send failed !! Check the connection" });
                    }

                }
                return Ok(new { success = false, message = "Email not Confirmed" });
            }
            return Ok(new { success = false, message = "ModelState IsValid" });
        }


        [HttpPost]
        [Route("PasswordConfirmEmail")]
        [AllowAnonymous]
        public async Task<ActionResult> PasswordConfirmEmail(ConfirmEmailViewModel ConfirmEmailViewModel)
        {

            if (ConfirmEmailViewModel.EmailOrUserName == null || ConfirmEmailViewModel.TokenFromEmail == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "Email Token not found  "
                });
            }
            var user = await userManager.Users.FirstOrDefaultAsync(u => u.UserName == ConfirmEmailViewModel.EmailOrUserName || u.Email == ConfirmEmailViewModel.EmailOrUserName);
            //var user = await userManager.FindByIdAsync(userId);


            if (user == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "Email not found  "
                });
            }
            var generateTokenId = _context.GenerateTokens.Where(e => e.ApplicationUsers!.Email == user.Email || e.ApplicationUsers.UserName == user.UserName)
                       .Select(e => e.GenerateTokenId).FirstOrDefault();
            if (generateTokenId == null)
            {
                return Ok(new { success = false, message = "generateTokenId Null" });
            }
            var tokenEC = await _context.GenerateTokens.FindAsync(generateTokenId);

            if (tokenEC?.TokenFromEmail != ConfirmEmailViewModel.TokenFromEmail)
            {
                return Ok(new { success = false, message = "Code Error" });
            }
            else if (tokenEC?.TokenFromEmail == ConfirmEmailViewModel.TokenFromEmail)
            {
                //return View();
                return Ok(new
                {
                    success = true,
                    email = ConfirmEmailViewModel.EmailOrUserName,
                    message = "ConfirmEmail Successfully"
                });
            }
            return BadRequest();
        }


        [HttpPost]
        [Route("ResetPasswordbyEmail")]
        [AllowAnonymous]
        public async Task<ActionResult> ResetPasswordbyEmail(ResetPasswordbyEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Find the user by email
                var user = await userManager.FindByEmailAsync(model.Email!);

                if (user != null)
                {
                    var generateTokenId = _context.GenerateTokens.Where(e => e.ApplicationUsers!.Id == user.Id)
                      .Select(e => e.GenerateTokenId).FirstOrDefault();
                    if (generateTokenId == null)
                    {
                        return Ok(new { success = false, message = "generateTokenId Null" });
                    }
                    var tokenEC = await _context.GenerateTokens.FindAsync(generateTokenId);
                    //if (tokenEC?.TokenFromEmail != model.Token)
                    //{
                    //    return Ok(new { success = false, message = "Code Error" });
                    //}
                    // reset the user password
                    var result = await userManager.ResetPasswordAsync(user, tokenEC!.TokenPasswordReset!, model.NewPassword!);
                    if (result.Succeeded)
                    {
                        if (await userManager.IsLockedOutAsync(user))
                        {
                            await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
                        }
                        return Ok(new { success = true, message = "Reset Password Successfully" });
                    }
                }
                return Ok(new { success = false, message = "Email Not Found" });
            }
            // Display validation errors if model state is not valid
            return Ok(model);
        }













        [HttpGet]
        [Route("facebook-login")]
        [AllowAnonymous]
        public IActionResult FacebookLogin(string returnURL)
        {
            return Challenge(
                new AuthenticationProperties()
                {
                    RedirectUri = Url.Action(nameof(FacebookLoginCallback), new { returnURL })
                },
                FacebookDefaults.AuthenticationScheme
            );
        }

        [HttpGet]
        [Route("facebook-login-callback")]
        [AllowAnonymous]
        public async Task<IActionResult> FacebookLoginCallback(string returnURL)
        {
            var authenticationResult = await HttpContext
            .AuthenticateAsync(FacebookDefaults.AuthenticationScheme);
            if (authenticationResult.Succeeded)
            {
                string Id = authenticationResult.Principal.FindFirst(ClaimTypes.NameIdentifier)!.Value;
                string Email = authenticationResult.Principal.FindFirst(ClaimTypes.Email)!.Value;
                string FirstName = authenticationResult.Principal.FindFirst(ClaimTypes.GivenName)!.Value;
                string LastName = authenticationResult.Principal.FindFirst(ClaimTypes.Surname)!.Value;
                string FullName = authenticationResult.Principal.FindFirst(ClaimTypes.Name)!.Value;

                //===============Encode==================================
                var UTF8 = System.Text.Encoding.UTF8.GetBytes(Email!);
                var ToBase64String_Email = Convert.ToBase64String(UTF8);
                //=================================================

                return Redirect($"{returnURL}externallogin?email={ToBase64String_Email}");
            }
            return Redirect($"{returnURL}?externalauth=false");
        }


        [HttpGet]
        [Route("google-login")]
        [AllowAnonymous]
        public IActionResult GoogleLogin(string returnURL)
        {
            return Challenge(
                new AuthenticationProperties
                {
                    RedirectUri = Url.Action(nameof(GoogleLoginCallBack), new { returnURL })
                },
                GoogleDefaults.AuthenticationScheme
            );
        }

        [HttpGet]
        [Route("google-login-callback")]
        [AllowAnonymous]
        public async Task<IActionResult> GoogleLoginCallBack(string returnURL)
        {
            var authenticationResult = await HttpContext
            .AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            if (authenticationResult.Succeeded)
            {
                string Id = authenticationResult.Principal.FindFirst(ClaimTypes.NameIdentifier)!.Value;
                string Email = authenticationResult.Principal.FindFirst(ClaimTypes.Email)!.Value;
                string FirstName = authenticationResult.Principal.FindFirst(ClaimTypes.GivenName)!.Value;
                string LastName = authenticationResult.Principal.FindFirst(ClaimTypes.Surname)!.Value;
                string FullName = authenticationResult.Principal.FindFirst(ClaimTypes.Name)!.Value;

                //===============Encode==================================
                var UTF8 = System.Text.Encoding.UTF8.GetBytes(Email!);
                var ToBase64String_Email = Convert.ToBase64String(UTF8);
                //=================================================
                return Redirect($"{returnURL}externallogin?email={ToBase64String_Email}");
            }
            return Redirect($"{returnURL}?externalauth=false");
        }


        
        
        
        
        
        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequestDto request)
        {
            var result = await RefreshTokensAsync(request);
            if (result is null || result.AccessToken is null || result.RefreshToken is null)
                return Unauthorized("Invalid refresh token.");
        
            return Ok(result);
        }
        
        
        // =================RefreshTokensAsync===========================
        public async Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request)
        {
            var user = await ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);
            if (user is null)
                return null;
        
            return await CreateTokenResponse(user);
        }

        private async Task<ApplicationUser?> ValidateRefreshTokenAsync(string userId, string refreshToken)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user is null || user.RefreshToken != refreshToken
                             || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return null;
            }

            return user;
        }
        
        private async Task<TokenResponseDto> CreateTokenResponse(ApplicationUser? user)
        {
            
            var roleId = _context.UserRoles.Where(e => e.UserId == user!.Id)
                .Select(e => e.RoleId).FirstOrDefault();

            var roleName = await roleManager.FindByIdAsync(roleId!);
            return new TokenResponseDto
            {
                AccessToken = GenerateAccessToken(user.Email,roleName.Name),
                RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
            };
        }
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        private async Task<string> GenerateAndSaveRefreshTokenAsync(ApplicationUser user)
        {
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _context.SaveChangesAsync();
            return refreshToken;
        }
        
        
        
        



        private string GenerateAccessToken(string email,string rolename)
        {
            var claims = new[]
                {
                new Claim(ClaimTypes.Name, email),
                new Claim(ClaimTypes.Role, rolename)
                };

            var key = new SymmetricSecurityKey(Encoding.UTF32.GetBytes(_configuration["JwtSecurityKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.UtcNow.AddYears(Convert.ToInt32(_configuration["JwtExpiry"]));

            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtAudience"],
                claims,
                expires: expiry,
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }





    }
}

