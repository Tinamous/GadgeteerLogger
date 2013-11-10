namespace Tinamous.GadgeteerLogger.Core
{
    /// <summary>
    /// Global constants
    /// </summary>
    static class Globals
    {
        /// <summary>
        /// Account name (e.g. ddd.Tinaous.com is ddd)
        /// Replace ddd with your own account name as set-up when registring
        /// </summary>
        private const string AccountName = "ddd";

        /// <summary>
        /// The Tinamous api Url for your account.
        /// </summary>
        public const string ApiBaseUrl = "http://" + AccountName + ".Tinamous.com/api/v1";

        /// <summary>
        /// Username for this device in your Tinamous account
        /// </summary>
        /// <remarks>
        /// Replace this with the username for your device.
        /// </remarks>
        public const string UserName = "Spider";

        /// <summary>
        /// Password for the device.
        /// </summary>
        /// <remarks>
        /// Replace this with the password you assigned to the device
        /// </remarks>
        public const string Password = "Passw0rd1234";
    }
}
