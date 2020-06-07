using Microsoft.MixedReality.Toolkit.Utilities;

public class WebResponse<T>
{
    public bool Successful { get; private set; }
    public T Content { get; private set; }
    public byte[] ByteData { get; private set; }
    public long Code { get; private set; }
    public string ErrorMessage { get; private set; }

    public WebResponse(bool successful, T content, byte[] byteData, long code, string errorMessage)
    {
        Successful = successful;
        Content = content;
        ByteData = byteData;
        Code = code;
        ErrorMessage = errorMessage;
    }

    public WebResponse(T content, byte[] byteData, long code) : this(true, content, byteData, code, "")
    { }

    public WebResponse(string errorMessage, long code) : this(false, default, new byte[0], code, errorMessage)
    { }

    public static WebResponse<string> FromResponse(Response response)
    {
        if (response.Successful)
        {
            return new WebResponse<string>(
            response.Successful,
            response.ResponseBody,
            response.ResponseData,
            response.ResponseCode,
            "");
        }
        else
        {

        }
        return new WebResponse<string>(
            response.Successful,
            response.ResponseBody,
            response.ResponseData,
            response.ResponseCode,
            response.ResponseBody);
    }
}
