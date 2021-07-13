using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TriviaService.Models;

namespace TriviaService
{
    public class GameHub : Hub
    {
        public int JoinGameRoom(GamePlayerModel gamePlayer)
        {
            List<string> players = null;
            var totalNeeded = 5;

            using(var ctx = new TriviaCtx())
            {
                players = ctx.GamePlayers.Where(gp => gp.GameRoomId == gamePlayer.GameRoomId).OrderBy(gp => gp.PlayerName).Select(gp => gp.PlayerName).ToList();
                totalNeeded = ctx.GameRooms.FirstOrDefault(gr => gr.Id == gamePlayer.GameRoomId).PlayerCount;
            }

            Groups.Add(Context.ConnectionId, gamePlayer.GameRoomId.ToString());
            Clients.Group(gamePlayer.GameRoomId.ToString()).newPlayer(players, totalNeeded - players.Count);

            if (players.Count >= totalNeeded)
                StartGame(gamePlayer.GameRoomId);

            return totalNeeded - players.Count;
        }

        private void StartGame(Guid gameRoomId)
        {
            var questionList = new List<GameQuestionModel>();
            using (var ctx = new TriviaCtx())
            {
                var questions = ctx.GameQuestions.Where(gq => gq.GameRoomId == gameRoomId).OrderBy(gq => gq.RoundNumber);
                foreach(var question in questions)
                {
                    questionList.Add(new GameQuestionModel
                    {
                        Id = question.Id,
                        question = question.Question.Question1,
                        A = question.Question.OptionA,
                        B = question.Question.OptionB,
                        C = question.Question.OptionC,
                        D = question.Question.OptionD
                    });
                }

                Clients.Group(gameRoomId.ToString()).startGame(questionList);
            }
        }
    }
}