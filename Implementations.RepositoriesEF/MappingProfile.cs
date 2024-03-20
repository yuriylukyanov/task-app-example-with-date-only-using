using AutoMapper;
using Implementations.RepositoriesEF.Entitites;

namespace Implementations.RepositoriesEF
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DomainLogic.Models.TaskEntity, TaskEntity>()
                .ReverseMap();

        }
    }
}