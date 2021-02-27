using System.Text;

namespace BTCPayServer.Stream.Common.Extensions
{
    public static class ByteExtensions
    {
        #region Public methods

        public static string ToHexString(this byte[] bytes)
        {
            StringBuilder builder = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
            {
                builder.AppendFormat("{0:x2}", b);
            }

            return builder.ToString();
        }

        #endregion
    }
}
