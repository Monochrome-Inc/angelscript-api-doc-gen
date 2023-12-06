namespace JavaDocParser.Models
{
    /// <summary>
    /// Class that represent a JavaDoc field.
    /// We call "fields" the "[AT]param", "[AT]return" elements and similar.
    /// </summary>
    public class JavaDocField
    {
        /// <summary>
        /// Create a new JavaDoc field using a specific type and value.
        /// </summary>
        /// <param name="type">The type to set.</param>
        /// <param name="value">The value to set.</param>
        public JavaDocField(JavaDocFieldTypes type, string value)
        {
            Type = type;
            Value = value;
        }

        /// <summary>
        /// The type of this JavaDoc's field.
        /// </summary>
        public JavaDocFieldTypes Type { get; set; }

        /// <summary>
        /// The value of this JavaDoc's field.
        /// </summary>
        public string Value { get; set; }
    }
}
