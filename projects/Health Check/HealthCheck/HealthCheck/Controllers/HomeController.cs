using Microsoft.AspNetCore.Mvc;

namespace HealthCheck.Controllers;

public class HomeController : Controller
{
    public ActionResult Index()
    {
        return new ContentResult
        {
            Content = @"
<html>
    <head>
        <title>Health Check</title>
        <meta charset=""utf-8"" />
    </head>
    <body>
        <h1>Health Check (Служба проверки работоспособности)</h1>
        Запуск всех чеков со стандартным формата ответа: <a href=""/AllChecks"">/AllChecks</a><br/>
        Запуск всех чеков с мои форматом ответа: <a href=""/AllChecksWithMyResponseFormat"">/AllChecksWithMyResponseFormat</a><br/>
        Запуск только чека с рандомным ответом: <a href=""/OnlyRandomCheck"">/OnlyRandomCheck</a>
    </body>
</html>",
            ContentType = "text/html"
        };
    }
}