using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using ProfilerExample.Models;

namespace ProfilerExample.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        using var connection = new MySqlConnection("server=mysql;uid=dbuser;pwd=my-secret-pw;database=test");
        var command = connection.CreateCommand();
        command.CommandText = "SHOW VARIABLES LIKE '%version%';";
        
        connection.Open();
        using var reader = command.ExecuteReader();
        var version = new StringBuilder();
        while (reader.Read()) 
            version.AppendLine($"{reader.GetString(0)}: {reader.GetString(1)}");

        return View(nameof(Index), version.ToString());
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
