using MedTrack.Api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedTrack.Api.Controllers
{
    [ApiController]
    [Authorize]
    public abstract class ApiControllerBase : ControllerBase
    {
        protected int CurrentUserId => User.GetUserId();
    }
}
