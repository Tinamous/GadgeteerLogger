using Microsoft.SPOT;
using Tinamous.GadgeteerLogger.Core.Components;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;

namespace Tinamous.GadgeteerLogger.Core
{
    public partial class Program
    {
        private RfidMonitor _rfidMonitor;
        private TemperatureReader _temperatureReader;
        private LoggerDisplay _loggerDisplay;
        private StatusMessageMonitor _statusMessageMonitor;
        private ButtonMonitor _buttonMonitor;

        void ProgramStarted()
        {
            Debug.Print("Program Started");

            _loggerDisplay = new LoggerDisplay(display_T35);
            _rfidMonitor = new RfidMonitor(button, rfid, _loggerDisplay);
            _temperatureReader = new TemperatureReader(temperatureHumidity, _loggerDisplay);
            _statusMessageMonitor = new StatusMessageMonitor(relay_X1, _loggerDisplay);
            _buttonMonitor = new ButtonMonitor(button);
           
            ethernet_J11D.NetworkUp += ethernet_J11D_NetworkUp;
            ethernet_J11D.NetworkDown += ethernet_J11D_NetworkDown;
            ethernet_J11D.UseDHCP();
            //ethernet_J11D.UseStaticIP("10.1.0.2", "255.255.128.0", "10.1.0.1");
            ethernet_J11D.UseThisNetworkInterface();
        }

        void ethernet_J11D_NetworkDown(GTM.Module.NetworkModule sender, GTM.Module.NetworkModule.NetworkState state)
        {
            Debug.Print("Network down");
            _loggerDisplay.ShowMessage("Network Down", 10, 50);
            _statusMessageMonitor.Stop();
            _temperatureReader.Stop();    
        }

        void ethernet_J11D_NetworkUp(GTM.Module.NetworkModule sender, GTM.Module.NetworkModule.NetworkState state)
        {
            Debug.Print("Network up");
            _loggerDisplay.ShowMessage("Network Up" + ethernet_J11D.Interface.NetworkInterface.IPAddress, 10, 50);
            _statusMessageMonitor.Start();
            _temperatureReader.Start();
        }
    }
}
