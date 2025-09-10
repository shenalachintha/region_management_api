using System.ComponentModel.DataAnnotations;

namespace myApi.DTO
{
    public class RegisterRequestDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]

        public String UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public String Password { get; set; }
        public string[] Roles { get; set; }

    }
}
