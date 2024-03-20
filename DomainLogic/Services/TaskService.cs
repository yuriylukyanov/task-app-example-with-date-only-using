using DomainLogic.Models;
using DomainLogic.ParameterModels;
using DomainLogic.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLogic.Services
{
    public class TaskService
    {
        private readonly ITaskRepository _repository;

        public TaskService(
            ITaskRepository repository)
        {
            _repository = repository;
        }

        public async Task<TaskEntity> Create(TaskCreateDTO dto)
        {
            var task = new TaskEntity
            {
                Id = Guid.NewGuid(),
                CreateDateTime = DateTimeOffset.UtcNow,
                StartDate = dto.StartDate,
                FinishDate = dto.FinishDate,
                Name = dto.Name,
            };

            await _repository.Create(task);

            return task;
        }

        public async Task<List<TaskEntity>> Get()
        {
            return await _repository.Get();
        }

    }
}
