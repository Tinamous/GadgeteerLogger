using Gadgeteer.Modules.GHIElectronics;
using Tinamous.GadgeteerLogger.Core.Web;

namespace Tinamous.GadgeteerLogger.Core.Components
{
    class ButtonMonitor
    {
        private readonly Button _button;

        public ButtonMonitor(Button button)
        {
            _button = button;
            button.LEDMode = Button.LEDModes.Off;
            button.ButtonReleased += button_ButtonReleased;
        }

        void button_ButtonReleased(Button sender, Button.ButtonState state)
        {
            _button.TurnLEDOn();
            Status.PostStatus("Button Up...");
            _button.TurnLEDOff();
        }
    }
}
