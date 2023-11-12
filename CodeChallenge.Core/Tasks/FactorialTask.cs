namespace CodeChallenge.Tasks
{
    public class FactorialTask : BaseTask
    {

        public override string Name => "Factorial";

        public override string GetTaskTemplate()
        {
            return @"
using System;

public class CodeChallenge
{
    /// <summary>
    /// Findet Factorial af tallene op til og med number f.eks. Input: 5 Output: 120 
    /// </summary>
    public long Factorial(int number)
    {

    }
}
";
        }

        public override object[] GetArguments()
        {
            return new object[] { 5 };
        }

        public int GetSolutionResult()
        {
            var number = 5;

            var result = 1;
            for (var n = 1; n <= number; n++)
            {
                result *= n;
            }

            return result;
        }

        public override bool ValidateResult(object value)
        {
            int result = int.Parse(value.ToString());
            int solution = GetSolutionResult();

            return (result == solution);
        }
    }
}

