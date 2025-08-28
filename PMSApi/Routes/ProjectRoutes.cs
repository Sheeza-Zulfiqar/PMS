
using AutoMapper;
using PMSApi.DTOs.ProjectDtos;
using PMSApi.DTOs.ProjectTasksDtos;
using PMSApi.Entities;
using PMSApi.Enums;
using PMSApi.Interfaces;
using PMSApi.Services.Interfaces;
using PMSApi.Utils;
using DomainTaskStatus = PMSApi.Enums.TaskStatus;


namespace PMSApi.Routes
{
        public static class ProjectRoutes
        {
            public static void MapProjectRoutes(this IEndpointRouteBuilder app)
            {
                var grp = app.MapGroup("/projects");

 
                grp.MapGet("/", GetMyProjects)
                   .SetRequiredAccessLevel(AccessLevel.Read);

                grp.MapGet("/{id}", GetProjectById)
                   .SetRequiredAccessLevel(AccessLevel.Read);

                grp.MapPost("/", CreateProject)
                   .SetRequiredAccessLevel(AccessLevel.ReadCreateUpdate);

                grp.MapPut("/{id}", UpdateProject)
                   .SetRequiredAccessLevel(AccessLevel.ReadCreateUpdate);

                grp.MapDelete("/{id}", DeleteProject)
                   .SetRequiredAccessLevel(AccessLevel.ReadCreateUpdate);

 
                grp.MapGet("/{projectId}/tasks", GetTasksForProject)
                   .SetRequiredAccessLevel(AccessLevel.Read);

                grp.MapPost("/{projectId}/tasks", CreateTaskForProject)
                   .SetRequiredAccessLevel(AccessLevel.ReadCreateUpdate);

                grp.MapPut("/{projectId}/tasks/{taskId}", UpdateTask)
                   .SetRequiredAccessLevel(AccessLevel.ReadCreateUpdate);

                grp.MapDelete("/{projectId}/tasks/{taskId}", DeleteTask)
                   .SetRequiredAccessLevel(AccessLevel.ReadCreateUpdate);
                grp.MapGet("/{projectId}/tasks/{taskId}", GetTaskById)
                   .SetRequiredAccessLevel(AccessLevel.Read);

        }


        private static async Task<IResult> GetMyProjects(IProjectRepo repo, IMapper mapper, IAuthService auth)
            {
                var uid = auth.GetUser().Id;
                if (uid <= 0) return Results.Unauthorized();

                var items = await repo.GetMineAsync(uid);
                return Results.Ok(mapper.Map<IEnumerable<ProjectReadDto>>(items));
            }

            private static async Task<IResult> GetProjectById(IProjectRepo repo, IMapper mapper, IAuthService auth, int id)
            {
                var uid = auth.GetUser().Id;
                if (uid <= 0) return Results.Unauthorized();

                var proj = await repo.GetOwnedAsync(id, uid);
                return proj is null ? Results.NotFound() : Results.Ok(mapper.Map<ProjectReadDto>(proj));
            }

            private static async Task<IResult> CreateProject(IProjectRepo repo, IMapper mapper, IAuthService auth, ProjectCreateDto dto)
            {
                var me = auth.GetUser();
                if (me.Id <= 0) return Results.Unauthorized();

                if (await repo.NameExistsForUserAsync(me.Id, dto.Name)) return Results.BadRequest("Project name already exists.");

                var model = mapper.Map<Project>(dto);
                model.UserId = me.Id;
                model.CreatedAt = DateTime.Now;
                model.UpdatedAt = DateTime.Now;

                await repo.CreateAsync(model);
                await repo.SaveChangesAsync(me);

                return Results.Created($"/projects/{model.Id}", mapper.Map<ProjectReadDto>(model));
            }

            private static async Task<IResult> UpdateProject(IProjectRepo repo, IMapper mapper, IAuthService auth, int id, ProjectUpdateDto dto)
            {
                var me = auth.GetUser();
                if (me.Id <= 0) return Results.Unauthorized();

                var proj = await repo.GetOwnedAsync(id, me.Id);
                if (proj is null) return Results.NotFound();

                if (await repo.NameExistsForUserAsync(me.Id, dto.Name, excludeId: id))
                    return Results.BadRequest("Project name already exists.");

                mapper.Map(dto, proj);
                proj.UpdatedAt = DateTime.Now;

                await repo.SaveChangesAsync(me);
                return Results.Ok(mapper.Map<ProjectReadDto>(proj));
            }

