using JavaDocParser.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JavaDocParser.Factories;
using JavaDocParser.Models;

namespace TestJavaDocParser
{
    [TestClass]
    public class TestJavaDocComment
    {
        [TestMethod]
        public void TestJavaDocCommentParsingSuccessWithoutFields()
        {
            const string testSubject = "/**\n * Hello World!\n */";
            JavaDocComment actual = JavaDocCommentFactory.ParseJavaDocComment(testSubject);

            Assert.IsNotNull(actual);
            Assert.AreEqual("Hello World!", actual.Description);
        }

        [TestMethod]
        public void TestJavaDocCommentParsingSuccessWithOneField()
        {
            const string testSubject = "/**\n * Hello World!\n * @return This is a return field. */";
            JavaDocComment actual = JavaDocCommentFactory.ParseJavaDocComment(testSubject);

            Assert.IsNotNull(actual);
            Assert.AreEqual("Hello World!", actual.Description);
            Assert.AreEqual(1, actual.GetFields().Count);
            JavaDocField field = (JavaDocField)actual.GetFields()[0];
            Assert.AreEqual(JavaDocFieldTypes.JavadocFieldTypeReturn, field.Type);
            Assert.AreEqual("This is a return field.", field.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(NotAJavaDocCommentException), "JavaDoc comment parsing was supposed to fail, not to be a success!")]
        public void TestJavaDocCommentParsingFailNotAJavaDocComment()
        {
            const string testSubject = "// Hello World!";
            JavaDocCommentFactory.ParseJavaDocComment(testSubject);
        }
    }
}
