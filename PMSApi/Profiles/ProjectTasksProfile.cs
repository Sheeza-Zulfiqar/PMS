using AutoMapper;
using PMSApi.DTOs.ProjectTasksDtos;
using PMSApi.Entities;
using DomainTaskStatus = PMSApi.Enums.TaskStatus;  

namespace PMSApi.Profiles
{
    public class ProjectTasksProfile : Profile
    {
        public ProjectTasksProfile()
        {
            CreateMap<ProjectTaskCreateDto, ProjectTasks>()
                .ForMember(d => d.ProjectId, o => o.Ignore())  
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status ?? DomainTaskStatus.Pending))
                .ForMember(d => d.CompletedAt, o => o.Ignore())
                 .ForAllMembers(o => o.Condition((src, dest, srcMember, destMember, ctx) => srcMember != null));

            CreateMap<ProjectTaskUpdateDto, ProjectTasks>()
                .ForAllMembers(o => o.Condition((src, dest, srcMember, destMember, ctx) => srcMember != null));

            CreateMap<ProjectTasks, ProjectTaskReadDto>();
        }
    }
}
