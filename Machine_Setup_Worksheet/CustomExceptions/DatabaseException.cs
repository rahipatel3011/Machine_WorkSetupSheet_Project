using System;
using System.Runtime.Serialization;

namespace Machine_Setup_Worksheet.CustomExceptions
{
    /// <summary>
    /// Custom exception thrown for database-related errors.
    /// </summary>
    [Serializable]
    public class DatabaseException : Exception
    {
        /// <summary>
        /// Gets or sets the name of the entity related to the database exception.
        /// </summary>
        public object EntityName { get; set; }

        /// <summary>
        /// Default constructor for DatabaseException.
        /// </summary>
        public DatabaseException() { }

        /// <summary>
        /// Constructor for DatabaseException with a specified error message.
        /// </summary>
        /// <param name="message">Error message that describes the exception.</param>
        public DatabaseException(string message) : base(message) { }

        /// <summary>
        /// Constructor for DatabaseException with a specified error message and inner exception.
        /// </summary>
        /// <param name="message">Error message that describes the exception.</param>
        /// <param name="inner">The exception that caused the current exception.</param>
        public DatabaseException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Constructor for DatabaseException with a specified error message, entity name, and inner exception.
        /// </summary>
        /// <param name="message">Error message that describes the exception.</param>
        /// <param name="entityName">Name of the entity related to the exception.</param>
        /// <param name="inner">The exception that caused the current exception.</param>
        public DatabaseException(string message, object entityName, Exception inner)
            : base(message, inner)
        {
            EntityName = entityName;
        }

    }
}
