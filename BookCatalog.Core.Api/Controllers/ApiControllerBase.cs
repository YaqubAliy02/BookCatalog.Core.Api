using AutoMapper;
using BookCatalog.Core.Api.Filters;
using Microsoft.AspNetCore.Mvc;

namespace BookCatalog.Core.Api.Controllers
{
    [ApiController]
    [ValidationActionFilter]
    [CustomExceptionFilter]
    public class ApiControllerBase : ControllerBase
    {
        private readonly IMapper mapper;

        protected IMapper _mapper => mapper ?? HttpContext
            .RequestServices.GetRequiredService<IMapper>();
    }
}
