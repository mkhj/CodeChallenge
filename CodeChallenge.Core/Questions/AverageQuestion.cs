using System;
using CodeChallenge.Questions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CodeChallenge.Core.Questions
{
	public class AverageQuestion : BaseQuestion
	{
        private new object[] arguments;

        public override string MethodToInvoke => "Average";


        public AverageQuestion()
		{
            arguments = new object[] { new int[] { 1, 2, 3, 4, 5 } };
        }

        public override object[] GetArguments()
        {
            return arguments;
        }

        public override string GetTaskTemplate()
        {
            return @"using System;

public class CodeChallenge
{
    /// <summary>
    /// Find the average of all the numbers from the input.
    /// </summary>
    public int Average(int[] numbers)
    {

    }
}
";
        }

        public int GetSolutionResult()
        {
            var numbers = new List<int>((IEnumerable<int>)arguments[0]);
            var sum = 0;

            for (var i = 0; i < numbers.Count; i++)
            {
                sum += numbers[i];
            }

            return sum / numbers.Count;
        }

        public override bool ValidateResult(object value)
        {
            int result = int.Parse(value.ToString());
            int solution = GetSolutionResult();

            return (result == solution);

        }
    }
}

