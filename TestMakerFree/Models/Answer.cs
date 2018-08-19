using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMakerFreeWebApp.Models
{
    public class Answer
    {
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public int Flags { get; set; }

        public int Id { get; set; }

        public DateTime LastModifiedDate { get; set; } = DateTime.Now;

        public string Notes { get; set; }

        public int QuestionId { get; set; }

        public int QuizId { get; set; }

        public string Text { get; set; }

        public int Type { get; set; }

        public int Value { get; set; }
    }
}