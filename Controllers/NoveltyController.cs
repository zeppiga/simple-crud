using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using simple_crud.ApplicationConfiguration;
using simple_crud.Data.Contexts;

namespace simple_crud.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NoveltyController : ControllerBase
    {
        private readonly ILogger<NoveltyController> _logger;
        private readonly IApplicationConfiguration _configuration;
        private readonly NoveltyContext _noveltyContext;

        public NoveltyController(ILogger<NoveltyController> logger, IApplicationConfiguration configuration, NoveltyContext noveltyContext)
        {
            _logger = logger;
            _configuration = configuration;
            _noveltyContext = noveltyContext;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var novelty = await _noveltyContext.Novelties.SingleAsync(x => x.ID == id);
            return Ok(novelty);
        }
    }
}
