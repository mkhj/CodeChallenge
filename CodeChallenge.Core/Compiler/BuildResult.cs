using System;
using System.Reflection;

namespace CodeChallenge.Core.Compiler
{
    public class BuildResult
    {
        /// <summary>
        /// 
        /// </summary>
        private List<string> errors = new List<string>();

        /// <summary>
        /// 
        /// </summary>
        private Assembly? assembly { get; set; }

        public bool Success
        {
            get { return errors.Count == 0; }
        }

        public void AddError(string error)
        {
            errors.Add(error);
        }

        public List<string> GetErrors()
        {
            return errors;
        }

        public void AddAssembly(Assembly assembly)
        {
            this.assembly = assembly;
        }

        public Assembly GetAssembly()
        {
            return assembly;
        }
    }
}

