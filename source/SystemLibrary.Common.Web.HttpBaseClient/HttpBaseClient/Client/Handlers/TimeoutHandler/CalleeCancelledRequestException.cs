﻿using System;

namespace SystemLibrary.Common.Web
{
    /// <summary>
    /// The Callee Cancelled Request Exception is thrown when the callee (you) cancel the request.
    /// - To cancel a request you must pass a 'Cancellation Token', and then cancel the request through the Cancellation Token
    /// </summary>
    public class CalleeCancelledRequestException : Exception
    {
        public CalleeCancelledRequestException(string message = null, Exception inner = null) : base(message, inner)
        {
        }
    }
}
