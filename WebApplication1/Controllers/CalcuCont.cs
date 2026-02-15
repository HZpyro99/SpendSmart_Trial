using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ActualCalculator.Models;
using System.Security.Cryptography.X509Certificates;

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
            if (ModelState.IsValid)
            {
                try
                {
                    // TEMPORARY VARIABLES (logic only)
                    ParseExpression(model.Expression, out var numbers, out var operators);
                    double value = EvaluatePemdasNoParenNoExp(numbers, operators);

                    model.Result = value.ToString();
                }
                catch
                {
                    model.Result = "Error";
                }
            }

            return View(model);
        }

        // ---------------- PARSER ----------------
        private void ParseExpression(
            string expression,
            out List<double> numbers,
            out List<char> operators)
        {
            numbers = new List<double>();
            operators = new List<char>();

            string currentNumber = "";

            foreach (char c in expression)
            {
                if (char.IsDigit(c) || c == '.')
                {
                    currentNumber += c;
                }
                else if (c == '+' || c == '-' || c == '*' || c == '/')
                {
                    numbers.Add(double.Parse(currentNumber));
                    operators.Add(c);
                    currentNumber = "";
                }
            }

            if (!string.IsNullOrEmpty(currentNumber))
                numbers.Add(double.Parse(currentNumber));
        }


        // ---------------- PEMDAS (MD → AS) ----------------
        private double EvaluatePemdasNoParenNoExp(
            List<double> numbers,
            List<char> operators)
        {
            // PASS 1: MULTIPLY & DIVIDE
            for (int i = 0; i < operators.Count; i++)
            {
                if (operators[i] == '*' || operators[i] == '/')
                {
                    double left = numbers[i];
                    double right = numbers[i + 1];

                    double result;

                    if (operators[i] == '*')
                    {
                        result = left * right;
                    }
                    else
                    {
                        if (right == 0)
                            throw new DivideByZeroException();

                        result = left / right;
                    }

                    numbers[i] = result;
                    numbers.RemoveAt(i + 1);
                    operators.RemoveAt(i);
                    i--;
                }
            }

            // PASS 2: ADD & SUBTRACT
            double finalResult = numbers[0];

            for (int i = 0; i < operators.Count; i++)
            {
                if (operators[i] == '+')
                    finalResult += numbers[i + 1];
                else
                    finalResult -= numbers[i + 1];
            }

            return finalResult;
        }
    }
}


