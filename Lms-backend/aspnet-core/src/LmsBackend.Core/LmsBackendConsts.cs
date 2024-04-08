using LmsBackend.Debugging;

namespace LmsBackend
{
    public class LmsBackendConsts
    {
        public const string LocalizationSourceName = "LmsBackend";

        public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = true;


        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public static readonly string DefaultPassPhrase =
            DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "14cf8585433240b0aa5f503fbed8b624";
    }
}
