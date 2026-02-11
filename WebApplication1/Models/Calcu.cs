using System.ComponentModel.DataAnnotations;

namespace ActualCalculator.Models
{
    public class Calcu
    {
        public string but { get; set; }
        public double result { get; set; }

        public double actual { get; set; }

        public int numerator { get; set; }

        public int denominator { get; set; }
        
        public string opers {get; set; }
    }
}
