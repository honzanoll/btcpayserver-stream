using BTCPayServer.Stream.Common.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace BTCPayServer.Stream.Portal.Controllers
{
    [Authorize]
    public class ControllerBase : Controller
    {
        #region Properties

        protected Guid CurrentUserId
        {
            get
            {
                string nameIdentifier = HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (nameIdentifier != null && Guid.TryParse(nameIdentifier, out Guid userId))
                    return userId;

                throw new ApplicationException(CommonResource.Message_CannotAccessUserIdentity);
            }
        }

        #endregion
    }
}
