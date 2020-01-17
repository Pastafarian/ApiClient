using System;
using System.Runtime.Serialization;

namespace Avalara.ApiClient.Authorization
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class AuthorizationException : Exception
    {
        public AuthorizationException()
        {
        }

        public AuthorizationException(string message) : base(message)
        {
        }

        public AuthorizationException(string message, Exception inner) : base(message, inner)
        {
        }

        protected AuthorizationException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
