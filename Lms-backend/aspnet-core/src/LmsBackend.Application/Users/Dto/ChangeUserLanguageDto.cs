using System.ComponentModel.DataAnnotations;

namespace LmsBackend.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}