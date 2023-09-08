namespace user_api.Models.DTOs
{
    public class AddressDTO
    {
        public string? Place { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public int PinCode { get; set; }
    }
}
