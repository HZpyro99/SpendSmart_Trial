using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ActualCalculator.Models;

namespace WebApplication1.Controllers
{
    public class CalcuCont : Controller
    {

        [HttpGet]
        public IActionResult Actualcalc()
        {

            return View();
        }
    }
}
