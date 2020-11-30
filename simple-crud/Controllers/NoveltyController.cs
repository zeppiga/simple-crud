using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            var novelty = await _repository.GetAsync(id, cancellationToken);

            if (novelty == null)
                return NotFound($"No novelty with id: {id} was found.");

            return Ok(novelty);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("add")]
        public async Task<IActionResult> Add([FromBody] NoveltyToAddDto dto, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(dto.Name) || string.IsNullOrEmpty(dto.Description))
                return BadRequest("Cannot create novelty without name or description!");

            var noveltyToAdd = new NoveltyToAdd(dto.Name, dto.Description);
            var result = await _repository.TryAdd(noveltyToAdd, cancellationToken);

            if (!result.Succeeded)
            {
                _logger.LogError($"Failed to create novelty. Reason: {result.FailureReason}.");
                return BadRequest("Novelty creation was unsuccessful.");
            }

            return Created($"novelty/{result.AddedItem.ID}", new object());
        }
    
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("{id}")]
        public async Task<IActionResult> Modify(int id, [FromBody] NoveltyToAddDto dto, CancellationToken cancellationToken)
        {
            var noveltyToAdd = new NoveltyToAdd(dto.Name, dto.Description, id);
            var result = await _repository.TryUpdate(noveltyToAdd, cancellationToken);

            if (!result.Succeeded)
            {
                _logger.LogError($"Failed to modify novelty with id: {id}. Reason: {result.FailureReason}.");
                return BadRequest("Novelty modification was unsuccessful.");
            }

            return Ok(result.AddedItem);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var result = await _repository.TryRemove(id, cancellationToken);

            if (!result)
            {
                _logger.LogError($"Failed to delete novelty with id: {id}.");
                return BadRequest("Novelty deletion was unsuccessful.");
            }

            return NoContent();
        }
    }
}
