using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MyPortfolio.Models
{
	public class Course
	{
		public int Id { get; set; }

		[DisplayName("Course Code")]
		[Required(ErrorMessage = "Required"), MaxLength(12, ErrorMessage = "Max Length: 12"), MinLength(4, ErrorMessage = "Min Length: 4")]
		public string? Code { get; set; }

		[DisplayName("Course Name")]
		[Required(ErrorMessage = "Required"), MaxLength(100, ErrorMessage = "Max Length: 100"), MinLength(4, ErrorMessage = "Min Length: 4")]
		public string? Name { get; set; }

		[DisplayName("Progression")]
		[Required(ErrorMessage = "Required"), MaxLength(1, ErrorMessage = "Length: 1")]
		public string? Progression { get; set; }

		[DisplayName("Syllabus")]
		[Required(ErrorMessage = "Required"), Url, MaxLength(600, ErrorMessage = "Max Length: 600"), MinLength(10, ErrorMessage = "Min Length: 10")]
		public string? Syllabus { get; set; }

		[DisplayName("Start Date")]
		[Required(ErrorMessage = "Required")]
		public DateTime? StartDate { get; set; }

		[DisplayName("End Date")]
		[Required(ErrorMessage = "Required")]
		public DateTime? EndDate { get; set; }

		public string? ImageName { get; set; }

		[DisplayName("Image Title")]
		[Required(ErrorMessage = "Required"), MaxLength(100, ErrorMessage = "Max Length: 100"), MinLength(4, ErrorMessage = "Max Length: 4")]
		public string? ImageTitle { get; set; }

		[NotMapped]
		[DisplayName("Upload Image-File")]
		[Required(ErrorMessage = "Required")]
		public IFormFile? ImageFile { get; set; }

		public Course()
		{
		}
	}
}
