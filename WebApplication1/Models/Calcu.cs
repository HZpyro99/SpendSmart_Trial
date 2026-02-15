using System.ComponentModel.DataAnnotations;

namespace ActualCalculator.Models
{
    public class Calcu
    {
        public string? but { get; set; }
        public double result { get; set; }

        public double actual { get; set; }

        public int numerator { get; set; }

        public int denominator { get; set; } =
             0;
        
        public string? opers {get; set; }
    }
}
//1st step create a script that triggers when button is pressed and sends the values of
//numerator, denominator, and the operator to the controller.