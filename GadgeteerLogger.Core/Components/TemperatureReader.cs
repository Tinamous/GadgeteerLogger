using System;
using Gadgeteer.Modules.GHIElectronics;
using Gadgeteer.Modules.Seeed;
using Microsoft.SPOT;
using Tinamous.GadgeteerLogger.Core.Components.EventArguments;
using Tinamous.GadgeteerLogger.Core.Components.Interfaces;

namespace Tinamous.GadgeteerLogger.Core.Components
{
    /// <summary>
    /// Read temperature and humidity and updates the display. Raises event on new temperature measurement
    /// </summary>
    public class TemperatureReader
    {
        public delegate void LightLevelMeasurementCompleteEvent(object sender, LightLevelMeasurementCompleteEventArgs args);
        public event TemperatureHumidity.MeasurementCompleteEventHandler MeasurementComplete;
        public event LightLevelMeasurementCompleteEvent LightLevelMeasurementComplete;

        private const int TemperatureYPostion = 30;
        private const int HumidityYPosition = 45;
        private const int LightYPostion = 60;
        private const int StatusYPosition = 75;

        // Temp module pauses about 3.6 seconds to prevent overheating.
        // poll less often to allow to run cooler?
        private const int TemperatureCheckInterval = 10000;

        private readonly TemperatureHumidity _temperatureHumidity;
        private readonly LightSensor _lightSensor;
        private readonly ILoggerDisplay _loggerDisplay;
        private readonly Gadgeteer.Timer _temperatureCheckTimer = new Gadgeteer.Timer(TemperatureCheckInterval);
        private bool _updating;

        public TemperatureReader(TemperatureHumidity temperatureHumidity, LightSensor lightSensor, ILoggerDisplay loggerDisplay)
        {
            if (temperatureHumidity == null) throw new ArgumentNullException("temperatureHumidity");
            if (lightSensor == null) throw new ArgumentNullException("lightSensor");
            if (loggerDisplay == null) throw new ArgumentNullException("loggerDisplay");

            _temperatureHumidity = temperatureHumidity;
            _lightSensor = lightSensor;
            _temperatureHumidity.MeasurementComplete += TemperatureHumidityMeasurementComplete;

            _loggerDisplay = loggerDisplay;

            // Take measurement on timer tick
            // and hence update UI and Monitor.
            _temperatureCheckTimer.Tick += TemperatureCheckTimerTick;
        }

        public void Start()
        {
            _temperatureCheckTimer.Start();
        }

        public void Stop()
        {
            _temperatureCheckTimer.Stop();
            _temperatureHumidity.StopContinuousMeasurements();
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

                _temperatureHumidity.RequestMeasurement();
                GetLightLevel();

                // Clear any previous message
                _loggerDisplay.ClearMessage(10, StatusYPosition);
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

        private void GetLightLevel()
        {
            double lightLevelPercentage = _lightSensor.ReadLightSensorPercentage();
            double lightLevelVoltage= _lightSensor.ReadLightSensorVoltage();
            
            string lightString = "Light: " + lightLevelPercentage.ToString("f2") + " %";
            _loggerDisplay.ShowMessage(lightString, 10, LightYPostion);

            if (LightLevelMeasurementComplete != null)
            {
                var args = new LightLevelMeasurementCompleteEventArgs(lightLevelPercentage, lightLevelVoltage);
                LightLevelMeasurementComplete(this, args);

            }
        }

        void TemperatureHumidityMeasurementComplete(TemperatureHumidity sender, double temperature, double relativeHumidity)
        {
            string tempString = "Temperature: " + temperature.ToString("f2") + " °C";
            _loggerDisplay.ShowMessage(tempString, 10, TemperatureYPostion);

            string humidityString = "Humidity: " + relativeHumidity.ToString("f2") + " %";
            _loggerDisplay.ShowMessage(humidityString, 10, HumidityYPosition);

            // Bubble the same event on up.
            if (MeasurementComplete != null)
            {
                MeasurementComplete(sender, temperature, relativeHumidity);
            }
        }
    }
}