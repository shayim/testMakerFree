using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TestMakerFreeWebApp.ViewModels
{
    public class TokenRequestViewModel
    {
        [Required]
        public string Password { get; set; }

        [Required]
        public string Username { get; set; }
    }
}