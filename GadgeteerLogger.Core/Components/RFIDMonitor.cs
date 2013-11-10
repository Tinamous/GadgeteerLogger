using Gadgeteer.Modules.GHIElectronics;
using Tinamous.GadgeteerLogger.Core.Web;

namespace Tinamous.GadgeteerLogger.Core.Components
{
    /// <summary>
    /// Monitor the Rfid reader.
    /// </summary>
    class RfidMonitor
    {
        private const int CardDetailsYPosition = 110;
        private readonly RFID _rfid;
        private readonly ILoggerDisplay _display;

        public RfidMonitor(RFID rfid, ILoggerDisplay display)
        {
            _rfid = rfid;
            _display = display;

            _rfid.CardIDReceived += RfidCardIdReceived;
        }

        void RfidCardIdReceived(RFID sender, string id)
        {
            _display.ShowMessage("Card:" + id, 10, CardDetailsYPosition);
            Status.PostStatus("RFID:" + id);
        }
    }
}
