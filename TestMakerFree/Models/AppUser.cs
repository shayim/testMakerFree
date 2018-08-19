using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TestMakerFreeWebApp.Models
{
    public class AppUser : IdentityUser<string>
    {
        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string DisplayName { get; set; }

        [Required]
        public int Flags { get; set; }

        [Required]
        public DateTime LastModifiedDate { get; set; } = DateTime.Now;

        public string Notes { get; set; }

        [Required] public int Type { get; set; } = 0;
    }
}