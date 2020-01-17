namespace Avalara.ApiClient.Authorization.OAuth
{
    public class IdServerSettings
    {
        public string IdServerUrl { get; set; }

        /// <summary>
        /// Client name.
        /// </summary>
        public string IdServerClientId {get; set; }

        /// <summary>
        /// Client secret.
        /// </summary>
        public string IdServerClientSecret { get; set; }

        /// <summary>
        /// Scope to request (if any).
        /// </summary>
        public string Scope { get; set; }
        /// <summary>
        ///  Specifies if HTTPS is enforced on idserver endpoints.
        /// </summary>
        public bool RequireHttps { get; set; }
    }
}