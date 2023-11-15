using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CodeChallenge.Web.Models;
using CodeChallenge.Core;
using CodeChallenge.Web.Helpers;
using CodeChallenge.Questions;

namespace CodeChallenge.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private QuizManager QuizManager { get; set; }

    public HomeController(ILogger<HomeController> logger, IQuizSessionManager quizSessionManager)
    {
        _logger = logger;
        QuizManager = QuizManager.Create(quizSessionManager);
    }

    [HttpGet]
    public IActionResult Index()
    {
        BaseQuestion currentQuestion = null;

        if (!QuizManager.QuizInProgress) // Only generate a new quiz if noting exists.
        {
            QuizManager.GenerateNewQuiz();
            currentQuestion = QuizManager.GetFirstQuestion();
        }
        else // Quiz in progress, load current question. This handles Page refresh
        {
            currentQuestion = QuizManager.GetQuestionById(QuizManager.CurrentQuizId);
        }

        return View(new TaskViewModel { Id = currentQuestion.Id, CodeTemplate = currentQuestion.GetTaskTemplate() });
    }

    [HttpPost]
    public IActionResult Validate(int id, string answer)
    {

        var result = QuizManager.ValidateAnswer(answer);
        if (!result.Status) // Failed
        {
            return Json(new
            {
                status = result.Status,
                statusMessage = result.StatusMessage
            });
        }

        if (QuizManager.IsLastQuestion)
        {
            QuizManager.FinalizeQuiz();
            return Json(new
            {
                status = true,
                completed = true,
                statusMessage = "Congrats, you completed the game.",
                task = new
                {
                    codeTemplate = ""
                }
            });
        }

        var nextQuestion = QuizManager.GetNextQuestion();
        return Json(new
        {
            status = result.Status,
            statusMessage = result.StatusMessage,
            task = new
            {
                id = nextQuestion.Id,
                codeTemplate = nextQuestion.GetTaskTemplate()
            }
        });
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}


public class TaskViewModel
{
    public int Id { get; set; }

    public string CodeTemplate { get; set; }
}

