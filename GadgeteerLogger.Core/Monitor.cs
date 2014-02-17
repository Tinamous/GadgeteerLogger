using System;
using Gadgeteer;
using Gadgeteer.Modules.Seeed;
using Gadgeteer.Networking;
using Microsoft.SPOT;
using Tinamous.GadgeteerLogger.Core.Components;
using Tinamous.GadgeteerLogger.Core.Components.Interfaces;
using Tinamous.GadgeteerLogger.Core.Web;

namespace Tinamous.GadgeteerLogger.Core
{
    /// <summary>
    /// Responsible for monitoring sensors and periodically sending to Tinamous.
    /// </summary>
    public class Monitor
    {
        private const int StatusYPosition = 130;
        private readonly TemperatureReader _temperatureReader;
        private readonly ILoggerDisplay _loggerDisplay;
        private Timer _timer;
        private bool _hasTemperature;
        private bool _hasLightLevel;
        private bool _hasPressure;

        public Monitor(TemperatureReader temperatureReader, ILoggerDisplay loggerDisplay)
        {
            _temperatureReader = temperatureReader;
            _temperatureReader.MeasurementComplete += TemperatureReaderMeasurementComplete;
            _temperatureReader.LightLevelMeasurementComplete += _temperatureReader_LightLevelMeasurementComplete;
            _loggerDisplay = loggerDisplay;
            _timer = new Timer(new TimeSpan(0, 0, 1, 0));
            _timer.Tick += _timer_Tick;

            Pressure = "2";
            _hasPressure = true;
        }

        void _temperatureReader_LightLevelMeasurementComplete(object sender, Components.EventArguments.LightLevelMeasurementCompleteEventArgs args)
        {
            LightLevel = args.LightLevelPercentage.ToString("f1");
            Debug.Print("Light reader set light as: " + Temperature);
            _hasLightLevel = true;
        }

        void TemperatureReaderMeasurementComplete(TemperatureHumidity sender, double temperature, double relativeHumidity)
        {
            Temperature = temperature.ToString("f2");
            Debug.Print("Temperature reader set temp as: " + Temperature);
            Humidity = relativeHumidity.ToString("f0");
            _hasTemperature = true;
        }

        public void Start()
        {
            Debug.Print("Tinamous monitor started.");
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        public string Temperature { get; set; }
        public string Humidity { get; set; }
        public string LightLevel { get; set; }
        public string Pressure { get; set; }

        void _timer_Tick(Timer timer)
        {
            // Only post when we have a measurement from ALL of the sensors
            // to prevent inserting 0's into the results.
            if (HasAllMeasurements())
            {
                _loggerDisplay.ShowMessage("Sending to Tinamous...", 0, StatusYPosition);
                SendMeasurements();
            }
            else
            {
                Debug.Print("Not sending measurements as not all collected.");
            }
        }

        private bool HasAllMeasurements()
        {
            return _hasTemperature && _hasPressure && _hasLightLevel;
        }

        private void SendMeasurements()
        {
            Debug.Print("Sending measurements");
            var request = Measurements.CreatePostRequest(Temperature, Humidity, LightLevel, Pressure);
            request.ResponseReceived += MeasurementPostCompleted;
            request.SendRequest();
        }

        private void MeasurementPostCompleted(HttpRequest sender, HttpResponse response)
        {
            
            if (response.StatusCode == "201")
            {
                _loggerDisplay.ClearMessage(0, StatusYPosition);
                Debug.Print("Measurements send OK.");
            }
            else
            {
                _loggerDisplay.ShowErrorMessage("Error: " + response.StatusCode, 10, StatusYPosition);
                Debug.Print("HttpCode: " + response.StatusCode);
                Debug.Print("Error :" + response.Text);
            }
        }
    }
}