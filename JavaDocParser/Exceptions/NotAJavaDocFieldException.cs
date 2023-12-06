using System;

namespace JavaDocParser.Exceptions
{
    /// <summary>
    /// Exception class used to signal that the JavaDoc field parser did not recognized the buffer as a JavaDoc field.
    /// </summary>
    public class NotAJavaDocFieldException : Exception
    {
        public NotAJavaDocFieldException() {}

        public NotAJavaDocFieldException(string message) : base(message) {}

        public NotAJavaDocFieldException(string message, Exception inner) : base(message, inner) {}
    }
}
