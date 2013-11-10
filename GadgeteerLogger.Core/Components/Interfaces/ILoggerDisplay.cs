namespace Tinamous.GadgeteerLogger.Core.Components
{
    public interface ILoggerDisplay
    {
        void ShowMessage(string message, uint x, uint y);
        void ShowErrorMessage(string message, uint x, uint y);
    }
}