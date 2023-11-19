using System;
using CodeChallenge.Questions;

namespace CodeChallenge.Core.Questions
{
	public class LargestQuestion : BaseQuestion
	{
		public LargestQuestion()
		{
		}

        public override string MethodToInvoke => "Largest";

        public override object[] GetArguments()
        {
            return new object[]
            {
                10,
                12
            };
        }

        public override string GetTaskTemplate()
        {
            return @"using System;

public class CodeChallenge
{
    /// <summary>
    /// Find the largest integer of a and b 
    /// </summary>
    public int Largest(int b, int b)
    {

    }
}
";
        }

        public int GetSolutionResult()
        {
            int a = (int)GetArguments()[0];
            int b = (int)GetArguments()[1];

            return (a < b) ? b : a;
        }

        public override bool ValidateResult(object value)
        {
            int result = int.Parse(value.ToString());
            int solution = GetSolutionResult();

            return (result == solution);
        }
    }
}

