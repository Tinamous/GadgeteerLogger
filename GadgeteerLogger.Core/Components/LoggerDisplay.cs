using Gadgeteer.Modules.GHIElectronics;
using Microsoft.SPOT;

namespace Tinamous.GadgeteerLogger.Core.Components
{
    public class LoggerDisplay : ILoggerDisplay
    {
        private Font _font;
        private Gadgeteer.Color _color;
        private readonly Display_T35 _display;

        public LoggerDisplay(Display_T35 display)
        {
            _display = display;
            Initialize();
        }

        private void Initialize()
        {
            _font = Resources.GetFont(Resources.FontResources.NinaB);
            _color = Gadgeteer.Color.Green;
            _display.SimpleGraphics.AutoRedraw = true;
            _display.SimpleGraphics.Clear();
        }

        public void ShowMessage(string message, uint x, uint y)
        {
            _display.SimpleGraphics.DisplayRectangle(Gadgeteer.Color.Black, 1, Gadgeteer.Color.Black, x, y, 300, 15);
            _display.SimpleGraphics.DisplayText(message, _font, _color, x, y);
        }
    }
}