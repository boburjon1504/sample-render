using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace Render_V2.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{

    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    private readonly IMongoCollection<User> collection;
    private readonly IConfiguration _configuration;
    public WeatherForecastController(ILogger<WeatherForecastController> logger,IConfiguration configuration)
    {
        var variable = configuration.GetValue<string>("mongodb+srv://admin:admin@cluster0.ix4en.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0");
        var client = new MongoClient("mongodb+srv://boburjon:<db_password>@cluster0.ix4en.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0");
        var db = client.GetDatabase("RenderV2");
        collection = db.GetCollection<User>("users");

        _logger = logger;
    }
    [HttpGet("users")]
    public IActionResult GetUsers()
    {
        var users = collection.Find(_ => true).ToList();
        return Ok(users);
    }

    [HttpPost("users/add-user")]
    public async Task<IActionResult> Result([FromBody] User user)
    {
        user.Id = Guid.NewGuid();
        await collection.InsertOneAsync(user);

        return Ok(user);
    }



    [HttpGet("say-hi")]
    public IActionResult SayHi() => Ok("Say - hi endpoint hit!");

    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}

public class User
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.Binary)]
    [BsonGuidRepresentation(MongoDB.Bson.GuidRepresentation.Standard)]
    public Guid Id { get; set; }

    public string FirstName { get; set; }
}
