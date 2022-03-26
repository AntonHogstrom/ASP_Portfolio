using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MyPortfolio.Models
{
	public class SocialMedia
	{
		public int Id { get; set; }

		[DisplayName("Social Media Title")]
		[Required(ErrorMessage = "Required"), MaxLength(100, ErrorMessage = "Max Length: 60"), MinLength(4, ErrorMessage = "Min Length: 4")]
		public string? Title { get; set; }

		[DisplayName("Social Media URL")]
		[Required(ErrorMessage = "Required"), Url(ErrorMessage = "Must be URL-Format"), MaxLength(600, ErrorMessage = "Max Length: 600"), MinLength(4, ErrorMessage = "Min Length: 4")]
		public string? Url { get; set; }

		public string? ImageName { get; set; }

		[DisplayName("Image Title")]
		[Required(ErrorMessage = "Required")]
		public string? ImageTitle { get; set; }

		[NotMapped]
		[DisplayName("Upload Image-File")]
		[Required(ErrorMessage = "Required")]
		public IFormFile? ImageFile { get; set; }

		public SocialMedia()
		{
		}
	}
}
