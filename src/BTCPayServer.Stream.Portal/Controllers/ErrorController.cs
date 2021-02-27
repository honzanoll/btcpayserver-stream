using BTCPayServer.Stream.Common.Resources;
using BTCPayServer.Stream.Portal.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BTCPayServer.Stream.Portal.Controllers
{
    [AllowAnonymous]
    public class ErrorController : ControllerBase
    {
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Index()
        {
            return View(new ErrorViewModel(500, CommonResource.Message_SomethingWentWrong));
        }

        [HttpGet("Error/{code}")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Index(int code)
        {
            string message;

            switch ((HttpStatusCode)code)
            {
                case HttpStatusCode.NotFound:
                    message = CommonResource.Message_PageNotFound;
                    break;
                default:
                    message = CommonResource.Message_SomethingWentWrong;
                    break;
            }

            return View(new ErrorViewModel(code, message));
        }
    }
}
