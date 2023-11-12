using System;
using CodeChallenge.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CodeChallenge.Core.Tasks
{
	public class FindAverageTask : BaseTask
	{
        private new object[] arguments;

        public override string Name => "FindAverage";


        public FindAverageTask()
		{
            arguments = new object[] { new int[] { 1, 2, 3, 4, 5 } };
        }

        public override object[] GetArguments()
        {
            return arguments;
        }

        public override string GetTaskTemplate()
        {
            return @"
using System;
public class CodeChallenge
{

    /// <summary>
    /// Find gennemsnittet
    /// </summary>
    public int FindAverage(int[] numbers)
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

