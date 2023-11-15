using System;
using System.Reflection;

namespace CodeChallenge.Core.Compiler
{
    public static class Runner
    {
        public static object InvokeSolution(Assembly assembly, string method, object[] arguments)
        {
            // Execute the library code
            var libraryClassType = assembly.GetType("CodeChallenge");
            var libraryInstance = Activator.CreateInstance(libraryClassType);

            var libraryMethod = libraryClassType.GetMethod(method); // ("Factorial");
            var output = libraryMethod.Invoke(libraryInstance, arguments);

            return output;
        }
    }
}

