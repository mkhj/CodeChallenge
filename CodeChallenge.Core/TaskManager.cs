using System.Reflection;
using CodeChallenge.Core.Tasks;
using CodeChallenge.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Text;

namespace CodeChallenge.Core;

public class TaskManager
{
    private List<BaseTask> tasks;

    private BaseTask currentTask;

    public TaskManager()
    {
        this.tasks = new List<BaseTask>();

        tasks.Add(new FactorialTask() { Id = 1 });
        tasks.Add(new FindAverageTask() { Id = 2 });

    }

    public bool HasMoreTasks
    {
        get
        {
            return tasks.Count > 0;
        }
    }

    public BaseTask GetNewTask()
    {
        currentTask = tasks[0];

        tasks.Remove(currentTask);

        return currentTask;
    }

    public BaseTask GetTaskById(int id)
    {
        return tasks.Find(x => x.Id == id);
    }

    public TaskResult ValidateTask(int taskId, string answer)
    {
        var task = currentTask; // GetTaskById(taskId);
        if(task == null)
        {
            return new TaskResult()
            {
                Status = false,
                StatusMessage = "Task is null"
            };
        }

        var result = Compiler.BuildAndRun(answer, task);
        if (!result.Success) // Compiler error
        {
            return new TaskResult()
            {
                Status = false,
                StatusMessage = string.Join(", ", result.GetErrors())
            };
        }

        var correctResult = task.ValidateResult(result.Output);
        if (!correctResult)
        {
            return new TaskResult()
            {
                Status = false,
                StatusMessage = "Incorrect, try again"
            };
        }

        return new TaskResult()
        {
            Status = true,
            StatusMessage = "Yesssir!!"
        };
    }
}

public class TaskResult
{
    public bool Status { get; set; }
    public string? StatusMessage { get; set; }

}


public class CompileResult
{
    private List<string> errors = new List<string>();

    public bool Success
    {
        get { return errors.Count == 0; }
    }

    public object? Output { get; set; }

    public void AddError(string error)
    {
        errors.Add(error);
    }

    public List<string> GetErrors()
    {
        return errors;
    }


}

public static class Compiler
{

    public static CompileResult BuildAndRun(string code, BaseTask task)
    {

        // Set up the compilation options
        var compilationOptions = new CSharpCompilationOptions(
             OutputKind.DynamicallyLinkedLibrary,
             optimizationLevel: OptimizationLevel.Release

        );

        // Add necessary references
        var references = new[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(System.Console).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(System.Runtime.AssemblyTargetedPatchBandAttribute).Assembly.Location),
            MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location)
        };

        // Compile the code
        //var codeString = SourceText.From(task.GetTaskTemplate());
        var syntaxTree = SyntaxFactory.ParseSyntaxTree(code);

        var compilation = CSharpCompilation.Create("LibraryAssembly")
            .WithOptions(compilationOptions)
            .AddReferences(references)
            .AddSyntaxTrees(syntaxTree);


        var compileResult = new CompileResult();

        using var ms = new MemoryStream();

        var emitResult = compilation.Emit(ms);

        // Handle compilation errors
        var compilerErrors = !emitResult.Success;
        if (compilerErrors)
        {
            foreach (var diagnostic in emitResult.Diagnostics)
            {
                compileResult.AddError(diagnostic.ToString());
            }

            return compileResult;
        }

        ms.Seek(0, SeekOrigin.Begin);

        // Load the compiled assembly
        var assembly = Assembly.Load(ms.ToArray());

        // Execute the library code
        var libraryClassType = assembly.GetType("CodeChallenge");
        var libraryInstance = Activator.CreateInstance(libraryClassType);

        var libraryMethod = libraryClassType.GetMethod(task.Name); // ("Factorial");
        compileResult.Output = libraryMethod.Invoke(libraryInstance, task.GetArguments());

        return compileResult;
    }
};
