using System;
using CodeChallenge.Questions;

namespace CodeChallenge.Core.Questions
{
	public class FrequenceQuestion : BaseQuestion
    {
		public FrequenceQuestion()
		{
		}

        public override string MethodToInvoke => "Frequence";

        public override object[] GetArguments()
        {
            return new object[]
            {
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua",
                'a'
            };
        }

        public override string GetTaskTemplate()
        {
            return @"using System;

public class CodeChallenge
{
    /// <summary>
    /// Find the frequence of 'c' within 'text' 
    /// </summary>
    public int Frequence(string text, char c)
    {

    }
}
";
        }

        public int GetSolutionResult()
        {
            string text = (string) GetArguments()[0];
            char c = (char) GetArguments()[1];
            int count = 0;

            for (var i = 0; i < text.Length; i++)
            {
                if (text[i] == c)
                {
                    count++;
                }
            }

            return count;
        }

        public override bool ValidateResult(object value)
        {
            int result = int.Parse(value.ToString());
            int solution = GetSolutionResult();

            return (result == solution);
        }
    }
}

