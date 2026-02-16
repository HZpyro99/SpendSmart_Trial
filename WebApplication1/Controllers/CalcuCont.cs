using Microsoft.AspNetCore.Mvc;
using ActualCalculator.Models;
using System.Globalization;

namespace WebApplication1.Controllers
{
    public class CalcuCont : Controller
    {
        [HttpGet]//runs when user first opens the calculator page
        public IActionResult Actualcalc()
        {
            return View(new Calcu());
        }

        //runs when user press "="
        [HttpPost]
        public IActionResult Actualcalc(Calcu model)
        {
            //try to evaluate the expression, if it fails show error message
            try
            {
                if (!string.IsNullOrWhiteSpace(model.Expression)) //&& model.Expression != "0"
                {
                    double result = Evaluate(model.Expression); //evaluate the expression using custom method
                    model.Value = result; //store the result in the model to display in the view

                    // Update the expression to show the result instead of the original input
                    model.Expression = result.ToString(CultureInfo.InvariantCulture); 
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message; //show error message in the view if evaluation fails
            }

            return View(model); //return the view with the updated model (either with result or error message)
        }

        private List<string> Tokenize(string expr) //convert the input string into a list of tokens (numbers and operators)
        {
            var tokens = new List<string>(); //list to hold the tokens
            int i = 0; //index to iterate through the input string

            while (i < expr.Length) //loop through the input string
            {
                char c = expr[i];//get the current character

                if (char.IsWhiteSpace(c)) { i++; continue; } //skip whitespace

                if (char.IsDigit(c) || c == '.') //if the character is a digit or a decimal point, start building a number token
                {
                    int start = i;
                    while (i < expr.Length && (char.IsDigit(expr[i]) || expr[i] == '.')) //continue building the number token until we reach a non-digit character
                        i++;

                    tokens.Add(expr.Substring(start, i - start)); //add the number token to the list
                    continue;
                }

                if ("+-*/()".Contains(c)) //if the character is an operator or a parenthesis, add it as a token
                {
                    tokens.Add(c.ToString()); //add the operator token to the list
                    i++;
                    continue;
                }

                throw new Exception("Invalid character"); //throw an exception if we encounter an invalid character
            }

            return tokens; //return the list of tokens
        }

        private int Precedence(string op) //define the precedence of operators for the shunting yard algorithm
        {
            if (op == "+" || op == "-") return 1; //addition and subtraction have lower precedence than multiplication and division
            if (op == "*" || op == "/") return 2; //multiplication and division have higher precedence than addition and subtraction
            return 0; 
        }

        private List<string> ToPostfix(List<string> tokens) //convert the list of tokens from infix notation to postfix notation using the shunting yard algorithm
        {
            var output = new List<string>(); //list to hold the output in postfix notation
            var ops = new Stack<string>(); //stack to hold operators during the conversion process

            foreach (var token in tokens)//loop through each token in the input list
            {
                if (double.TryParse(token, out _))//if the token is a number, add it directly to the output list
                {
                    output.Add(token); //add the number token to the output list
                }
                else
                {
                    //if the token is an operator, pop operators from the stack to the output list until we find an operator with lower precedence or the stack is empty
                    while (ops.Count > 0 && Precedence(ops.Peek()) >= Precedence(token)) 
                        output.Add(ops.Pop()); //pop the operator from the stack and add it to the output list

                    ops.Push(token); //push the current operator onto the stack
                }
            }

            while (ops.Count > 0) //after processing all tokens, pop any remaining operators from the stack to the output list
                output.Add(ops.Pop()); //pop the operator from the stack and add it to the output list

            return output; //return the list of tokens in postfix notation
        }

        //evaluate the expression in postfix notation by using a stack to compute the result
        private double Evaluate(string expr) 
        {
            var tokens = Tokenize(expr); //first, tokenize the input expression to get a list of tokens (numbers and operators)
            var postfix = ToPostfix(tokens); //then, convert the list of tokens from infix notation to postfix notation using the shunting yard algorithm

            ViewBag.DebugTokens = string.Join(" ", tokens); //store the list of tokens in ViewBag for debugging purposes (to see how the input expression was tokenized)
            ViewBag.DebugPostfix = string.Join(" ", postfix); //store the list of tokens in postfix notation in ViewBag for debugging purposes (to see how the expression was converted to postfix notation)

            var stack = new Stack<double>(); //stack to hold numbers during the evaluation of the postfix expression

            foreach (var token in postfix) //loop through each token in the postfix expression
            {
                if (double.TryParse(token, NumberStyles.Float, CultureInfo.InvariantCulture, out double num)) //if the token is a number, push it onto the stack
                {
                    stack.Push(num); //push the number onto the stack
                }
                else
                {
                    double b = stack.Pop(); //if the token is an operator, pop the top two numbers from the stack (these are the operands for the operator)
                    double a = stack.Pop(); //pop the next number from the stack (this is the first operand for the operator)

                    double result = token switch //use a switch expression to perform the operation based on the operator token
                    {
                        "+" => a + b, //if the operator is "+", add the two operands
                        "-" => a - b, //if the operator is "-", subtract the second operand from the first operand
                        "*" => a * b, //if the operator is "*", multiply the two operands
                        "/" => a / b, //if the operator is "/", divide the first operand by the second operand
                        _ => throw new Exception("Invalid operator") //throw an exception if we encounter an invalid operator (this should not happen if the input is valid)
                    };

                    stack.Push(result); //push the result of the operation back onto the stack (this allows us to continue evaluating the expression as we process more tokens)
                }
            }
            //store the count of items left on the stack in ViewBag for debugging purposes (to verify that there is exactly one number left, which should be the final result)
            ViewBag.DebugStackCount = stack.Count; 
            //if the stack does not contain exactly one number at this point, it means there was an error in the expression (e.g., too many operators or operands)
            return stack.Pop(); //pop the final result from the stack and return it as the output of the evaluation
        }   
    }
}
