using System;
using DateAccess.Services.ContactService.Call.Models;

namespace DateAccess.Services.ContactService.Call.Exceptions
{
    public class UnfinishedCallException : Exception
    {
        public UnfinishedCallException() { }
        public UnfinishedCallException(string message):base(message) { }
        public UnfinishedCallException(string message, CallDetail callDetail)
            : base(message)
        {
            CallDetail = callDetail;

        }
        public UnfinishedCallException(string message, Exception inner) : base(message, inner) { }
        public UnfinishedCallException(string message, Exception inner, CallDetail callDetail)
            : base(message, inner)
        {
            CallDetail = callDetail;
        }

        public CallDetail CallDetail { get; set; }
    }
}
