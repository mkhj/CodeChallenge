using CodeChallenge.Core.Compiler;
using CodeChallenge.Core.Questions;
using CodeChallenge.Questions;
using CodeChallenge.Web.Helpers;
using Microsoft.CodeAnalysis;

namespace CodeChallenge.Core
{

    public interface IQuizSessionManager
    {
        public T GetObject<T>(string key);
        public void SetObject(string key, object data);
    }


    public class QuizManager
    {
        private List<BaseQuestion> _questions;
        private IQuizSessionManager _sessionManager;

        private const string QUIZ_QUESTION_IDS_SESSION_KEY = "quiz_question_ids";
        private const string QUIZ_CURRENT_QUESTION_ID = "quiz_current_question_id";
        private const string QUIZ_ACTIVE_QUESTION_SESSION_KEY = "quiz_active_questions_id";
        private const string QUIZ_IN_PROGRESS_SESSION_KEY = "quiz_in_progress";


        private QuizManager(List<BaseQuestion> questions, IQuizSessionManager session)
        {
            _questions = questions;
            _sessionManager = session;
        }

        /// <summary>
        /// Factory Method
        /// </summary>
        /// <returns></returns>
        public static QuizManager Create(IQuizSessionManager sessionManager)
        {
            if(sessionManager == null)
            {
                throw new ArgumentNullException("SessionManager cannot be null");
            }

            var questions = new List<BaseQuestion>
            {
                new FactorialQuestion() { Id = 1 },
                new AverageQuestion() { Id = 2 },
                new FrequenceQuestion() { Id = 3 }
            };

            var quizManager = new QuizManager(questions, sessionManager);
            return quizManager;
        }

        /// <summary>
        /// Current quiz questions
        /// </summary>
        public List<int> QuizQuestionIds
        {
            get
            {
                var value = _sessionManager.GetObject<List<int>>(QUIZ_QUESTION_IDS_SESSION_KEY);
                if(value == null)
                {
                    value = new List<int>();
                }

                return value;
            }
            private set
            {
                _sessionManager.SetObject(QUIZ_QUESTION_IDS_SESSION_KEY, value);
            }
        }

        public bool QuizInProgress
        {
            get
            {
                var value = _sessionManager.GetObject<bool>(QUIZ_IN_PROGRESS_SESSION_KEY);
                if(value == null)
                {
                    value = false;
                }

                return value;
            }
            private set
            {
                _sessionManager.SetObject(QUIZ_IN_PROGRESS_SESSION_KEY, value);
            }
        }

        public bool IsLastQuestion
        {
            get
            {
                return QuizQuestionIds[QuizQuestionIds.Count - 1] == CurrentQuizId;
            }
        }

        public int CurrentQuizId
        {
            get
            {
                return _sessionManager.GetObject<int>(QUIZ_CURRENT_QUESTION_ID);
            }
            private set
            {
                _sessionManager.SetObject(QUIZ_CURRENT_QUESTION_ID, value);
            }
        }

        /// <summary>
        /// Generates a new quiz with questions in random order
        /// </summary>
        /// <returns></returns>
        public List<int> GenerateNewQuiz()
        {
            if (QuizInProgress)
            {
                return QuizQuestionIds;
            }

            // Randomize challenges
            var ids = _questions.Select(x => x.Id).ToList();
            ids.Shuffle<int>();

            QuizQuestionIds = ids;

            QuizInProgress = true;

            CurrentQuizId = QuizQuestionIds[0];

            return QuizQuestionIds;
        }

        /// <summary>
        /// 
        /// </summary>
        public void FinalizeQuiz()
        {
            QuizInProgress = false;
        }

        public BaseQuestion? GetFirstQuestion()
        {
            return GetQuestionById(CurrentQuizId);
        }

        public BaseQuestion? GetNextQuestion()
        {
            var currentIndex = QuizQuestionIds.IndexOf(CurrentQuizId);
            if(currentIndex == -1)
            {
                return null;
            }

            if (IsLastQuestion)
            {
                return null;
            }

            var nextIndex = currentIndex + 1;
            var nextQuestionId = QuizQuestionIds[nextIndex];
            var nextQuestion = GetQuestionById(nextQuestionId);

            CurrentQuizId = nextQuestion.Id;

            return nextQuestion;
        }


        public BaseQuestion? GetQuestionById(int id)
        {
            var task = _questions.Find(x => x.Id == id);
            return task;
        }

        public TaskResult ValidateAnswer(string answer)
        {
            var currentTask = GetQuestionById(CurrentQuizId);
            if (currentTask == null)
            {
                return new TaskResult()
                {
                    Status = false,
                    StatusMessage = "Task is null"
                };
            }

            var buildResult = Compiler.Compiler.BuildSolution(answer);
            if (!buildResult.Success) // Are there any compiler errors?
            {
                return new TaskResult()
                {
                    Status = false,
                    StatusMessage = string.Join(", ", buildResult.GetErrors())
                };
            }

            var output = Runner.InvokeSolution(buildResult.GetAssembly(), currentTask.MethodToInvoke, currentTask.GetArguments());

            var correctResult = currentTask.ValidateResult(output);
            if (!correctResult) // Did the provided solution give the expected output?
            {
                return new TaskResult()
                {
                    Status = false,
                    StatusMessage = "Incorrect, try again"
                };
            }

            // Solution gave the right output
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

}









