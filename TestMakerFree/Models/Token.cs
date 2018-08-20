using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace TestMakerFreeWebApp.Models
{
    public class Token
    {
        [ForeignKey(nameof(UserId))]
        public virtual AppUser AppUser { get; set; }

        [Required]
        public string ClientId { get; set; }

        public DateTime CreatedDate { get; set; }
        public int Id { get; set; }
        public int Type { get; set; }
        public string UserId { get; set; }

        [Required]
        public string Value { get; set; }
    }
}