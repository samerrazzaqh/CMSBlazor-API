using System;
using Microsoft.AspNetCore.Identity;

namespace CMSBlazor.Shared.Models
{
    public class ApplicationUser : IdentityUser
    {

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool Block { get; set; }
        public DateTime? BlockTime { get; set; }
        
        // ============RefreshToken================================
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}

