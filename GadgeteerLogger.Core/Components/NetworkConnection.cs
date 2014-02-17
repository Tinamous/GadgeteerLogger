using Gadgeteer.Modules;
using Gadgeteer.Modules.GHIElectronics;
using Microsoft.SPOT;

namespace Tinamous.GadgeteerLogger.Core.Components
{
    /// <summary>
    /// Handle management of the network connection
    /// </summary>
    public class NetworkConnection
    {
        private readonly Ethernet_J11D _ethernetJ11D;
        private readonly LoggerDisplay _loggerDisplay;
        private const int NetworkStatusYPosition = 10;

        public NetworkConnection(Ethernet_J11D ethernetJ11D, LoggerDisplay loggerDisplay)
        {
            _ethernetJ11D = ethernetJ11D;
            _loggerDisplay = loggerDisplay;
        }

        public event EventHandler NetworkUp;
        public event EventHandler NetworkDown;

        protected virtual void OnNetworkUp(EventArgs e)
        {
            EventHandler handler = NetworkUp;
            if (handler != null) handler(this, e);
        }

        protected virtual void OnNetworkDown(EventArgs e)
        {
            EventHandler handler = NetworkDown;
            if (handler != null) handler(this, e);
        }

        public void Initialize()
        {
            Debug.Print("Setting up network interface");
            _ethernetJ11D.UseThisNetworkInterface();
            _ethernetJ11D.NetworkUp += EthernetJ11DNetworkUp;
            _ethernetJ11D.NetworkDown += EthernetJ11DNetworkDown;
            _ethernetJ11D.Interface.NetworkAddressChanged += InterfaceNetworkAddressChanged;
            _ethernetJ11D.Interface.CableConnectivityChanged += InterfaceCableConnectivityChanged;

            _ethernetJ11D.UseDHCP();
            // If you need to use Static IP Assignment set it here.
            //ethernet_J11D.UseStaticIP("10.1.0.2", "255.255.128.0", "10.1.0.1");
        }

        void InterfaceCableConnectivityChanged(object sender, GHI.Premium.Net.EthernetBuiltIn.CableConnectivityEventArgs e)
        {
            // If it has been reconnected then 
            if (e.IsConnected)
            {
                _loggerDisplay.ShowMessage("Network cable connected", 10, NetworkStatusYPosition);
                _ethernetJ11D.Interface.Open();
            }
            else
            {
                _loggerDisplay.ShowErrorMessage("Network cable disconnected", 10, NetworkStatusYPosition);
                _ethernetJ11D.Interface.Close();
            }
        }

        void InterfaceNetworkAddressChanged(object sender, EventArgs e)
        {
            Debug.Print("Network IP Address changed: " + _ethernetJ11D.Interface.NetworkInterface.IPAddress);
            string message = "Network Up. IP: " + _ethernetJ11D.Interface.NetworkInterface.IPAddress;
            _loggerDisplay.ShowMessage(message, 10, NetworkStatusYPosition);
        }

        void EthernetJ11DNetworkUp(Module.NetworkModule sender, Module.NetworkModule.NetworkState state)
        {
            string message = "Network up. IP: " + _ethernetJ11D.Interface.NetworkInterface.IPAddress;
            Debug.Print(message);
            _loggerDisplay.ShowMessage(message, 10, NetworkStatusYPosition);
            OnNetworkUp(EventArgs.Empty);
            
        }

        void EthernetJ11DNetworkDown(Module.NetworkModule sender, Module.NetworkModule.NetworkState state)
        {
            Debug.Print("Network down");
            _loggerDisplay.ShowMessage("Network Down :-(", 10, NetworkStatusYPosition);
            OnNetworkDown(EventArgs.Empty);
        }
    }
}