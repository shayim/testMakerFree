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
    public class QuizViewModel
    {
        #region constructor

        public QuizViewModel()
        {
        }

        #endregion constructor

        #region properties

        public DateTime CreatedDate { get; } = DateTime.Now;

        public string Description { get; set; }

        public int Flags { get; set; }

        public int Id { get; set; }

        public DateTime LastModifiedDate { get; } = DateTime.Now;

        public string Notes { get; set; }

        public string Text { get; set; }
        [Required, StringLength(200, MinimumLength = 3)]
        public string Title { get; set; }

        public int Type { get; set; }

        [Required]
        public string UserId { get; set; }

        [JsonIgnore]
        public int ViewCount { get; set; }

        #endregion properties
    }
}