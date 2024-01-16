using AutoMapper;
using System.Security.Cryptography;
using Usable_Security_Project_Key_Registry.Models;
using Usable_Security_Project_Key_Registry.Models.DTO;
using Usable_Security_Project_Key_Registry.Repositories;

namespace Usable_Security_Project_Key_Registry.Services.User_Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public IEnumerable<User> Get()
        {
            return _userRepository.Get();
        }



        public User? GetByEmail(string email)
        {
            return _userRepository.GetByEmail(email);
        }

        public User? GetByID(int id)
        {
            return _userRepository.GetByID(id);
        }

        public User? GetByPublicKey(string publicKey)
        {
           return _userRepository.GetByPublicKey(publicKey);
        }

        public string GetPublicKey(string email)
        {
            return _userRepository.GetPublicKey(email);
        }

        public string Add(UserDTO userDTO)
        {
            User checkForUsers =  _userRepository.GetByEmail(userDTO.Email);

            if (checkForUsers != null)
            {
                return "Email Already in Database!";
            }

            User newUser = _mapper.Map<User>(userDTO);

            string privateKey = "";
            newUser.PublicKey = GeneratePublicAndPrivateKey(out privateKey);
            newUser.PrivateKey = privateKey;

            newUser.CreateDate = DateTime.Now;

            _userRepository.Add(newUser);

            return privateKey;
        }

        public void Update(UserDTO userDTO)
        {   //need to get the user there are more steps
            User newUser = _mapper.Map<User>(userDTO);
            _userRepository.Update(newUser);
        }

        private string GeneratePublicAndPrivateKey(out string privateKey)
        {

            //lets take a new CSP with a new 2048 bit rsa key pair
            var csp = new RSACryptoServiceProvider(2048);

            //how to get the private key
            var privKey = csp.ExportParameters(true);

            //and the public key ...
            var pubKey = csp.ExportParameters(false);

            //converting the public key into a string representation
            string pubKeyString;
            {
                //we need some buffer
                var sw = new System.IO.StringWriter();
                //we need a serializer
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                //serialize the key into the stream
                xs.Serialize(sw, pubKey);
                //get the string from the stream
                pubKeyString = sw.ToString();
            }

            string privKeyString;
            {
                //we need some buffer
                var sw = new System.IO.StringWriter();
                //we need a serializer
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                //serialize the key into the stream
                xs.Serialize(sw, privKey);
                //get the string from the stream
                privKeyString = sw.ToString();
            }


            privateKey = privKeyString;
            return pubKeyString;
         
        }

        
    }
}
