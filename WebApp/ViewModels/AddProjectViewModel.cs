using System.ComponentModel.DataAnnotations;
using Domain.Dtos;

namespace WebApp.ViewModels
{
    public class AddProjectViewModel
    {
        [Required]
        public string? ProjectName { get; set; }

        [Required]
        public string? Client { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required]
        public string? Member { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Budget must be a positive number.")]
        public decimal Budget { get; set; }

        public IFormFile? ProjectImage { get; set; } 
        public AddProjectData ToDto(string? imageUrl = null)
        {
            return new AddProjectData
            {
                ProjectName = ProjectName,
                Client = Client,
                Description = Description,
                StartDate = StartDate,
                EndDate = EndDate,
                Member = Member,
                Budget = Budget,
                ImageUrl = imageUrl
            };
        }
    }
}
