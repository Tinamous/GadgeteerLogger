using System;
using System.Text;
using Gadgeteer.Networking;

namespace Tinamous.GadgeteerLogger.Core
{
    public static class WebHelper
    {
        /// <summary>
        /// The Tinamous api Url for your account.
        /// Replace ddd with your account name
        /// </summary>
        private const string ApiBaseUrl = "http://ddd.Tinamous.com/api/v1";
        
        /// <summary>
        /// Username for this device in your Tinamous account
        /// </summary>
        private const string UserName = "Spider";

        /// <summary>
        /// Passwordfor the device
        /// </summary>
        private const string Password = "Passw0rd1234";

        public static HttpRequest CreateGetRequest(string url)
        {
            return HttpHelper.CreateHttpGetRequest(ApiBaseUrl + url);
        }

        public static HttpRequest CreatePostRequest(POSTContent content, string controller)
        {
            var postRequest = HttpHelper.CreateHttpPostRequest(ApiBaseUrl + controller, content, "application/json");
            AddAuthorizationHeaders(postRequest);
            return postRequest;
        }

        public static void AddAuthorizationHeaders(HttpRequest getRequest)
        {
            var auth = GetAuthHeaderValue(UserName, Password);
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
