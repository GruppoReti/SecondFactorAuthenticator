using System;

namespace SecondFactorAuthenticator.Database
{
    class DatabaseException : Exception
    {
        //
        // Summary:
        //     Initializes a new instance of the System.Exception class.
        public DatabaseException() : base()
        {
            // Do nothing special
        }

        //
        // Summary:
        //     Initializes a new instance of the System.Exception class with a specified error
        //     message.
        //
        // Parameters:
        //   message:
        //     The message that describes the error.
        public DatabaseException(string message) : base(message)
        {
            // Do nothing special
        }

        //
        // Summary:
        //     Initializes a new instance of the System.Exception class with a specified error
        //     message and a reference to the inner exception that is the cause of this exception.
        //
        // Parameters:
        //   message:
        //     The error message that explains the reason for the exception.
        //
        //   innerException:
        //     The exception that is the cause of the current exception, or a null reference
        //     (Nothing in Visual Basic) if no inner exception is specified.
        public DatabaseException(string message, Exception innerException) : base(message, innerException)
        {
            // Do nothing special
        }

    }
}
