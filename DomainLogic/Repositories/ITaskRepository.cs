using DomainLogic.Filters;
using DomainLogic.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainLogic.Repositories
{
    public interface ITaskRepository
    {
        Task Create(TaskEntity task);
        Task Delete(TaskEntity task);
        Task<List<TaskEntity>> Get(TaskFilter filter);
        Task Update(TaskEntity task);
    }
}