﻿using Usable_Security_Project_Key_Registry.Models;
using Usable_Security_Project_Key_Registry.Models.DTO;

namespace Usable_Security_Project_Key_Registry.Services.User_Services
{
    public interface IUserService
    {
        public IEnumerable<User> Get();

        public User? GetByID(int id);

        public User? GetByEmail(string email);

        public User? GetByPublicKey(string publicKey);

        public bool CheckEmail(string email);

        public bool CheckQR(string email, string QRSignature);
        public bool CheckPIN(string email, string PINSignature);

        public string GetPublicKey(string email);

        public string Add(UserDTO userDTO);
        public void Update(UserDTO userDTO);

    }
}
