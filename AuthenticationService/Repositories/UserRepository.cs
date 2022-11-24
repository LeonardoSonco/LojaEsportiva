using AuthenticationService.Model;

namespace AuthenticationService.Repositories
{
    public static class UserRepository
    {
        public static User Get(string username, string password)
        {
            var users = new List<User>
            {
                new User {Id = 1, Username = "Admin", Password = "1234", Role = "manager"},
                new User {Id = 2, Username = "Funcionario", Password = "1234", Role = "employee"}
            };
            return users.Where(x => x.Username.ToLower() == username.ToLower() && x.Password == password).FirstOrDefault();
        }
    }
}
