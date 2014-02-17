//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34003
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Tinamous.GadgeteerLogger.Core {
    using Gadgeteer;
    using GTM = Gadgeteer.Modules;
    
    
    public partial class Program : Gadgeteer.Program {
        
        /// <summary>The Display_T35 module using sockets 14, 13, 12 and 10 of the mainboard.</summary>
        private Gadgeteer.Modules.GHIElectronics.Display_T35 display_T35;
        
        /// <summary>The UsbClientSP module using socket 1 of the mainboard.</summary>
        private Gadgeteer.Modules.GHIElectronics.UsbClientSP usbClientSP;
        
        /// <summary>The Ethernet_J11D (Premium) module using socket 7 of the mainboard.</summary>
        private Gadgeteer.Modules.GHIElectronics.Ethernet_J11D ethernet_J11D;
        
        /// <summary>The Button module using socket 4 of the mainboard.</summary>
        private Gadgeteer.Modules.GHIElectronics.Button button;
        
        /// <summary>The TemperatureHumidity module using socket 8 of the mainboard.</summary>
        private Gadgeteer.Modules.Seeed.TemperatureHumidity temperatureHumidity;
        
        /// <summary>The RFID module using socket 11 of the mainboard.</summary>
        private Gadgeteer.Modules.GHIElectronics.RFID rfid;
        
        /// <summary>The Relay X1 module using socket 5 of the mainboard.</summary>
        private Gadgeteer.Modules.GHIElectronics.Relay_X1 relay_X1;
        
        /// <summary>The LightSensor module using socket 9 of the mainboard.</summary>
        private Gadgeteer.Modules.GHIElectronics.LightSensor lightSensor;
        
        /// <summary>This property provides access to the Mainboard API. This is normally not necessary for an end user program.</summary>
        protected new static GHIElectronics.Gadgeteer.FEZSpider Mainboard {
            get {
                return ((GHIElectronics.Gadgeteer.FEZSpider)(Gadgeteer.Program.Mainboard));
            }
            set {
                Gadgeteer.Program.Mainboard = value;
            }
        }
        
        /// <summary>This method runs automatically when the device is powered, and calls ProgramStarted.</summary>
        public static void Main() {
            // Important to initialize the Mainboard first
            Program.Mainboard = new GHIElectronics.Gadgeteer.FEZSpider();
            Program p = new Program();
            p.InitializeModules();
            p.ProgramStarted();
            // Starts Dispatcher
            p.Run();
        }
        
        private void InitializeModules() {
            this.display_T35 = new GTM.GHIElectronics.Display_T35(14, 13, 12, 10);
            this.usbClientSP = new GTM.GHIElectronics.UsbClientSP(1);
            this.ethernet_J11D = new GTM.GHIElectronics.Ethernet_J11D(7);
            this.button = new GTM.GHIElectronics.Button(4);
            this.temperatureHumidity = new GTM.Seeed.TemperatureHumidity(8);
            this.rfid = new GTM.GHIElectronics.RFID(11);
            this.relay_X1 = new GTM.GHIElectronics.Relay_X1(5);
            this.lightSensor = new GTM.GHIElectronics.LightSensor(9);
        }
    }
}
