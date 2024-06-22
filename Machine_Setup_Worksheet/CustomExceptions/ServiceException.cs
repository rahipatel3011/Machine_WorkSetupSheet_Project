using System;

namespace Machine_Setup_Worksheet.CustomExceptions
{
    /// <summary>
    /// Custom exception thrown for service-related errors.
    /// </summary>
    public class ServiceException : Exception
    {
        /// <summary>
        /// Default constructor for ServiceException.
        /// </summary>
        public ServiceException() { }

        /// <summary>
        /// Constructor for ServiceException with a specified error message.
        /// </summary>
        /// <param name="message">Error message that describes the exception.</param>
        public ServiceException(string message) : base(message) { }

        /// <summary>
        /// Constructor for ServiceException with a specified error message and inner exception.
        /// </summary>
        /// <param name="message">Error message that describes the exception.</param>
        /// <param name="inner">The exception that caused the current exception.</param>
        public ServiceException(string message, Exception inner) : base(message, inner) { }
    }
}
