using System;

namespace JavaDocParser.Exceptions
{
    /// <summary>
    /// Exception class used to signal that the JavaDoc comment parser did not recognized the buffer as a JavaDoc comment.
    /// </summary>
    public class NotAJavaDocCommentException : Exception
    {
        public NotAJavaDocCommentException() {}

        public NotAJavaDocCommentException(string message) : base(message) {}

        public NotAJavaDocCommentException(string message, Exception inner) : base(message, inner) {}
    }
}
