using System;

namespace CodeChallenge.Questions
{

    public abstract class BaseQuestion
    {
        public int Id { get; set; }

        public abstract string MethodToInvoke { get; }

        public abstract string GetTaskTemplate();

        public abstract object[] GetArguments();

        public abstract bool ValidateResult(object result);

    }

}

