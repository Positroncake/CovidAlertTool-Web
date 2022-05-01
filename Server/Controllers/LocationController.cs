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

        var query = $"INSERT INTO {hash} (Latitude, Longitude, Name) VALUES (@Latitude, @Longitude, @Name)";
        await access.Execute(query, new
        {
            location.Latitude,
            location.Longitude,
            location.Name
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

    [HttpPost]
    [Route("ReportCase")]
    public async Task<ActionResult> ReportCase(Location location)
    {
        IAccess access = new Access();
        const String query = "INSERT INTO user_reports (Latitude, Longitude, Name) VALUES (@Latitude, @Longitude, @Name)";
        await access.Execute(query, new
        {
            location.Latitude,
            location.Longitude,
            location.Name
        }, Utils.LocationsDatabaseConnectionString);

        return Ok(query);
    }

    [HttpGet]
    [Route("GetReports")]
    public async Task<ActionResult> GetReports()
    {
        IAccess access = new Access();
        const String query = "SELECT * FROM user_reports";
        List<Location> locations =
            await access.Query<Location, dynamic>(query, new { }, Utils.LocationsDatabaseConnectionString);
        return Ok(locations);
    }
}