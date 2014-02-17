using Microsoft.SPOT;
using Tinamous.GadgeteerLogger.Core.Components;

namespace Tinamous.GadgeteerLogger.Core
{
    public partial class Program
    {
        private TemperatureReader _temperatureReader;
        private LoggerDisplay _loggerDisplay;

        private RfidMonitor _rfidMonitor;
        private StatusMessageMonitor _statusMessageMonitor;
        private ButtonMonitor _buttonMonitor;
   
        private NetworkConnection _networkConnection;
        private Monitor _monitor;

        void ProgramStarted()
        {
            Debug.Print("Program Started");

            _loggerDisplay = new LoggerDisplay(display_T35);
            _temperatureReader = new TemperatureReader(temperatureHumidity, lightSensor, _loggerDisplay);
            _temperatureReader.Start();

            //_rfidMonitor = new RfidMonitor(rfid, _loggerDisplay);
            //_statusMessageMonitor = new StatusMessageMonitor(relay_X1, _loggerDisplay);
            //_buttonMonitor = new ButtonMonitor(button);

            _networkConnection = new NetworkConnection(ethernet_J11D, _loggerDisplay);
            _networkConnection.NetworkUp+=NetworkConnectionUp;
            _networkConnection.NetworkDown+=NetworkConnectionDown;
            _networkConnection.Initialize();

            _monitor = new Monitor(_temperatureReader, _loggerDisplay);
        }

        private void NetworkConnectionUp(object sender, EventArgs e)
        {
            //_statusMessageMonitor.Start();
            _monitor.Start();
        }

        private void NetworkConnectionDown(object sender, EventArgs e)
        {
            //_statusMessageMonitor.Stop();
            _monitor.Stop();
        }
    }
}
