using System;
using System.Collections;
using Gadgeteer.Modules.GHIElectronics;
using Gadgeteer.Networking;
using Microsoft.SPOT;
using Tinamous.GadgeteerLogger.Core.Dtos;
using Tinamous.GadgeteerLogger.Core.Web;

namespace Tinamous.GadgeteerLogger.Core.Components
{
    class StatusMessageMonitor
    {
        private const int StatusCheckInterval = 60000;
        private Gadgeteer.Timer _statusCheckTimer;
        private readonly Relay_X1 _relayX1;
        private readonly ILoggerDisplay _loggerDisplay;
        private bool _updating;
        private DateTime _lastPost = new DateTime(2013, 10, 12, 00, 00, 00);


        public StatusMessageMonitor(Relay_X1 relayX1, ILoggerDisplay loggerDisplay)
        {
            _relayX1 = relayX1;
            _loggerDisplay = loggerDisplay;

            // Check status messages every x seconds
            _statusCheckTimer = new Gadgeteer.Timer(StatusCheckInterval);
            _statusCheckTimer.Tick += _statusCheckTimer_Tick;

        }

        void _statusCheckTimer_Tick(Gadgeteer.Timer timer)
        {
            if (_updating)
            {
                return;
            }

            try
            {
                _updating = true;

                _loggerDisplay.ShowMessage("Reading Status Posts", 10, 150);

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
            _loggerDisplay.ShowMessage("", 10, 150);

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
                _loggerDisplay.ShowMessage("@" + post.User.UserName + ":", 10, 190);
                _loggerDisplay.ShowMessage(post.SummaryMessage, 10, 210);
                if (IsPostForMe(post))
                {
                    CheckCommands(post);
                }
            }
        }

        private bool IsPostForMe(StatusPost post)
        {
            // Ignore posts not to this device
            if (post.Message.ToLower().IndexOf("@spider") == 0)
            {
                return true;
            }
            return false;
        }

        private bool AmIMentioned(StatusPost post)
        {
            if (post.Message.ToLower().IndexOf("@spider") >= 0)
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
