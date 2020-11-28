using System;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using simple_crud.ApplicationConfiguration;
using simple_crud.Data;
using simple_crud.Data.Contexts;
using simple_crud.Data.Entities;

namespace simple_crud.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NoveltyController : ControllerBase
    {
        private readonly ILogger<NoveltyController> _logger;
        private readonly IApplicationConfiguration _configuration;
        //private readonly NoveltyContext _noveltyContext;
        private INoveltyRepository _noveltyRepository;

        public NoveltyController(ILogger<NoveltyController> logger, IApplicationConfiguration configuration, NoveltyContext noveltyContext)
        {
            _logger = logger;
            _configuration = configuration;
            //_noveltyContext = noveltyContext;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetNames(CancellationToken cancellationToken)
        {
            var novelties = await ExecuteActionInRepositoryContext(() => _noveltyRepository.GetNamesAsync(cancellationToken));

            return Ok(novelties);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            var novelty = await ExecuteActionInRepositoryContext(() => _noveltyRepository.GetAsync(id, cancellationToken));
            return Ok(novelty);
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add([FromBody] object dto, CancellationToken cancellationToken)
        {
            Novelty novelty = default;
            var result = await ExecuteActionInRepositoryContext(() => _noveltyRepository.TryAdd(novelty, cancellationToken));

            return Ok(result);
        }
    
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Modify(int id, [FromBody] object dto, CancellationToken cancellationToken)
        {
            Novelty novelty = default;
            var result = await ExecuteActionInRepositoryContext(() => _noveltyRepository.TryUpdate(id, novelty, cancellationToken));
            return Ok(novelty);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            Novelty novelty = default;
            var result = await ExecuteActionInRepositoryContext(() => _noveltyRepository.TryUpdate(id, novelty, cancellationToken));
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
