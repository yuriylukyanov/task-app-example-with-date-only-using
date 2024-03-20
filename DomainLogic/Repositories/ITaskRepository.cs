using DomainLogic.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainLogic.Repositories
{
    public interface ITaskRepository
    {
        Task Create(TaskEntity game);
        Task<List<TaskEntity>> Get();
    }
}