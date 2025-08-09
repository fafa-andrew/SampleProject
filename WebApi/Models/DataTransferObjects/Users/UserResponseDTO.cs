using BusinessEntities;
using System.Collections.Generic;

namespace WebApi.Models.DataTransferObjects.Users
{
    public class UserResponseDTO : IdObjectData
    {
        public UserResponseDTO(User user) : base(user)
        {
            Email = user.Email;
            Name = user.Name;
            Age = user.Age;
            Type = new EnumData(user.Type);
            MonthlySalary = user.MonthlySalary;
            AnnualSalary = user.MonthlySalary * 12;
            Tags = user.Tags;
        }

        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public EnumData Type { get; set; }
        public decimal? MonthlySalary { get; set; }
        public decimal? AnnualSalary { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}