using Microsoft.AspNetCore.Mvc;
using ActualCalculator.Models;
using System.Globalization;

namespace WebApplication1.Controllers
{
    public class CalcuCont : Controller
    {
        [HttpGet]
        public IActionResult Actualcalc()
        {
            return View(new Calcu());
        }

        [HttpPost]
        public IActionResult Actualcalc(Calcu model)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(model.Expression))
                {
                    double result = Evaluate(model.Expression);
                    model.Value = result;

                    model.Expression = result.ToString(CultureInfo.InvariantCulture);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return View(model);
        }

        private List<string> Tokenize(string expr)
        {
            var tokens = new List<string>();
            int i = 0;

            while (i < expr.Length)
            {
                char c = expr[i];

                if (char.IsWhiteSpace(c)) { i++; continue; }

                if (char.IsDigit(c) || c == '.')
                {
                    int start = i;
                    while (i < expr.Length && (char.IsDigit(expr[i]) || expr[i] == '.'))
                        i++;

                    tokens.Add(expr.Substring(start, i - start));
                    continue;
                }

                if ("+-*/()".Contains(c))
                {
                    tokens.Add(c.ToString());
                    i++;
                    continue;
                }

                throw new Exception("Invalid character");
            }

            return tokens;
        }

        private int Precedence(string op)
        {
            if (op == "+" || op == "-") return 1;
            if (op == "*" || op == "/") return 2;
            return 0;
        }

        private List<string> ToPostfix(List<string> tokens)
        {
            var output = new List<string>();
            var ops = new Stack<string>();

            foreach (var token in tokens)
            {
                if (double.TryParse(token, out _))
                {
                    output.Add(token);
                }
                else
                {
                    while (ops.Count > 0 && Precedence(ops.Peek()) >= Precedence(token))
                        output.Add(ops.Pop());

                    ops.Push(token);
                }
            }

            while (ops.Count > 0)
                output.Add(ops.Pop());

            return output;
        }

        private double Evaluate(string expr)
        {
            var tokens = Tokenize(expr);
            var postfix = ToPostfix(tokens);

            ViewBag.DebugTokens = string.Join(" ", tokens);
            ViewBag.DebugPostfix = string.Join(" ", postfix);

            var stack = new Stack<double>();

            foreach (var token in postfix)
            {
                if (double.TryParse(token, NumberStyles.Float, CultureInfo.InvariantCulture, out double num))
                {
                    stack.Push(num);
                }
                else
                {
                    double b = stack.Pop();
                    double a = stack.Pop();

                    double result = token switch
                    {
                        "+" => a + b,
                        "-" => a - b,
                        "*" => a * b,
                        "/" => a / b,
                        _ => throw new Exception("Invalid operator")
                    };

                    stack.Push(result);
                }
            }

            ViewBag.DebugStackCount = stack.Count;

            return stack.Pop();
        }   
    }
}
