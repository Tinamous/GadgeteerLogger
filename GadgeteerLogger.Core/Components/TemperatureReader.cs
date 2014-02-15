using System;
using Gadgeteer.Modules.Seeed;
using Gadgeteer.Networking;
using Microsoft.SPOT;
using Tinamous.GadgeteerLogger.Core.Web;

namespace Tinamous.GadgeteerLogger.Core.Components
{
    /// <summary>
    /// Read temperature and humidity and post the measurements to Tinamous
    /// </summary>
    public class TemperatureReader
    {
        private const int TemperatureCheckInterval = 10000;
        private const int TemperatureYPostion = 70;
        private const int HumidityYPosition = 90;
        private const int StatusYPosition = 130;

        private readonly TemperatureHumidity _temperatureHumidity;
        private readonly ILoggerDisplay _loggerDisplay;
        private readonly Gadgeteer.Timer _temperatureCheckTimer;
        private bool _updating;

        public TemperatureReader(TemperatureHumidity temperatureHumidity, ILoggerDisplay loggerDisplay)
        {
            _temperatureHumidity = temperatureHumidity;
            _loggerDisplay = loggerDisplay;

            // Post an update on temp/humidity etc every x seconds
            _temperatureCheckTimer = new Gadgeteer.Timer(TemperatureCheckInterval);
            _temperatureCheckTimer.Tick += TemperatureCheckTimerTick;

            temperatureHumidity.MeasurementComplete += TemperatureHumidityMeasurementComplete;
        }

        public void Start()
        {
            _temperatureCheckTimer.Start();
        }

        public void Stop()
        {
            _temperatureCheckTimer.Stop();
        }

        void TemperatureCheckTimerTick(Gadgeteer.Timer timer)
        {
            if (_updating)
            {
                _loggerDisplay.ShowErrorMessage("Waiting for previous update to finish", 10, StatusYPosition);
                return;
            }

            try
            {
                _updating = true;

                _loggerDisplay.ShowMessage("Read Temperature & Humidity", 10, StatusYPosition);
                _temperatureHumidity.RequestMeasurement();
            }
            catch (Exception ex)
            {
                Debug.Print("Exception updating:" + ex);
                _loggerDisplay.ShowErrorMessage("Exception requesting measurement", 10, StatusYPosition);
            }
            finally
            {
                _updating = false;
            }
        }


        void TemperatureHumidityMeasurementComplete(TemperatureHumidity sender, double temperature, double relativeHumidity)
        {
            string tempString = "Temperature: " + temperature.ToString("f2") + " °C";
            _loggerDisplay.ShowMessage(tempString, 10, TemperatureYPostion);

            string humidityString = "Humidity: " + relativeHumidity.ToString("f2") + " %";
            _loggerDisplay.ShowMessage(humidityString, 10, HumidityYPosition);

            PostMeasurements(temperature, relativeHumidity);
        }
        
        private void PostMeasurements(double temperature, double relativeHumidity)
        {
            Debug.Print("Sending measurements");

            _loggerDisplay.ShowMessage("Sending Temperature & Humidity", 10, StatusYPosition);
            var request = Measurements.CreatePostRequest(temperature.ToString("f2"), relativeHumidity.ToString("f2"),"1");
            request.ResponseReceived += MeasurementPostCompleted;
            request.SendRequest();
        }

        private void MeasurementPostCompleted(HttpRequest sender, HttpResponse response)
        {
            if (response.StatusCode == "201")
            {
                _loggerDisplay.ShowMessage("", 10, StatusYPosition);
            }
            else
            {
                _loggerDisplay.ShowErrorMessage("Error: " + response.StatusCode, 10, StatusYPosition);
            }

            Debug.Print("Post Measurements: ");
            Debug.Print(response.StatusCode);
            //Debug.Print(response.Text);
        }
    }
}