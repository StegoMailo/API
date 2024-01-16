using Usable_Security_Project_Key_Registry.Models;

namespace Usable_Security_Project_Key_Registry.Repositories
{
    public interface IUserRepository
    {

        public IEnumerable<User> Get();

        public User? GetByID(int id);

        public User? GetByEmail(string email);

        public User? GetByPublicKey(string publicKey);

        public string GetPublicKey(string email);

        public void Add(User user);
        public void Update(User user);

        public void Save();

    }
}
