using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using TriviaService;
using System.Linq;
using TriviaService.Models;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace TriviaServiceUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestNewPlayer()
        {            
            var triviaController = new TriviaService.Controllers.TriviaController();            
            var playerModel = await triviaController.NewPlayer("TestPlayer") as OkNegotiatedContentResult<GamePlayerModel>;                
            Assert.IsNotNull(playerModel);            
        }       

        [TestMethod]
        public void TestAnswer()
        {
            var triviaController = new TriviaService.Controllers.TriviaController();
            var result = triviaController.ValidateAnswer(new TriviaService.Models.AnswerModel
            {
                PlayerId = new Guid("79788BC0-AA71-46D4-A01A-28355416E484"),
                Answer = "C",
                QuestionId = new Guid("D4FE8314-DD1B-460A-9775-80C59BEF7382")
            }).Result;

            Assert.IsTrue(result);
        }
    }
}
