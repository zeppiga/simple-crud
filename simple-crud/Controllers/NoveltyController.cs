using System;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using simple_crud.Data;
using simple_crud.DTO;

namespace simple_crud.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NoveltyController : ControllerBase
    {
        private readonly ILogger<NoveltyController> _logger;
        private readonly INoveltyRepository _repository;

        public NoveltyController(ILogger<NoveltyController> logger, INoveltyRepository repository)
        {
            _logger = logger;
            _repository = repository;
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
        public async Task<IActionResult> Add([FromBody] NoveltyToAddDto dto, CancellationToken cancellationToken)
        {
            var noveltyToAdd = new NoveltyToAdd(dto.Name, dto.Description);
            var result = await ExecuteActionInRepositoryContext(() => _repository.TryAdd(noveltyToAdd, cancellationToken));

            return Ok(result);
        }
    
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Modify(int id, [FromBody] NoveltyToAddDto dto, CancellationToken cancellationToken)
        {
            var noveltyToAdd = new NoveltyToAdd(dto.Name, dto.Description, id);
            var _ = await _repository.TryUpdate(noveltyToAdd, cancellationToken);

            var novelty = await _repository.GetAsync(id, cancellationToken);

            return Ok(novelty);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var _ = await _repository.TryRemove(id, cancellationToken);

            return Ok();
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
