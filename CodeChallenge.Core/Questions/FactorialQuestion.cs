namespace CodeChallenge.Questions
{
    public class FactorialQuestion : BaseQuestion
    {

        public override string MethodToInvoke => "Factorial";

        public override string GetTaskTemplate()
        {
            return @"using System;

public class CodeChallenge
{
    /// <summary>
    /// Find the factorial of the input. For example, factorial of 3 is 3 * 2 * 1 which equals to 6
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

