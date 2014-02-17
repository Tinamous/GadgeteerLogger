using Gadgeteer.Modules.GHIElectronics;
using Microsoft.SPOT;
using Tinamous.GadgeteerLogger.Core.Components.Interfaces;

namespace Tinamous.GadgeteerLogger.Core.Components
{
    /// <summary>
    /// Display interface class.
    /// </summary>
    public class LoggerDisplay : ILoggerDisplay
    {
        private Font _font;
        private readonly Gadgeteer.Color _color = Gadgeteer.Color.Green;
        private readonly Gadgeteer.Color _errorColor = Gadgeteer.Color.Red;
        private readonly Display_T35 _display;

        public LoggerDisplay(Display_T35 display)
        {
            _display = display;
            Initialize();
        }

        private void Initialize()
        {
            _font = Resources.GetFont(Resources.FontResources.NinaB);
            _display.SimpleGraphics.AutoRedraw = true;
            _display.SimpleGraphics.Clear();
        }

        public void ShowMessage(string message, uint x, uint y)
        {
            _display.SimpleGraphics.DisplayRectangle(Gadgeteer.Color.Black, 1, Gadgeteer.Color.Black, x, y, 300, 15);
            _display.SimpleGraphics.DisplayText(message, _font, _color, x, y);
        }

        public void ClearMessage(uint x, uint y)
        {
            _display.SimpleGraphics.DisplayRectangle(Gadgeteer.Color.Black, 1, Gadgeteer.Color.Black, x, y, 300, 15);
        }

        public void ShowErrorMessage(string message, uint x, uint y)
        {
            _display.SimpleGraphics.DisplayRectangle(Gadgeteer.Color.Black, 1, Gadgeteer.Color.Black, x, y, 300, 15);
            _display.SimpleGraphics.DisplayText(message, _font, _errorColor, x, y);
        }
    }
}