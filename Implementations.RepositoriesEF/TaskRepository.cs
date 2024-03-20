using AutoMapper;
using DomainLogic.Models;
using DomainLogic.Repositories;
using Microsoft.EntityFrameworkCore;

using DomainTask = DomainLogic.Models.TaskEntity;
using TaskEntity = Implementations.RepositoriesEF.Entitites.TaskEntity;

namespace Implementations.RepositoriesEF
{
    public class TaskRepository: ITaskRepository
    {
        private readonly TaskAppDbContext _context;
        private readonly IMapper _mapper;

        public TaskRepository(TaskAppDbContext gameDbContext, IMapper mapper)
        {
            _context = gameDbContext;
            _mapper = mapper;
        }

        public async Task Create(DomainTask game)
        {
            var dbTask = _mapper.Map<TaskEntity>(game);
            
            await _context.Tasks.AddAsync(dbTask);

            await _context.SaveChangesAsync();
        }

        public async Task<List<DomainTask>> Get()
        {
            var tasks = await _context.Tasks.ToListAsync();
            return _mapper.Map<List<DomainTask>>(tasks);
        }
    }
}