            private static async Task<IResult> DeleteProject(IProjectRepo repo, IAuthService auth, int id)
            {
                var me = auth.GetUser();
                if (me.Id <= 0) return Results.Unauthorized();

                var proj = await repo.GetOwnedAsync(id, me.Id);
                if (proj is null) return Results.NotFound();

                repo.Delete(proj);
                await repo.SaveChangesAsync(me);
                return Results.NoContent();
            }

 
            private static async Task<IResult> GetTasksForProject(IProjectRepo projRepo, IProjectTaskRepo taskRepo, IMapper mapper, IAuthService auth, int projectId)
            {
                var uid = auth.GetUser().Id;
                if (uid <= 0) return Results.Unauthorized();

                 var proj = await projRepo.GetOwnedAsync(projectId, uid);
                if (proj is null) return Results.NotFound();

                var tasks = await taskRepo.GetForProjectAsync(projectId, uid);
                return Results.Ok(mapper.Map<IEnumerable<ProjectTaskReadDto>>(tasks));
            }

            private static async Task<IResult> CreateTaskForProject(IProjectRepo projRepo, IProjectTaskRepo taskRepo, IMapper mapper, IAuthService auth, int projectId, ProjectTaskCreateDto dto)
            {
                var me = auth.GetUser();
                if (me.Id <= 0) return Results.Unauthorized();

                var proj = await projRepo.GetOwnedAsync(projectId, me.Id);
                if (proj is null) return Results.NotFound();

                var task = mapper.Map<ProjectTasks>(dto);
                task.ProjectId = projectId;
                task.CreatedAt = DateTime.Now;
                task.UpdatedAt = DateTime.Now;

                await taskRepo.CreateAsync(task);
                await taskRepo.SaveChangesAsync(me);

                return Results.Created($"/projects/{projectId}/tasks/{task.Id}", mapper.Map<ProjectTaskReadDto>(task));
            }

        private static async Task<IResult> UpdateTask(
            IProjectTaskRepo taskRepo,
            IProjectRepo projRepo,
            IMapper mapper,
            IAuthService auth,
            int projectId,
            int taskId,
            ProjectTaskUpdateDto dto)
        {
            var me = auth.GetUser();
            if (me.Id <= 0) return Results.Unauthorized();

            var task = await taskRepo.GetOwnedTaskAsync(taskId, me.Id);
            if (task is null || task.ProjectId != projectId) return Results.NotFound();

            var wasDone = task.Status == DomainTaskStatus.Done;

            mapper.Map(dto, task);

            if (!wasDone && task.Status == DomainTaskStatus.Done)
                task.CompletedAt = DateTime.UtcNow;        
            else if (wasDone && task.Status != DomainTaskStatus.Done)
                task.CompletedAt = null;

            task.UpdatedAt = DateTime.UtcNow;

            await taskRepo.SaveChangesAsync(me);
            return Results.Ok(mapper.Map<ProjectTaskReadDto>(task));
        }

        private static async Task<IResult> DeleteTask(IProjectTaskRepo taskRepo, IProjectRepo projRepo, IAuthService auth, int projectId, int taskId)
            {
                var me = auth.GetUser();
                if (me.Id <= 0) return Results.Unauthorized();

                var task = await taskRepo.GetOwnedTaskAsync(taskId, me.Id);
                if (task is null || task.ProjectId != projectId) return Results.NotFound();

                taskRepo.Delete(task);
                await taskRepo.SaveChangesAsync(me);
                return Results.NoContent();
            }
        private static async Task<IResult> GetTaskById(
        IProjectRepo projRepo,
        IProjectTaskRepo taskRepo,
        IMapper mapper,
        IAuthService auth,
        int projectId,
        int taskId)
        {
            var id = auth.GetUser().Id;
            if (id <= 0) return Results.Unauthorized();

            var proj = await projRepo.GetOwnedAsync(projectId, id);
            if (proj is null) return Results.NotFound();

            var task = await taskRepo.GetOwnedTaskAsync(taskId, id);
            if (task is null || task.ProjectId != projectId) return Results.NotFound();

            var dto = mapper.Map<ProjectTaskReadDto>(task);
            return Results.Ok(dto);
        }


    }

}
