using Microsoft.VisualStudio.TestTools.UnitTesting;
using JavaDocParser.Exceptions;
using JavaDocParser.Factories;
using JavaDocParser.Models;

namespace TestJavaDocParser
{
    [TestClass]
    public class TestJavaDocField
    {
        [TestMethod]
        public void TestJavaDocFieldParsingSuccess()
        {
            const string testSubject = "@return True if this is a success, false otherwise.";
            JavaDocField actual = JavaDocFieldFactory.ParseJavaDocField(testSubject);

            Assert.IsNotNull(actual);
            Assert.AreEqual(JavaDocFieldTypes.JavadocFieldTypeReturn, actual.Type);
            Assert.AreEqual("True if this is a success, false otherwise.", actual.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(BadJavaDocFieldFormatException), "JavaDoc field parsing was supposed to fail, not to be a success!")]
        public void TestJavaDocFieldParsingFailBadFormat()
        {
            const string testSubject = "@returnTrue.";
            JavaDocFieldFactory.ParseJavaDocField(testSubject);
        }

        [TestMethod]
        [ExpectedException(typeof(NotAJavaDocFieldException), "JavaDoc field parsing was supposed to fail, not to be a success!")]
        public void TestJavaDocFieldParsingFailNotAJavaDocField()
        {
            const string testSubject = "return True.";
            JavaDocFieldFactory.ParseJavaDocField(testSubject);
        }

        [TestMethod]
        [ExpectedException(typeof(UnknownJavaDocFieldException), "JavaDoc field parsing was supposed to fail, not to be a success!")]
        public void TestJavaDocFieldParsingFailUnknownJavaDocField()
        {
            const string testSubject = "@foobar True.";
            JavaDocFieldFactory.ParseJavaDocField(testSubject);
        }
    }
}
