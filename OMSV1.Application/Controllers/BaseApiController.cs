using Microsoft.AspNetCore.Mvc;

namespace OMSV1.Application.Controllers;

// [ServiceFilter(typeof(LogUserActivity))]
[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{

}
