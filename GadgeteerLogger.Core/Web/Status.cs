using System;
using Gadgeteer.Networking;
using Microsoft.SPOT;

namespace Tinamous.GadgeteerLogger.Core.Web
{
    static class Status
    {
        public static void PostStatus(string statusMessage)
        {
            string postContent = "{ \"Message\" : \"" + statusMessage + "\"}";
            postContent += "\n\r";
            postContent += "\n\r";

            POSTContent content = POSTContent.CreateTextBasedContent(postContent);
            var request = WebHelper.CreatePostRequest(content, "/Status");
            request.SendRequest();
        }

        public static HttpRequest CreateGetRequest(DateTime lastPost)
        {
            // Get the posts after the last
            var startDate = FormatDateForUrl(lastPost.AddSeconds(1));
            Debug.Print("Requesting status posts from: " + startDate);
            HttpRequest getRequest = WebHelper.CreateGetRequest("/Status" + "?startDate=" + startDate);
            WebHelper.AddAuthorizationHeaders(getRequest);
            return getRequest;
        }

        private static string FormatDateForUrl(DateTime lastPost)
        {
            string startDate = lastPost.ToString("yyyy-MM-dd");
            startDate += "T"; // or T
            string time = lastPost.ToString("HH:mm:ss");

            string[] times = time.Split(':');
            startDate += times[0];
            startDate += "%3a";
            startDate += times[1];
            startDate += "%3a";
            startDate += times[2];

            return startDate;
        }
    }
}
