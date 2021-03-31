using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using BowlingLeague.Models;
using BowlingLeague.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BowlingLeague.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public BowlingLeagueContext context { get; set; }

        public HomeController(ILogger<HomeController> logger, BowlingLeagueContext ctx)
        {
            _logger = logger;
            context = ctx;
        }

        public IActionResult Index(long? teamID, string teamName, int pageNum = 0)
        {
            int pageSize = 5;

            return View(new IndexViewModel
            {
                Bowlers = context.Bowlers
                .Where(m => m.TeamId == teamID || teamID == null)
                .OrderBy(m => m.BowlerLastName)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToList(),

                PageNumberingInfo = new PageNumberingInfo
                {
                    NumItemsPerPage = pageSize,
                    CurrentPage = pageNum,

                    // IF no meal has been seleted, then get the full count.
                    // Otherwise only count the number from the meal type that has been selected
                    TotalNumItems = (teamID == null ? context.Bowlers.Count() :
                        context.Bowlers.Where(x => x.TeamId == teamID).Count())
                },

                TeamName = teamName
            });

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
