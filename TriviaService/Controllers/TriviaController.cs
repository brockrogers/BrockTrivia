using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TriviaService.Models;

namespace TriviaService.Controllers
{
    [RoutePrefix("api/Trivia")]
    public class TriviaController : ApiController
    {
        [HttpPut]
        [Route("LoadQuestions")]
        public async Task<IHttpActionResult> LoadQuestions([FromBody] string password)
        {
            if (password != "brocktrivia")
                return Unauthorized();

            var mappedPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Models/QandA.json");
            var questionsAndAnswersText = System.IO.File.ReadAllText(mappedPath);
            var questionsAndAnswersModel = Newtonsoft.Json.JsonConvert.DeserializeObject<List<QuestionsModel>>(questionsAndAnswersText);

            using(var ctx = new TriviaCtx())
            {
                ctx.Database.ExecuteSqlCommand("TRUNCATE Table Questions");
                ctx.Questions.AddRange(questionsAndAnswersModel.Select(qa => new Question {
                    Id = Guid.NewGuid(),
                    Question1 = qa.question,
                    Answer = qa.answer,
                    OptionA = qa.A,
                    OptionB = qa.B,
                    OptionC = qa.C,
                    OptionD = qa.D
                }).ToList());

                ctx.SaveChanges();
            }           

            return Ok();
        }

        [HttpPost]
        [Route("NewPlayer")]
        public async Task<IHttpActionResult> NewPlayer([FromBody] string playerName)
        {
            Guid playerId = Guid.NewGuid();
            var playerModel = new GamePlayerModel();
            playerModel.PlayerId = playerId;

            if (string.IsNullOrEmpty(playerName))
                return BadRequest();

            using(var ctx = new TriviaCtx())
            {
                int playerCount = 3;
                short questionCount = 5;
                var settings = ctx.GameSettings.FirstOrDefault();
                if(settings != null)
                {
                    playerCount = settings.PlayerCount;
                    questionCount = settings.QuestionCount;
                }

                Guid? gameRoomId = null;
                var gameRoom = ctx.GameRooms.OrderByDescending(gr => gr.DateCreated).FirstOrDefault();
                if (gameRoom == null)
                    gameRoom = await CreateGameRoom(playerCount, questionCount, ctx);
                else
                {                    
                    if (gameRoom.GamePlayers.Count() >= playerCount)
                        gameRoom = await CreateGameRoom(playerCount, questionCount, ctx);                    
                }                

                if (gameRoom == null)
                    return InternalServerError();

                gameRoom.GamePlayers.Add(new GamePlayer
                {
                    Id = Guid.NewGuid(),
                    GameRoomId = gameRoom.Id,
                    PlayerName = playerName,
                    PlayerId = playerId
                });

                playerModel.GameRoomId = gameRoom.Id;

                ctx.SaveChanges();
            }

            return Ok(playerModel);
        }

        private async Task<GameRoom> CreateGameRoom(int playerCount, short questionCount, TriviaCtx ctx)
        {            
            var newGameRoom = ctx.GameRooms.Add(new GameRoom
            {
                Id = Guid.NewGuid(),
                DateCreated = DateTime.UtcNow,
                PlayerCount = playerCount,
                QuestionCount = questionCount
            });            

            var allQuestions = ctx.Questions.ToList();
            var random = new Random();
            for(short item = 0; item < questionCount; item++ )
            {
                var randomNumber = random.Next(allQuestions.Count - 1);
                newGameRoom.GameQuestions.Add(new GameQuestion
                {
                    Id = Guid.NewGuid(),
                    QuestionId = allQuestions[randomNumber].Id,
                    RoundNumber = (short)(item + 1)
                }); ;
            }

            return newGameRoom;
        }

    }
}
