using System;
namespace Domain.Dtos;

    public class AddProjectData
    {
        public string? ProjectName { get; set; }
        public string? Client { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Member { get; set; }
        public decimal Budget { get; set; }
        public string? ImageUrl { get; set; }
    
    }
