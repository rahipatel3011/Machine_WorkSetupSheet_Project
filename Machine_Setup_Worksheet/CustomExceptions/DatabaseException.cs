using System.Runtime.Serialization;
using System;

namespace Machine_Setup_Worksheet.CustomExceptions
{
    [Serializable]
    public class DatabaseException: Exception
    {

        public object EntityName { get; set; }
        public DatabaseException() { }

        public DatabaseException(string message) : base(message) { }
        public DatabaseException(string message, Exception inner) : base(message, inner) { }

        public DatabaseException(string message, string entityName, Exception inner)
            : base(message, inner)
        {
            EntityName = entityName;
        }

    }
}
