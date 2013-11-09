using Gadgeteer.Networking;

namespace Tinamous.GadgeteerLogger.Core.Web
{
    static class Measurements
    {
        public static void GetMeasurements(HttpRequest.ResponseHandler responseHandler)
        {
            var getRequest = WebHelper.CreateGetRequest("/Measurements");
            WebHelper.AddAuthorizationHeaders(getRequest);
            getRequest.ResponseReceived += responseHandler;
            getRequest.SendRequest();
        }

        public static HttpRequest CreatePostRequest(double temperature, double relativeHumidity)
        {
            string postContent = "{ \"field1\" : \"" + temperature + "\", ";
            postContent += "\"field2\" : \"" + relativeHumidity + "\" }";
            postContent += "\n\r";
            postContent += "\n\r";

            POSTContent content = POSTContent.CreateTextBasedContent(postContent);
            HttpRequest request = WebHelper.CreatePostRequest(content, "/Measurements");
            return request;
        }


    }
}
