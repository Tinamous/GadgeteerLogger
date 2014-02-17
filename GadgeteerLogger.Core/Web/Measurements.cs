using Gadgeteer.Networking;

namespace Tinamous.GadgeteerLogger.Core.Web
{
    /// <summary>
    /// Responsible for reading and writing measurments to Tinamous
    /// </summary>
    static class Measurements
    {
        public static void GetMeasurements(HttpRequest.ResponseHandler responseHandler)
        {
            var getRequest = WebHelper.CreateGetRequest("/Measurements");
            WebHelper.AddAuthorizationHeaders(getRequest);
            getRequest.ResponseReceived += responseHandler;
            getRequest.SendRequest();
        }

        public static HttpRequest CreatePostRequest(string temperature, string relativeHumidity, string lightLevel, string pressure)
        {
            string postContent = "{ ";
            postContent += "\"field1\" : \"" + temperature + "\", ";
            postContent += "\"field2\" : \"" + relativeHumidity + "\", ";
            postContent += "\"field3\" : \"" + lightLevel + "\", ";
            postContent += "\"field4\" : \"" + pressure + "\", ";
            postContent += "\"channel\" : \"0\" ";
            postContent += "}";
            postContent += "\n\r";
            postContent += "\n\r";

            POSTContent content = POSTContent.CreateTextBasedContent(postContent);
            HttpRequest request = WebHelper.CreatePostRequest(content, "/Measurements");
            return request;
        }
    }
}
