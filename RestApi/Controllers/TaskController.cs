using DomainLogic.Models;
using DomainLogic.ParameterModels;
using DomainLogic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ILogger<TaskController> _logger;

        private readonly TaskService _service;

        public TaskController(ILogger<TaskController> logger, TaskService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaskCreateDTO dto)
        {
            var task = await _service.Create(dto);

            return Ok(task);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] TaskEntity dto)
        {
            var task = await _service.Update(dto);

            return Ok(task);
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var tasks = await _service.Get();

            return Ok(tasks);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            await _service.Delete(id);

            return Ok();
        }
    }
}
