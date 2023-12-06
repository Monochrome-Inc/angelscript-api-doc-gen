using System.Collections;

namespace JavaDocParser.Models
{
    /// <summary>
    /// Class that represent a JavaDoc comment.
    /// </summary>
    public class JavaDocComment
    {
        /// <summary>
        /// The list of fields this JavaDoc has.
        /// </summary>
        private readonly ArrayList _fields = new ArrayList();

        /// <summary>
        /// Create a new JavaDoc comment with a specific description.
        /// </summary>
        /// <param name="description">The description to set.</param>
        public JavaDocComment(string description)
        {
            Description = description;
        }

        /// <summary>
        /// The JavaDoc's description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Returns the list of fields that this JavaDoc comment has.
        /// </summary>
        /// <returns>The list of fields this JavaDoc comment has.</returns>
        public ArrayList GetFields()
        {
            return _fields;
        }

        /// <summary>
        /// Add a JavaDoc field in this JavaDoc comment.
        /// </summary>
        /// <param name="field">The JavaDoc field to add.</param>
        public void AddField(JavaDocField field)
        {
            if (field == null)
                return;

            _fields.Add(field);
        }
    }
}
