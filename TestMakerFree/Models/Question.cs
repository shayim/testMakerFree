using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMakerFreeWebApp.Models
{
    public class Question
    {
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public int Flags { get; set; }

        public int Id { get; set; }

        public DateTime LastModifiedDate { get; set; } = DateTime.Now;

        public string Notes { get; set; }

        public int QuizId { get; set; }

        public string Text { get; set; }

        public int Type { get; set; }
    }
}