using System;
using System.Collections;
using Json.NETMF;

namespace Tinamous.GadgeteerLogger.Core.Dtos
{
    /// <summary>
    /// Represents a status post read from Tinamous
    /// </summary>
    class StatusPost
    {
        public StatusPost(Hashtable statusPost)
        {
            Message = statusPost["Message"].ToString();
            SummaryMessage = statusPost["SummaryMessage"].ToString();
            Id = Convert.ToInt64(statusPost["Id"].ToString());
            PostedOn = DateTimeExtensions.FromIso8601(statusPost["PostedOn"] as string);
            User = new User(statusPost["User"] as Hashtable);
        }

        public string Message { get; set; }
        public string SummaryMessage { get; set; }
        public long Id { get; set; }
        public DateTime PostedOn { get; set; }
        public User User { get; set; }

    }

    internal class User
    {
        public User(Hashtable hashtable)
        {
            //Account = Convert.ToInt64(hashtable["Account"].ToString());
            Id = Convert.ToInt64(hashtable["Id"].ToString());
            //Name = hashtable["Name"].ToString();
            UserName = hashtable["UserName"].ToString();
            FullUserName = hashtable["FullUserName"].ToString();
            //DisplayName = hashtable["DisplayName"].ToString();
            //TimeLine = hashtable["TimeLine"].ToString();
        }

        public long Account { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string FullUserName { get; set; }
        public string DisplayName { get; set; }
        public string TimeLine { get; set; }
    }
}
