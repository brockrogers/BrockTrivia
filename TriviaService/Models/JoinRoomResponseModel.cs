using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TriviaService.Models
{
    public class JoinRoomResponseModel
    {
        public int PlayersNeeded { get; set; }
        public List<string> Players { get; set; }
    }
}