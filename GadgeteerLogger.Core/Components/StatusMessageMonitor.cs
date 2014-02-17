using System;
using System.Collections;
using Gadgeteer.Modules.GHIElectronics;
using Gadgeteer.Networking;
using Microsoft.SPOT;
using Tinamous.GadgeteerLogger.Core.Components.Interfaces;
using Tinamous.GadgeteerLogger.Core.Dtos;
using Tinamous.GadgeteerLogger.Core.Web;

namespace Tinamous.GadgeteerLogger.Core.Components
{
    /// <summary>
    /// Reads status messages from Tinamous and displayes them on the screen
    /// </summary>
    /// <remarks>
    /// If the status message is a command to the user (@spider) then it tries
    /// to process the command and perform an action based on it.
    /// </remarks>
    class StatusMessageMonitor
    {
        private const int StatusYPosition = 150;
        private const int ReadStatusYPosition = 190;
        private const int StatusCheckInterval = 30000;

        private Gadgeteer.Timer _statusCheckTimer;
        private readonly Relay_X1 _relayX1;
        private readonly ILoggerDisplay _loggerDisplay;
        private bool _updating;
        private DateTime _lastPost = new DateTime(2013, 10, 12, 00, 00, 00);
        private string _username;

        public StatusMessageMonitor(Relay_X1 relayX1, ILoggerDisplay loggerDisplay)
        {
            _relayX1 = relayX1;
            _loggerDisplay = loggerDisplay;
            _username = "@" + Globals.UserName.ToLower();

            // Check status messages every x seconds
            _statusCheckTimer = new Gadgeteer.Timer(StatusCheckInterval);
            _statusCheckTimer.Tick += StatusCheckTimerTick;
        }

        void StatusCheckTimerTick(Gadgeteer.Timer timer)
        {
            if (_updating)
            {
                return;
            }

            try
            {
                _updating = true;

                _loggerDisplay.ShowMessage("Reading Status Posts", 10, StatusYPosition);

                // Check for new status messages
                var request = Status.CreateGetRequest(_lastPost);
                request.ResponseReceived += StatusPostsReceived;
                request.SendRequest();

                // Check for Alerts.
            }
            catch (Exception ex)
            {
                Debug.Print("Exception updating:" + ex);
            }
            finally
            {
                _updating = false;
            }
        }

        void StatusPostsReceived(HttpRequest sender, HttpResponse response)
        {
            Debug.Print("Status Posts Recieved: ");
            Debug.Print(response.StatusCode);

            // Blank out the "Updating" message
            _loggerDisplay.ShowMessage("", 10, StatusYPosition);

            if (response.StatusCode == "200")
            {
                Debug.Print("OK");
                Debug.Print(response.Text);

                var statusPosts = Json.NETMF.JsonSerializer.DeserializeString(response.Text) as ArrayList;
                if (statusPosts != null)
                {
                    ProcessStatusMessages(statusPosts);
                }
            }
        }

        private void ProcessStatusMessages(ArrayList statusPosts)
        {
            foreach (var statusPost in statusPosts)
            {
                var post = new StatusPost((Hashtable)statusPost);
                ProcessStatusMessage(post);
                _lastPost = post.PostedOn;
            }
        }

        private void ProcessStatusMessage(StatusPost post)
        {
            // Show only posts I am mentioned in
            if (AmIMentioned(post))
            {
                _loggerDisplay.ShowMessage("@" + post.User.UserName + ":", 10, ReadStatusYPosition);
                _loggerDisplay.ShowMessage(post.SummaryMessage, 10, ReadStatusYPosition + 20);

                if (IsPostForMe(post))
                {
                    CheckCommands(post);
                }
            }
        }

        /// <summary>
        /// Determine if the status post is sent to us.
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        private bool IsPostForMe(StatusPost post)
        {
            // If the username is the start of the message then the post
            // is for us. otherwise ignore
            if (post.Message.ToLower().IndexOf(_username) == 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Determine if this device is mentioned in the status post.
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        private bool AmIMentioned(StatusPost post)
        {
            if (post.Message.ToLower().IndexOf(_username) >= 0)
            {
                return true;
            }
            return false;
        }

        private bool CheckCommands(StatusPost post)
        {
            // Check it's a command, should be 2 parts, who to and command:action
            // any more than 2 and it's probably not a command
            string[] userCommand = post.Message.Split(' ');
            if (userCommand.Length != 2)
            {
                return false;
            }

            return ProcessCommand(userCommand[1]);
        }

        private bool ProcessCommand(string command)
        {
            // Check it's a command and operation seperated by : 
            if (command.IndexOf(':') < 1)
            {
                return false;
            }
            string[] split = command.Split(':');
            if (split.Length < 2)
            {
                return false;
            }

            switch (split[0].ToLower())
            {
                case "relay":
                    ToggleRelay(split[1]);
                    break;
            }
            return true;
        }

        private void ToggleRelay(string option)
        {
            switch (option.ToLower())
            {
                case "on":
                    _relayX1.TurnOn();
                    break;
                case "off":
                    _relayX1.TurnOff();
                    break;
            }
        }

        public void Start()
        {
            _statusCheckTimer.Start();
        }

        public void Stop()
        {
            _statusCheckTimer.Stop();
        }
    }
}
