using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MyPortfolio.Models;

public class Category
{
	public int Id { get; set; }

	[DisplayName("Category Title")]
	[Required(ErrorMessage = "Required"), MaxLength(30, ErrorMessage = "Max Length: 30"), MinLength(2, ErrorMessage = "Min Length: 2")]
	public string? Title { get; set; }

	[DisplayName("Category Color")]
	[Required(ErrorMessage = "Required"), MaxLength(12, ErrorMessage = "Max Length: 12"), MinLength(3, ErrorMessage = "Min Length: 3")]
	public string? Color { get; set; }

	public string? ImageName { get; set; }

	[DisplayName("Image Title")]
	[Required(ErrorMessage = "Required"), MinLength(2, ErrorMessage = "Min Length: 2"), MaxLength(50, ErrorMessage = "Max Length: 50")]
	public string? ImageTitle { get; set; }

	[NotMapped]
	[DisplayName("Upload Image-File")]
	[Required(ErrorMessage = "Required")]
	public IFormFile? ImageFile { get; set; }

	public ICollection<Project>? Project { get; set; }



	public Category()
	{
	}
}

