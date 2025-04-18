using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET.Common.Base;

[Route("api/[controller]/[action]")]
[ApiController]
public abstract class BaseApiController : ControllerBase
{
	protected readonly ISender _sender;

	protected BaseApiController(ISender sender)
    {
        _sender = sender;
    }
}
