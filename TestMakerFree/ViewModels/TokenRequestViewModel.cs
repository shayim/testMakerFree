using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TestMakerFreeWebApp.ViewModels
{
    public class TokenRequestViewModel
    {
        [JsonProperty("grant_type")]
        public string GrantType { get; set; }

        public string Password { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        public string Username { get; set; }
    }
}