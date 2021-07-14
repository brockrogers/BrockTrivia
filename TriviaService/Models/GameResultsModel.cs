using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TriviaService.Models
{
    public class GameResultsModel
    {
        public short RoundNumber { get; set; }
        public string Question { get; set; }
        public string CorrectAnswer { get; set; }
        public int TotalPlayers { get; set; }
        public int CorrectAnswers { get; set; }
        public int IncorrectAnswers { get; set; }
    }
}