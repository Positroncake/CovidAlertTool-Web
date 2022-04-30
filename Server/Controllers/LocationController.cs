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
    public async Task<ActionResult> NewLocation(NewLocation location)
    {
        IAccess access = new Access();
        String hash = Utils.HashWords(location.Key);
        foreach (String word in location.Key) Console.WriteLine(word);
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
            Lat = location.Lat,
            Lon = location.Lon,
            T = location.T
        }, Utils.UsersDatabaseConnectionString);

        return Ok(query);
    }

    [HttpPost]
    [Route("Get")]
    public async Task<ActionResult> GetUserLocations(String[] key)
    {
        IAccess access = new Access();
        String hash = Utils.HashWords(key);
        Console.WriteLine(hash);
        var sql = $"SELECT * FROM {hash}";

        List<Location> locations;
        // Checks if account is valid
        try
        {
            locations = await access.Query<Location, dynamic>(sql, new { }, Utils.UsersDatabaseConnectionString);
        }
        catch (MySqlException e)
        {
            return Unauthorized();
        }

        return Ok(locations);
    }
}