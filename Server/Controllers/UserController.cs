using System.Security.Cryptography;
using CovidAlertTool.Shared;
using DatabaseAccess;
using Microsoft.AspNetCore.Mvc;

namespace CovidAlertTool.Server.Controllers;

[ApiController]
[Route("UserApi")]
public class UserController : ControllerBase
{
    [HttpPost]
    [Route("New")]
    public async Task<ActionResult> NewUser()
    {
        // Generate 12-word password
        IAccess access = new Access();
        var words = new String[12];
        const String sql = "SELECT * FROM words WHERE Id = @Id LIMIT 1";
        for (var i = 0; i < 12; ++i)
        {
            List<WordClass> word = await access.Query<WordClass, dynamic>(sql, new
            {
                Id = RandomNumberGenerator.GetInt32(0, 10001)
            }, Utils.UsersDatabaseConnectionString);
            words[i] = word[0].Word;
        }

        // Hash password and create new user
        String hash = Utils.HashWords(words);
        var query = @$"CREATE TABLE {hash} (
    Lat VARCHAR(8),
    Lon VARCHAR(8),
    T DATETIME
)";
        await access.Execute(query, new
        {
            TableName = hash
        }, Utils.UsersDatabaseConnectionString);

        // Return words to user
        return Ok(words);
    }

    [HttpPost]
    [Route("Delete")]
    public async Task<ActionResult> Delete(String[] words)
    {
        String hash = Utils.HashWords(words);
        IAccess access = new Access();
        var sql = $"DROP TABLE {hash}";
        await access.Execute(sql, new { }, Utils.UsersDatabaseConnectionString);
        return Ok();
    }
}