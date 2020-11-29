using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using simple_crud.ApplicationConfiguration;
using simple_crud.Data;
using simple_crud.DTO;

namespace simple_crud.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NoveltyInfoController : Controller
    {
        private readonly ILogger<NoveltyController> _logger;
        private readonly IApplicationConfiguration _configuration;
        private readonly INoveltyRepository _repository;

        public NoveltyInfoController(ILogger<NoveltyController> logger, IApplicationConfiguration configuration, INoveltyRepository repository)
        {
            _logger = logger;
            _configuration = configuration;
            _repository = repository;
        }

        [HttpGet]
        [Route("getCount")]
        public async Task<IActionResult> GetCount(CancellationToken cancellationToken)
        {
            var count = await _repository.TotalCountAsync(cancellationToken);

            return Ok(count);
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Get([FromQuery] int take, [FromQuery] int offset, CancellationToken cancellationToken)
        {
            var infos = await _repository.GetInfosAsync(take, offset, cancellationToken);
            var dto = infos.Select(x => new BasicNoveltyInfoDto { Id = x.Id, LastChanged = x.LastChanged, Name = x.Name });

            return Ok(dto);
        }

    }
}
