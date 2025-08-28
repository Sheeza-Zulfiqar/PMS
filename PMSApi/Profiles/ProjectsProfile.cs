using AutoMapper;
using PMSApi.DTOs.ProjectDtos;
using PMSApi.Entities;

namespace PMSApi.Profiles
{
    public class ProjectsProfile : Profile
    {
        public ProjectsProfile()
        {
             CreateMap<ProjectCreateDto, Project>()
                .ForMember(d => d.UserId, o => o.Ignore())  
                .ForMember(d => d.ProjectTasks, o => o.Ignore())
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForMember(d => d.UpdatedAt, o => o.Ignore())
                .ForMember(d => d.CreateUserID, o => o.Ignore())
                .ForMember(d => d.UpdateUserID, o => o.Ignore());


             CreateMap<ProjectUpdateDto, Project>()
                .ForAllMembers(o => o.Condition((src, dest, srcMember) => srcMember != null));

             CreateMap<Project, ProjectReadDto>();
        }
    }
}
