using System;
using CodeChallenge.Core;

namespace CodeChallenge.Web.Helpers
{
	public class QuizSessionManager : IQuizSessionManager
	{
        private ISession session;

		public QuizSessionManager(IHttpContextAccessor httpContextAccessor)
		{
            this.session = httpContextAccessor.HttpContext.Session;
		}

        public T GetObject<T>(string key)
        {
            return session.GetObject<T>(key);
        }

        public void SetObject(string key, object data)
        {
            session.SetObject(key, data);
        }
    }
}

