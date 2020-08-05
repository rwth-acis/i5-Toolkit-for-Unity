namespace i5.Toolkit.Core.Utilities
{
    /// <summary>
    /// Container object which is produced by IContentLoader classes after they fetched content from the Web
    /// </summary>
    /// <typeparam name="T">The return type of the WebResponse</typeparam>
    public class WebResponse<T>
    {
        /// <summary>
        /// Set to true if the Web request was successful
        /// </summary>
        public bool Successful { get; private set; }
        /// <summary>
        /// Contains the content of the Web request
        /// Set to default value if the Web request failed
        /// </summary>
        public T Content { get; private set; }
        /// <summary>
        /// Byte data of the Web requests response body
        /// </summary>
        public byte[] ByteData { get; private set; }
        /// <summary>
        /// The response code of the Web request
        /// </summary>
        public long Code { get; private set; }
        /// <summary>
        /// An error message if any occurred
        /// </summary>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// Creates a new WebResponse object
        /// </summary>
        /// <param name="successful">States whether the request was successful</param>
        /// <param name="content">Contains the content of the response</param>
        /// <param name="byteData">Contains the byte data of the response body</param>
        /// <param name="code">The response code</param>
        /// <param name="errorMessage">Any error messages that might have occurred</param>
        public WebResponse(bool successful, T content, byte[] byteData, long code, string errorMessage)
        {
            Successful = successful;
            Content = content;
            ByteData = byteData;
            Code = code;
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Creates a successul WebResponse with the given content and code
        /// </summary>
        /// <param name="content">The content of the Web request's response</param>
        /// <param name="byteData">The byte data of the response body</param>
        /// <param name="code">The response code</param>
        public WebResponse(T content, byte[] byteData, long code) : this(true, content, byteData, code, "")
        { }

        /// <summary>
        /// Creates a failed WebResponse with the specified error message and code
        /// </summary>
        /// <param name="errorMessage">The error message</param>
        /// <param name="code">The response code</param>
        public WebResponse(string errorMessage, long code) : this(false, default, new byte[0], code, errorMessage)
        { }

        ///// <summary>
        ///// Converts MRTK's Response objects to a WebResponse object
        ///// </summary>
        ///// <param name="response">The MRTK Response object to convert</param>
        ///// <returns>A WebResponse object with the same contents as the response object</returns>
        //public static WebResponse<string> FromResponse(Response response)
        //{
        //    // Response objects always return string values, so they are of type WebResponse<string>
        //    if (response.Successful)
        //    {
        //        // return a successful response
        //        return new WebResponse<string>(
        //        response.ResponseBody,
        //        response.ResponseData,
        //        response.ResponseCode);
        //    }
        //    else
        //    {
        //        // construct a failed response
        //        // the error message is in the response body
        //        return new WebResponse<string>(
        //        response.ResponseBody,
        //        response.ResponseCode);
        //    }
        //}
    }
}