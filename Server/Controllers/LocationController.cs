using CovidAlertTool.Shared;
using DatabaseAccess;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace CovidAlertTool.Server.Controllers;

[ApiController]
[Route("LocationApi")]
public class LocationController : ControllerBase
{
    [HttpPost]
    [Route("New")]
    public async Task<ActionResult> NewLocation(String[] key)
    {
        IAccess access = new Access();
        String hash = Utils.HashWords(key);
        foreach (String word in key) Console.WriteLine(word);
        Console.WriteLine(hash);
        var sql = $"SELECT * FROM {hash}";
        
        // Checks if account is valid
        try
        {
            List<Location> locations = await access.Query<Location, dynamic>(sql, new { }, Utils.UsersDatabaseConnectionString);
        }
        catch (MySqlException e)
        {
            return Unauthorized();
        }

        var query = $"INSERT INTO {hash} (Lat, Lon, T) VALUES (@Lat, @Lon, @T)";
        await access.Execute(query, new
        {
            Lat = "82-31-33",
            Lon = "49-02-44",
            T = DateTime.UtcNow
        }, Utils.UsersDatabaseConnectionString);

        return Ok(query);
    }
}