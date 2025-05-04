using Data.Entities;
using WebApp.ViewModels;

namespace WebApp.Extensions
{
    public static class ProjectMappingExtensions
    {
        public static ProjectCardViewModel ToCardViewModel(this ProjectEntity entity)
        {
            return new ProjectCardViewModel
            {
                ProjectName = entity.ProjectName,
                Description = entity.Description,
                ImageUrl = entity.ImageUrl,
                StatusName = entity.Status?.StatusName ?? "",
            };
        }
    }
}
