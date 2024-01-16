using System.Security.Cryptography.X509Certificates;
using Usable_Security_Project_Key_Registry.Data;
using Usable_Security_Project_Key_Registry.Models;

namespace Usable_Security_Project_Key_Registry.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;

        public UserRepository(ApplicationDbContext db)
        {
            _db = db;
        }


        public IEnumerable<User> Get()
        {
            return _db.User;
        }

        public User? GetByID(int id)
        {
            return _db.User.FirstOrDefault(u => u.Id == id);
        }

        public User? GetByEmail(string email)
        {
            return _db.User.FirstOrDefault(u => u.Email == email);
        }

        public User? GetByPublicKey(string publicKey)
        {
            return _db.User.FirstOrDefault(u => u.PublicKey == publicKey);
        }

        public string GetPublicKey(string email)
        {
            User user = _db.User.FirstOrDefault(u => u.Email == email);

            if (user != null)
            {
                return user.PublicKey;
            }
            else
            {
                return "No Public Key!";
            }
           
        }
        public void Add(User user)
        {
            _db.Add(user);
            Save();
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(User user)
        {
            _db.Add(user);
            Save();
        }
    }
}
