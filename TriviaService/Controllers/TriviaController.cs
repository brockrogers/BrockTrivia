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

            using(var ctx = new TrivaCtx())
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
    }
}
