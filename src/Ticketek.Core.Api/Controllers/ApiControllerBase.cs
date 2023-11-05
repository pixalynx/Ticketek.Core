using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ticketek.Core.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApiControllerBase : Controller
{
    private ISender _mediator = null!;
    
    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}