using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CodeChallenge.Web.Models;
using CodeChallenge.Core;

namespace CodeChallenge.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private static TaskManager taskManager = new TaskManager();

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var task = taskManager.GetNewTask();

        return View(new TaskViewModel { Id = task.Id, CodeTemplate = task.GetTaskTemplate() });
    }

    [HttpPost]
    public IActionResult Validate(int id, string answer)
    {
        var result = taskManager.ValidateTask(id, answer);
        if(!result.Status) // Failed
        {
            return Json(new
            {
                status = result.Status,
                statusMessage = result.StatusMessage
            });
        }

        if (!taskManager.HasMoreTasks)
        {
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

        var task = taskManager.GetNewTask();

        return Json(new
        {
            status = result.Status,
            statusMessage = result.StatusMessage,
            task = new {
                id = task.Id,
                codeTemplate = task.GetTaskTemplate()
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

