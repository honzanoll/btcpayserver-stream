namespace BTCPayServer.Stream.Data.Enums
{
    public enum Culture
    {
        EN = 1,
        CZ = 2
    }

    public static class CultureExtensions
    {
        public static string ToISO(this Culture culture)
        {
            switch (culture)
            {
                case Culture.CZ:
                    return "cs-CZ";
                default:
                    return "en-US";
            }
        }
    }
}
