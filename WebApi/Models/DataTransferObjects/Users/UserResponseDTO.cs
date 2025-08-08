using BusinessEntities;

namespace WebApi.Models.DataTransferObjects.Users
{
    public class UserResponseDTO : IdObjectData
    {
        public UserResponseDTO(User user) : base(user)
        {
            Email = user.Email;
            Name = user.Name;
            Type = new EnumData(user.Type);
            MonthlySalary = user.MonthlySalary;
            Age = user.Age;
        }

        public string Name { get; set; }
        public string Email { get; set; }
        public EnumData Type { get; set; }
        public decimal? MonthlySalary { get; set; }
        public int Age { get; set; }
    }
}