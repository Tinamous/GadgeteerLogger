using System;
using Gadgeteer.Modules.Seeed;
using Gadgeteer.Networking;
using Microsoft.SPOT;
using Tinamous.GadgeteerLogger.Core.Web;

namespace Tinamous.GadgeteerLogger.Core.Components
{
    public class TemperatureReader
    {
        private bool _updating;
        private readonly TemperatureHumidity _temperatureHumidity;
        private readonly ILoggerDisplay _loggerDisplay;
        private Gadgeteer.Timer _temperatureCheckTimer;
        private const int TemperatureCheckInterval = 10000;

        public TemperatureReader(TemperatureHumidity temperatureHumidity, ILoggerDisplay loggerDisplay)
        {
            _temperatureHumidity = temperatureHumidity;
            _loggerDisplay = loggerDisplay;
            // Post an update on temp/humidity etc every x seconds
            _temperatureCheckTimer = new Gadgeteer.Timer(TemperatureCheckInterval);
            _temperatureCheckTimer.Tick += TemperatureCheckTimerTick;

            temperatureHumidity.MeasurementComplete += temperatureHumidity_MeasurementComplete;

        }

        void TemperatureCheckTimerTick(Gadgeteer.Timer timer)
        {
            if (_updating)
            {
                return;
            }

            try
            {
                _updating = true;

                _loggerDisplay.ShowMessage("Read/Post Temperature & Humidity", 10, 130);
                _temperatureHumidity.RequestMeasurement();
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


        void temperatureHumidity_MeasurementComplete(TemperatureHumidity sender, double temperature, double relativeHumidity)
        {
            string tempString = "Temperature: " + System.Math.Round(temperature) + " C";
            _loggerDisplay.ShowMessage(tempString, 10, 70);

            string humidityString = "Humidity: " + System.Math.Round(relativeHumidity) + " %";
            _loggerDisplay.ShowMessage(humidityString, 10, 90);

            PostMeasurements(temperature, relativeHumidity);
        }

        private void PostMeasurements(double temperature, double relativeHumidity)
        {
            Debug.Print("Sending measurements");
            var request = Measurements.CreatePostRequest(temperature, relativeHumidity);
            request.ResponseReceived += MeasurementPostCompleted;
            request.SendRequest();
        }

        private void MeasurementPostCompleted(HttpRequest sender, HttpResponse response)
        {
            if (response.StatusCode == "201")
            {
                _loggerDisplay.ShowMessage("", 10, 130);
            }
            else
            {
                _loggerDisplay.ShowMessage("Error: " + response.StatusCode, 10, 130);
            }
            Debug.Print("Post Measurements: ");
            Debug.Print(response.StatusCode);
            Debug.Print(response.Text);
        }

        public void Stop()
        {
            _temperatureCheckTimer.Stop();
        }

        public void Start()
        {
            _temperatureCheckTimer.Start();
        }
    }
}