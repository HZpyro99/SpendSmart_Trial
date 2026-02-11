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

        [HttpPost]
        public IActionResult Actualcalc(Calcu model)
        {
            if (model.but == "add")
            {
                model.result = model.numerator + model.denominator;
            }
            else if (model.but == "sub")
            {
                model.result = model.numerator - model.denominator;
            }
            else if (model.but == "X")
            {
                model.result = model.numerator * model.denominator;
            }
            else if (model.but == "div")
            {
                if (model.denominator != 0)
                {
                    model.result = model.numerator / model.denominator;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Cannot divide by zero.");
                }
            }
            else if(model.but == "clr")
            {
                model.result = 0;
                model.numerator = 0;
                model.denominator = 0;
            }
            return View(model);
        }

    }
}
