namespace BTCPayServer.Stream.Portal.ViewModels
{
    public class ErrorViewModel
    {
        #region Properties

        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public string ErrorNumber { get; set; }

        public string ErrorMessage { get; set; }

        #endregion

        #region Constructors

        public ErrorViewModel() { }

        public ErrorViewModel(int code)
        {
            ErrorNumber = code.ToString();
        }

        public ErrorViewModel(int code, string message)
        {
            ErrorNumber = code.ToString();
            ErrorMessage = message;
        }

        #endregion
    }
}
