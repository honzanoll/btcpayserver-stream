namespace BTCPayServer.Stream.Business.Converters.Abstractions
{
    public interface IConverter<TConverter> where TConverter : BaseConverter
    {
        #region Public methods

        string Convert(string text);

        #endregion
    }
}
