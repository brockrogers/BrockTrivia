﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TriviaService.Models
{
    public class QuestionsModel
    {
        public string question { get; set; }
        public string A { get; set; }
        public string B { get; set; }
        public string C { get; set; }
        public string D { get; set; }
        public string answer { get; set; }
    }
}