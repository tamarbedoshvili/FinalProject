
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjects
{
    public class Calculator
    {
        public double firstnumber { get; set; }
        public double secondnumber { get; set; }
        public string action { get; set; }

        public static double Sum(double firstnumber, double secondnumber)
        {
            return firstnumber + secondnumber;
        }
        public static double Difference(double firstnumber, double secondnumber)
        {
            return firstnumber - secondnumber;
        }
        public static double Product(double firstnumber, double secondnumber)
        {
            return firstnumber * secondnumber;
        }
        public static double Division(double firstnumber, double secondnumber)
        {
            return firstnumber / secondnumber;

        }
        public static bool Validation(string input)
        {
            try
            {
                double.Parse(input);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
           

               
        }
    }

    internal class Program
    {
        
        static void Main(string[] args)
        {
            Console.WriteLine("CONSOLE CALCULATOR");
            Console.WriteLine("TYPE 'end' IF YOU WANT TO EXIT");
            string input = string.Empty;

            while (true)
            { 
                
                Calculator calculator = new Calculator();
                Console.WriteLine("Please enter the first number:");
                 input = Console.ReadLine();
                if(input.ToLowerInvariant() == "end")
                {
                    break;
                }
                 while (!Calculator.Validation(input))
                    {
                    Console.WriteLine("Please enter numbers:");
                     input = Console.ReadLine();
                }
                calculator.firstnumber = double.Parse(input);
                Console.WriteLine("Please enter the second number:");
                 input = Console.ReadLine();
                if (input.ToLowerInvariant() == "end")
                {
                    break;
                }
                while (!Calculator.Validation(input))
                {
                    Console.WriteLine("Please enter numbers:");
                    input = Console.ReadLine();
                }
                calculator.secondnumber = double.Parse(input);
                Console.WriteLine("Please enter the action(+,_,*,/)");
                input = Console.ReadLine();
                if(input.ToLowerInvariant() == "end")
                {
                    break;
                }
                calculator.action = input;
                

                switch (calculator.action)
                {
                    case "+":
                        var result1 = Calculator.Sum(calculator.firstnumber, calculator.secondnumber);
                        Console.WriteLine(result1);
                        break;
                    case "-":
                        var result2 = Calculator.Difference(calculator.firstnumber, calculator.secondnumber);
                        Console.WriteLine(result2);
                        break;
                    case "*":
                        var result3 = Calculator.Product(calculator.firstnumber, calculator.secondnumber);
                        Console.WriteLine(result3);
                        break;
                    case "/":

                        if (calculator.secondnumber == 0)
                        {
                            Console.WriteLine("divide by 0 not possible");
                        }
                        else
                        {
                            var result4 = Calculator.Division(calculator.firstnumber, calculator.secondnumber);
                            Console.WriteLine(result4);

                        }
                       
                        break;
                    default:
                        Console.WriteLine("NON-EXISTING ACTION");
                        break;
                }
                
            }

        }
    }
}
