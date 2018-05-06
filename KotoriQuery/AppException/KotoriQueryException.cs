using System;

namespace KotoriQuery.AppException
{
    public class KotoriQueryException : Exception
    {
        public KotoriQueryException(string message) : base(message)
        {
        }
    }
}