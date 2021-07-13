using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TriviaService.Models
{
    public class AnswerModel
    {        
        public Guid PlayerId { get; set; }
        public Guid QuestionId { get; set; }
        public string Answer { get; set; }
    }
}