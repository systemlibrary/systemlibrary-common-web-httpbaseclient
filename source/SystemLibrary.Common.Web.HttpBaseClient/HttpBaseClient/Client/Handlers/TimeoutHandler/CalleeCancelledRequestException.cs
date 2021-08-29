using System;

namespace SystemLibrary.Common.Web
{
    /// <summary>
    /// Callee cancelled request exception is thrown when the callee cancels the request, which
    /// occurs through the cancellation token that you can pass to each request methods
    /// </summary>
    public class CalleeCancelledRequestException : Exception
    {
        public CalleeCancelledRequestException(string message = null, Exception inner = null) : base(message, inner)
        {
        }
    }
}
