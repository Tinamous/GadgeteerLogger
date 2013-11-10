using System;
using System.Text;
using Gadgeteer.Networking;

namespace Tinamous.GadgeteerLogger.Core
{
    public static class WebHelper
    {
        public static HttpRequest CreateGetRequest(string url)
        {
            return HttpHelper.CreateHttpGetRequest(Globals.ApiBaseUrl + url);
        }

        public static HttpRequest CreatePostRequest(POSTContent content, string controller)
        {
            var postRequest = HttpHelper.CreateHttpPostRequest(Globals.ApiBaseUrl + controller, content, "application/json");
            AddAuthorizationHeaders(postRequest);
            return postRequest;
        }

        public static void AddAuthorizationHeaders(HttpRequest getRequest)
        {
            var auth = GetAuthHeaderValue(Globals.UserName, Globals.Password);
            getRequest.AddHeaderField("Authorization", auth);
        }

        private static string GetAuthHeaderValue(string user, string password)
        {
            string userPassword = user + ":" + password;
            byte[] bytes = Encoding.UTF8.GetBytes(userPassword);
            return "Basic " + Convert.ToBase64String(bytes);
        }
    }
}
