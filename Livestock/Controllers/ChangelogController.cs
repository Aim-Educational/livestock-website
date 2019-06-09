using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Website.Controllers
{
    public class ChangelogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ShowLog(string version, bool technical = false)
        {
            return View(version, technical);
        }
    }
}