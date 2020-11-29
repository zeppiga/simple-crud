using System;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using simple_crud.ApplicationConfiguration;
using simple_crud.Data;
using simple_crud.Data.Entities;

namespace simple_crud.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NoveltyController : ControllerBase
    {
        private readonly ILogger<NoveltyController> _logger;
        private readonly IApplicationConfiguration _configuration;
        private readonly INoveltyRepository _repository;

        public NoveltyController(ILogger<NoveltyController> logger, IApplicationConfiguration configuration, INoveltyRepository repository)
        {
            _logger = logger;
            _configuration = configuration;
            _repository = repository;
        }

        [HttpGet]
        [Route("getCount")]
        public async Task<IActionResult> GetCount(CancellationToken cancellationToken)
        {
            var count = await ExecuteActionInRepositoryContext(() => _repository.TotalCountAsync(cancellationToken));

            return Ok(count);
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetInfos([FromQuery] int take, [FromQuery] int offset, CancellationToken cancellationToken)
        {
            var infos = await ExecuteActionInRepositoryContext(() => _repository.GetInfosAsync(take, offset, cancellationToken));

            return Ok(infos);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            var novelty = await ExecuteActionInRepositoryContext(() => _repository.GetAsync(id, cancellationToken));
            return Ok(novelty);
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add([FromBody] object dto, CancellationToken cancellationToken)
        {
            Novelty novelty = default;
            var result = await ExecuteActionInRepositoryContext(() => _repository.TryAdd(novelty, cancellationToken));

            return Ok(result);
        }
    
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Modify(int id, [FromBody] object dto, CancellationToken cancellationToken)
        {
            Novelty novelty = default;
            var result = await ExecuteActionInRepositoryContext(() => _repository.TryUpdate(id, novelty, cancellationToken));
            return Ok(novelty);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            Novelty novelty = default;
            var result = await ExecuteActionInRepositoryContext(() => _repository.TryUpdate(id, novelty, cancellationToken));
            return Ok(novelty);
        }

        private async Task<T> ExecuteActionInRepositoryContext<T>(Func<Task<T>> action)
        {
            try
            {
                var result = await action();
                return result;
            }
            catch (NoveltyRepositoryException ex)
            {
                _logger.LogError(ex, "Exception was raised during request handling!");
                throw;
            }
        }
    }
}
