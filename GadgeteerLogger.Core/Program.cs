using Microsoft.SPOT;
using Tinamous.GadgeteerLogger.Core.Components;

namespace Tinamous.GadgeteerLogger.Core
{
    public partial class Program
    {
        private RfidMonitor _rfidMonitor;
        private TemperatureReader _temperatureReader;
        private LoggerDisplay _loggerDisplay;
        private StatusMessageMonitor _statusMessageMonitor;
        private ButtonMonitor _buttonMonitor;
        private NetworkConnection _networkConnection;

        void ProgramStarted()
        {
            Debug.Print("Program Started");

            _loggerDisplay = new LoggerDisplay(display_T35);
            //_rfidMonitor = new RfidMonitor(rfid, _loggerDisplay);
            _temperatureReader = new TemperatureReader(temperatureHumidity, _loggerDisplay);
            //_statusMessageMonitor = new StatusMessageMonitor(relay_X1, _loggerDisplay);
            //_buttonMonitor = new ButtonMonitor(button);
            _networkConnection = new NetworkConnection(ethernet_J11D, _loggerDisplay);
            _networkConnection.NetworkUp+=NetworkConnectionUp;
            _networkConnection.NetworkDown+=NetworkConnectionDown;
            _networkConnection.Initialize();
        }

        private void NetworkConnectionUp(object sender, EventArgs e)
        {
            //_statusMessageMonitor.Start();
            _temperatureReader.Start();
        }

        private void NetworkConnectionDown(object sender, EventArgs e)
        {
            //_statusMessageMonitor.Stop();
            _temperatureReader.Stop();
        }
    }
}
