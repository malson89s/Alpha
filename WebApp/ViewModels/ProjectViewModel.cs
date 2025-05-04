using Google.Apis.Services;

namespace WebApp.ViewModels
{
    public class ProjectViewModel
    {
        public AddProjectViewModel AddProjectFormData { get; set; } = new();
        public EditProjectViewModel EditProjectFormData { get; set; }
        public IEnumerable<ProjectCardViewModel> Projects { get; set; } = new List<ProjectCardViewModel>();
    }
}