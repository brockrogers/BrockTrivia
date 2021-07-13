using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TriviaService.Models
{
    public class GamePlayerModel
    {
        public Guid GameRoomId { get; set; }
        public Guid PlayerId { get; set; }
        public string PlayerName { get; set; }
    }
}