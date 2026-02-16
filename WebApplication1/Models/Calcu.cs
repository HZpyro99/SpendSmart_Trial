using System.ComponentModel.DataAnnotations;

namespace ActualCalculator.Models
{
    public class Calcu
    {
        public string Expression { get; set; }
       
        public double Value {  get; set; }
        
    }
}
//1st step create a script that triggers when button is pressed and sends the values of
//numerator, denominator, and the operator to the controller.