using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MyPortfolio.Models
{
    public class Project
    {
        public int Id { get; set; }

        [DisplayName("Project Name")]
        [Required(ErrorMessage = "Required"), MaxLength(100, ErrorMessage = "Max Length: 100"), MinLength(2, ErrorMessage = "Min Length: 2")]
        public string? Name { get; set; }

        [DisplayName("Project Context"), MaxLength(300, ErrorMessage = "Max Length: 300"), MinLength(20, ErrorMessage = "Min Length: 20")]
        [Required(ErrorMessage = "Required")]
        public string? Context { get; set; }

        [DisplayName("Date Created")]
        [Required(ErrorMessage = "Required")]
        public DateTime? Created { get; set; }

        [DisplayName("Project URL")]
        [Required(ErrorMessage = "Required"), Url(ErrorMessage = "Must be URL-format"), MaxLength(600, ErrorMessage = "Max Length: 600"), MinLength(4, ErrorMessage = "Min Length: 4")]
        public string? Url { get; set; }

        [DisplayName("Slug")]
        [Required(ErrorMessage = "Required"), MaxLength(50, ErrorMessage = "Max Length: 50"), MinLength(2, ErrorMessage = "Min Length: 2")]
        public string? Slug { get; set; }

        public string? ImageName { get; set; }

        [DisplayName("Image Title")]
        [Required(ErrorMessage = "Required"), MaxLength(30, ErrorMessage = "Max Length: 30"), MinLength(2, ErrorMessage = "Min Length: 2")]
        public string? ImageTitle { get; set; }

        [NotMapped]
        [DisplayName("Upload Image-File")]
        [Required(ErrorMessage = "Required")]
        public IFormFile? ImageFile { get; set; }

        [DisplayName("Category ID")]
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        public Project()
        {
        }
    }
}
