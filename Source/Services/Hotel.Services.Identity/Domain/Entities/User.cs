using System;
using Hotel.Common.Entities;
using Hotel.Common.Extensions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Hotel.Services.Identity.Domain.Entities
{
    public class User : Entity
    {
        private User() { }

        public User(string password)
        {
            PasswordHash = HashPassword(password);
        }

        public string UserName { get; set; }
        public string PasswordHash { get; private set; }

        private static string HashPassword(string password) => password.ComputeSha256Hash();
    }
}