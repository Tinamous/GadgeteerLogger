using Microsoft.SPOT;

namespace Tinamous.GadgeteerLogger.Core.Components.EventArguments
{
    public class LightLevelMeasurementCompleteEventArgs : EventArgs
    {
        public double LightLevelPercentage { get; set; }
        public double LightLevelVoltage { get; set; }

        public LightLevelMeasurementCompleteEventArgs(double lightLevelPercentage, double lightLevelVoltage)
        {
            LightLevelPercentage = lightLevelPercentage;
            LightLevelVoltage = lightLevelVoltage;
        }
    }
}