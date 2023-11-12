using System;

namespace CodeChallenge.Tasks
{

    public abstract class BaseTask
    {
        public int Id { get; set; }

        public abstract string Name { get; }

        public abstract string GetTaskTemplate();

        public abstract object[] GetArguments();

        public abstract bool ValidateResult(object result);

    }

}

