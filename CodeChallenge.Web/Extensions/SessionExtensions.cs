using System;
using System.Text.Json;

namespace CodeChallenge.Web.Helpers
{
    public static class SessionExtensions
    {
        public static T GetObject<T>(this ISession session, string key)
        {
            var data = session.GetString(key);
            if (data == null)
            {
                return default(T);
            }

            return JsonSerializer.Deserialize<T>(data);

        }

        public static void SetObject(this ISession session, string key, object value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));

        }
    }
}
