using System.Diagnostics;
using ContainVirus.Models;
using ContainVirus.Solution;
using Microsoft.AspNetCore.Mvc;

namespace ContainVirus.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SolutionWithLogs _solutionWithLogs;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _solutionWithLogs = new SolutionWithLogs();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Solve([FromBody] int[][] grid)
        {
            try
            {
                if (grid == null || grid.Length == 0 || grid[0].Length == 0)
                {
                    return BadRequest(new { error = "Invalid grid" });
                }

                int[][] gridCopy = new int[grid.Length][];
                for (int i = 0; i < grid.Length; i++)
                {
                    gridCopy[i] = new int[grid[i].Length];
                    Array.Copy(grid[i], gridCopy[i], grid[i].Length);
                }

                var (totalWalls, totalDays, logs) = _solutionWithLogs.ContainVirusWithLogs(gridCopy);
                
                var logsDto = logs.Select(log => new
                {
                    dayNumber = log.DayNumber,
                    chosenRegionWalls = log.ChosenRegionWalls,
                    chosenRegionCells = log.ChosenRegionCells.Select(c => new { r = c.r, c = c.c }).ToArray(),
                    spreadCells = log.SpreadCells.Select(c => new { r = c.r, c = c.c }).ToArray(),
                    gridSnapshot = log.GridSnapshot
                }).ToArray();

                return Ok(new 
                { 
                    result = totalWalls,
                    totalDays = totalDays,
                    logs = logsDto
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error solving contain virus problem");
                return StatusCode(500, new { error = "An error occurred while solving the problem" });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
