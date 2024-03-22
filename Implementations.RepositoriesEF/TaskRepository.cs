using AutoMapper;
using DomainLogic.Filters;
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

        public TaskRepository(TaskAppDbContext dbContext, IMapper mapper)
        {
            _context = dbContext;
            _mapper = mapper;
        }

        public async Task Create(DomainTask task)
        {
            var dbTask = _mapper.Map<TaskEntity>(task);
            
            await _context.Tasks.AddAsync(dbTask);

            await _context.SaveChangesAsync();
        }

        public async Task Delete(DomainTask task)
        {
            var dbTask = _context.ChangeTracker.Entries<TaskEntity>().FirstOrDefault(t => t.Entity.Id == task.Id)?.Entity;
            if (dbTask is null)
                dbTask = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == task.Id);

            if (dbTask is null)
                throw new Exception("NOT FOUND");

            dbTask.DeleteDateTime = DateTimeOffset.UtcNow;
            await _context.SaveChangesAsync();
        }

        public async Task<List<DomainTask>> Get(TaskFilter filter)
        {
            var query = _context.Tasks.Where(t => !t.DeleteDateTime.HasValue);
            if (filter.Id is not null)
                query = query.Where(t => t.Id == filter.Id);

            var tasks = await query.ToListAsync();
            return _mapper.Map<List<DomainTask>>(tasks);
        }

        public async Task Update(DomainTask task)
        {
            var dbTask = _context.ChangeTracker.Entries<TaskEntity>().FirstOrDefault(t => t.Entity.Id == task.Id)?.Entity;
            if (dbTask is null) 
                dbTask = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == task.Id);

            if (dbTask is null)
                throw new Exception("NOT FOUND");

            _mapper.Map(task, dbTask);

            await _context.SaveChangesAsync();
        }
    }
}