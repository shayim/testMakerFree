using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace TestMakerFreeWebApp.ViewModels
{
  public class ResultViewModel
  {
    [JsonIgnore]
    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public int Flags { get; set; }

    public int Id { get; set; }
    public DateTime LastModifiedDate { get; set; } = DateTime.Now;
    public string Notes { get; set; }
    public int QuizId { get; set; }

    [Required, StringLength(250, MinimumLength = 3)]
    public string Text { get; set; }

    public int Type { get; set; }

    public int? MaxValue { get; set; }
    public int? MinValue { get; set; }
  }
}