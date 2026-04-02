using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMSBlazor.Shared.Models
{
	public class GenerateToken
	{
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? GenerateTokenId { get; set; }
        public string? TokenFromEmail { get; set; }
        public string? TokenEmailConfirmation { get; set; }
        public string? TokenPasswordReset { get; set; }





        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser? ApplicationUsers { get; set; }
    }
}

