using System;

namespace JavaDocParser.Exceptions
{
    /// <summary>
    /// Exception class used to signal that the JavaDoc field being parsed is unknown.
    /// </summary>
    public class UnknownJavaDocFieldException : Exception
    {
        public UnknownJavaDocFieldException() {}

        public UnknownJavaDocFieldException(string message) : base(message) {}

        public UnknownJavaDocFieldException(string message, Exception inner) : base(message, inner) {}
    }
}
