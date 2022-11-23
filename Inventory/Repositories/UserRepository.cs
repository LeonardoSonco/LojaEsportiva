using InventoryService.Models;

namespace InventoryService.Repositories
{
    public static class UserRepository
    {
        public static User Get(string username, string password)
        {
            var users = new List<User>
            {
                new User {Id = 1, Username = "Leo", Password = "Leo", Role = "manager"},
                new User {Id = 2, Username = "Valmir", Password = "Valmir", Role = "employee"}
            };
            return users.Where(x => x.Username.ToLower() == username.ToLower() && x.Password == password).FirstOrDefault();
        }
    }
}
