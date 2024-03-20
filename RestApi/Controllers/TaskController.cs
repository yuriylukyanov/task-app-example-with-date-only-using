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

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] TaskCreateDTO dto)
        {
            var task = await _service.Create(dto);

            return Ok(task);
        }

        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {
            var tasks = await _service.Get();

            return Ok(tasks);
        }
    }
}
