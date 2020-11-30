using System;

namespace simple_crud.Data
{
    public sealed class CouldNotConnectToDbException : Exception
    {
        public CouldNotConnectToDbException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}
