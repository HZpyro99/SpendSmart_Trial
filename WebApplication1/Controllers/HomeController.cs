using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SpendSmart.Models;
using Calculator.Models;
using WebApplication1.Models;


namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly SpendSmartDbContext _context;

        public HomeController(ILogger<HomeController> logger, SpendSmartDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Expenses()
        {
            var allExpenses = _context.Expenses.ToList();

            var totalExpenses = allExpenses.Sum(expense => expense.Value);

            ViewBag.Expenses = totalExpenses;

            return View(allExpenses);
        }

        public IActionResult CreateEditExpense(int? id)
        {
            if(id !=null)
            {
                var expenseInDb = _context.Expenses.SingleOrDefault(expense => expense.Id == id);
                return View(expenseInDb);
            }
            return View();
        }
        
        public IActionResult DeleteExpense(int id)
        {
            var expenseInDb = _context.Expenses.SingleOrDefault(expense => expense.Id == id);
            _context.Expenses.Remove(expenseInDb);
            _context.SaveChanges();
            return RedirectToAction("Expenses");
        }
        public IActionResult Forms(Expense model)
        {
            if(model.Id == 0)
            {
                _context.Expenses.Add(model);
            } else
            {
                _context.Expenses.Update(model);
            }

            _context.SaveChanges();

            return RedirectToAction("Expenses");
        }

        public IActionResult Calculator()
        {
            return View();

        }

        public IActionResult Calcadd(Calc model)
        {
            model.result = model.numerator + model.denominator;
            ViewBag.result = model.result;
            return View("Calculator", model);
        }

        public IActionResult Calcsub(Calc model)
        {
            model.result = model.numerator - model.denominator;
            ViewBag.result = model.result;
            return View("Calculator", model);
        }

        public IActionResult Calcmul(Calc model)
        {
            model.result = model.numerator * model.denominator;
            ViewBag.result = model.result;
            return View("Calculator", model);
        }
        public IActionResult Calcdiv(Calc model)
        {
            if (model.denominator == 0)
            {
                ViewBag.ErrorMessage = "Denominator cannot be zero.";
                return View("Calculator");
            }
            model.result = model.numerator / model.denominator;
            ViewBag.ErrorMessage = model.result;
            return View("Calculator", model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
