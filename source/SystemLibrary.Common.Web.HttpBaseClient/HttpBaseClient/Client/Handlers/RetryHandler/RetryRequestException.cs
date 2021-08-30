using System;

namespace SystemLibrary.Common.Web
{
    /// <summary>
    /// A retry request exception is thrown if:
    /// - "retryOnceOnRequestCancelled" is set to True, then if the retry is erroring too, this exception is thrown to the callee (you)
    /// - If retryOnceOnRequestCancelled is False, this exception will never occur for the callee (you)
    /// 
    /// Note: A retry request gets a fixed timeout of 10 seconds to finish the request
    /// </summary>
    public class RetryRequestException : Exception
    {
        public RetryRequestException(string message = null, Exception inner = null) : base(message, inner)
        {
        }
    }
}
