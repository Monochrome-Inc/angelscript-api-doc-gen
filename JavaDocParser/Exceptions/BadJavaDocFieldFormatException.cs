using System;

namespace JavaDocParser.Exceptions
{
    /// <summary>
    /// Exception class used to signal that the JavaDoc field being parsed is badly formated.
    /// </summary>
    public class BadJavaDocFieldFormatException : Exception
    {
        public BadJavaDocFieldFormatException() {}

        public BadJavaDocFieldFormatException(string message) : base(message) {}

        public BadJavaDocFieldFormatException(string message, Exception inner) : base(message, inner) {}
    }
}
