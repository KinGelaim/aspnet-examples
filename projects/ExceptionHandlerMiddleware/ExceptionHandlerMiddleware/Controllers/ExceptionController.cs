using Microsoft.AspNetCore.Mvc;

namespace ExceptionHandlerMiddleware.Controllers;

[ApiController]
[Route("[controller]")]
public class ExceptionController : ControllerBase
{
    [HttpGet("success")]
    public string GetSuccess()
    {
        return "Everything is fine";
    }

    [HttpGet("time-out-exception")]
    public string GetTimeOutException()
    {
        throw new TimeoutException("Out of time");
    }

    [HttpGet("other-exception")]
    public string GetOtherException()
    {
        throw new Exception("Something went wrong");
    }
}