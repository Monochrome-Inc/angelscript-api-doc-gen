using System;
using JavaDocParser.Exceptions;
using JavaDocParser.Models;

namespace JavaDocParser.Factories
{
    public abstract class JavaDocFieldFactory
    {
        private static JavaDocFieldTypes GetJavaDocFieldTypeByName(string name)
        {
            if (name.Equals("@return"))
                return JavaDocFieldTypes.JavadocFieldTypeReturn;

            return JavaDocFieldTypes.JavadocFieldTypeUnknown;
        }

        public static JavaDocField ParseJavaDocField(string buffer)
        {
            if (!buffer.StartsWith("@"))
                throw new NotAJavaDocFieldException();

            int spaceIndex = buffer.IndexOf(" ", StringComparison.Ordinal);
            if (spaceIndex == -1)
                throw new BadJavaDocFieldFormatException();

            string name = buffer.Substring(0, spaceIndex);
            JavaDocFieldTypes type = GetJavaDocFieldTypeByName(name);
            if (type == JavaDocFieldTypes.JavadocFieldTypeUnknown)
                throw new UnknownJavaDocFieldException();

            string value = buffer.Substring(spaceIndex + 1);
            return new JavaDocField(type, value);
        }
    }
}
