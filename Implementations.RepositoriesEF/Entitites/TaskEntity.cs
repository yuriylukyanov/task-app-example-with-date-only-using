using DomainLogic.Models;

namespace Implementations.RepositoriesEF.Entitites
{
    public class TaskEntity
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreateDateTime { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly FinishDate { get; set; }
        public string Name { get; set; }
        public DateTimeOffset? DeleteDateTime { get; set; }
    }
}