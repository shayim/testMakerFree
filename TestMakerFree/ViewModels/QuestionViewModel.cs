using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TestMakerFreeWebApp.ViewModels
{
    [JsonObject(MemberSerialization.OptOut)]
    public class QuestionViewModel
    {
        [JsonIgnore]
        public DateTime CreatedDate { get; set; }

        public int Flags { get; set; }

        public int Id { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string Notes { get; set; }

        public int QuizId { get; set; }

        [Required, StringLength(250, MinimumLength = 3)]
        public string Text { get; set; }

        public int Type { get; set; }
    }
}