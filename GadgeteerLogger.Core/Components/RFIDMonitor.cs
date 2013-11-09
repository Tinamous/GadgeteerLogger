using Gadgeteer.Modules.GHIElectronics;
using Tinamous.GadgeteerLogger.Core.Web;

namespace Tinamous.GadgeteerLogger.Core.Components
{
    class RfidMonitor
    {
        private readonly Button _button;
        private readonly RFID _rfid;
        private readonly ILoggerDisplay _display;

        public RfidMonitor(Button button, RFID rfid, ILoggerDisplay display)
        {
            _button = button;
            _rfid = rfid;
            _display = display;
            Initialize();
        }

        private void Initialize()
        {
            _rfid.CardIDReceived += rfid_CardIDReceived;
        }

        void rfid_CardIDReceived(RFID sender, string id)
        {
            _button.TurnLEDOn();
            _display.ShowMessage("Card:" + id, 10, 110);
            Status.PostStatus("RFID:" + id);
            _button.TurnLEDOff();
        }
    }
}
