﻿using API.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ServiceFilter(typeof(LogUserActivity))]
public class BaseAPIController : ControllerBase
{
    // This class serves as a base controller for all API controllers in the application.
}
