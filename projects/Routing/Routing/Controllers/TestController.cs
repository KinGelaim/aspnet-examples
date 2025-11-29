using Microsoft.AspNetCore.Mvc;

namespace Routing.Controllers;

[ApiController]
public sealed class TestController : ControllerBase
{
    [HttpGet("/test1/{id}", Name = "Test")]
    public string ThrowExceptionWhenIdNotInt(int id)
    {
        return "An exception will be thrown when the id is not a number";
    }

    [HttpGet("/test2/{id:int}")]
    public string NotFoundCurrentPointWhenIdNotInt(int id)
    {
        return "The current method will not be called if the id is not a number";
    }

    [HttpGet("test3/{name:alpha:length(3,5)=all}/{id:int:max(10)?}")]
    public string DefaultAndOptionalValue(string name, int? id)
    {
        return $"Name = {name}, Id = {id}";
    }

    [HttpGet("test4/{**others}")]
    public string Others(string others)
    {
        // http://localhost:5090/test4/qw/we/er -> qw/we/er
        return $"Value = {others}";
    }

    [HttpGet("test5")]
    public string? RedirectToRoute(LinkGenerator linkGenerator)
    {
        var route = linkGenerator.GetPathByName("Test", new { id = 123 });
        return route;
    }
}