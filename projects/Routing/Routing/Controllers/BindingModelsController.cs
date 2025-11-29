using Microsoft.AspNetCore.Mvc;

namespace Routing.Controllers;

[ApiController]
public sealed class BindingModelsController : ControllerBase
{
    [HttpGet("test/{id}/test")]
    public string Test(
        [FromRoute] int id,  // [FromRoute] и [FromQuery] лучше не указывать 
        [FromQuery] int val, // и полагаться на соглашение о привязке данных
        [FromHeader(Name = "MyHeader")] int headerValue)
    {
        return $"Id = {id}, Val = {val}, HeaderValue = {headerValue}";
    }

    // В качестве значений извлекаемых из роута можно указывать простой тип (те, кто реализует TryParse)
    [HttpGet("test/{id}/{name}/{value}")]
    public string Test(int id, string name, double value)
    {
        return $"Id = {id}, Name = {name}, Value = {value}";
    }

    // TryParse можно самостоятельно реализовать
    [HttpGet("test/{myId}")]
    public string Test(MyId myId)
    {
        // test/My123 -> MyId { id = 123 }
        return $"Id = {myId}";
    }

    [HttpGet("test/array")]
    public string Test(
        [FromQuery(Name = "id")] int[] ids)
    {
        // test/array?id=1&id=2&id=3 -> 3
        return $"Length = {ids.Length}";
    }

    [HttpPost("test/with-body")]
    public string Test(
        [FromBody] int body,
        [FromServices] LinkGenerator linkGenerator)
    {
        return $"Body = {body}, RequestBody = {linkGenerator.GetPathByName("Test", new { id = 123 })}";
    }
}

public record MyId(int Id)
{
    public static bool TryParse(string? value, out MyId? result)
    {
        if (value is not null
            && value.StartsWith("My")
            && int.TryParse(
                value.AsSpan().Slice(2),
                out int id))
        {
            result = new MyId(id);
            return true;
        }

        result = default;
        return false;
    }
}