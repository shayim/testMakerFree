using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace TestMakerFreeWebApp.Models
{
    public class Quiz
    {
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string Description { get; set; }

        public int Flags { get; set; }

        public int Id { get; set; }

        public DateTime LastModifiedDate { get; set; } = DateTime.Now;

        public string Notes { get; set; }

        public virtual List<Question> Questions { get; set; }

        public virtual List<Result> Results { get; set; }

        public string Text { get; set; }

        public string Title { get; set; }

        public int Type { get; set; }

        public virtual AppUser User { get; set; }

        [Required]
        public string UserId { get; set; }

        public int ViewCount { get; set; }
    }
}