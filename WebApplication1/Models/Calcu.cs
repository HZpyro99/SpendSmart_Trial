using System.ComponentModel.DataAnnotations;

// This class represents the model for the calculator, containing the expression to be evaluated and the resulting value. It is used to pass data between the view and the controller in an ASP.NET MVC application.
namespace ActualCalculator.Models
{
    public class Calcu
    {
        public string Expression { get; set; }
       
        public double Value {  get; set; }
        
    }
}
