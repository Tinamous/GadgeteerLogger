namespace Tinamous.GadgeteerLogger.Core.Components.Interfaces
{
    public interface ILoggerDisplay
    {
        void ShowMessage(string message, uint x, uint y);
        void ClearMessage(uint x, uint y);
        void ShowErrorMessage(string message, uint x, uint y);
    }
}