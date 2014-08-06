namespace WebApi.Hal
{
    /// <summary>
    /// Stores all ReservedProperties for HAL representation. Avoid magic strings
    /// </summary>
    public class ReservedProperties
    {
        /// <summary>
        /// Reserved strings for Links
        /// </summary>
        /// <remarks>See https://tools.ietf.org/html/draft-kelly-json-hal-06#section-8</remarks>
        public class Links
        {
            // https://tools.ietf.org/html/draft-kelly-json-hal-06#section-8.1
            public const string Self = "self";

            public const string Curies = "curies";

            public const string Curie = "curie";
        }
    }
}
