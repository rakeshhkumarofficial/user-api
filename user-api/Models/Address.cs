using System.ComponentModel.DataAnnotations;

namespace user_api.Models
{
    public class Address
    {
        [Key]
        public Guid Id { get; set; }
        public string? Place { get; set; } 
        public string? City { get; set; }
        public string? State { get; set; }
        public int PinCode { get; set; }     
        public Guid? UserId { get; set; }
        public DateTime CreatedAt { get; set; } 
        public DateTime UpdatedAt { get; set; }

    }
}
