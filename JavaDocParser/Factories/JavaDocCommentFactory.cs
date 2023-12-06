using System;
using JavaDocParser.Exceptions;
using JavaDocParser.Models;

namespace JavaDocParser.Factories
{
    public abstract class JavaDocCommentFactory
    {
        public static JavaDocComment ParseJavaDocComment(string buffer)
        {
            if (!buffer.StartsWith("/**") || !buffer.EndsWith("*/"))
                throw new NotAJavaDocCommentException();

            buffer = buffer.Replace("/**\n * ", "");
            buffer = buffer.Replace("\n */", "");

            if (!buffer.Contains(" * @"))
                return new JavaDocComment(buffer);

            string description = buffer.Substring(0, buffer.IndexOf(" * @", StringComparison.Ordinal));
            if (description.Contains("\n"))
                description = description.Replace("\n", "");

            JavaDocComment comment = new JavaDocComment(description);

            string[] fields = buffer.Split('@');
            for (int i = 1; i < fields.Length; i++)
            {
                string fieldString = "@" + fields[i];
                if (fieldString.Contains(" */"))
                    fieldString = fieldString.Replace(" */", "");

                JavaDocField field = JavaDocFieldFactory.ParseJavaDocField(fieldString);
                comment.AddField(field);
            }

            return comment;
        }
    }
}
