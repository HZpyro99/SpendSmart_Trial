using System.ComponentModel.DataAnnotations;

namespace Calculator.Models
{
    public class Calc
    {
        public decimal result { get; set; }
   
        public decimal numerator { get; set; }

        public decimal denominator { get; set; }
    }
}
