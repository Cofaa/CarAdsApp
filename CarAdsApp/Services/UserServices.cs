using CarAdsApp.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;

namespace CarAdsApp.Services
{
    public class UserServices
    {
        private readonly IMongoCollection<User> _users; // Kolekcija korisnika

        public UserServices(IConfiguration config) // Konstruktori
        {
            var client = new MongoClient(config["MongoDB:ConnectionString"]); // Konekcija na MongoDB
            var database = client.GetDatabase(config["MongoDB:DatabaseName"]); // Baza podataka
            _users = database.GetCollection<User>(config["MongoDB:UsersCollection"]); // Kolekcija korisnika
        }

        public List<User> GetAll()
        {
            return _users.Find(user => true).ToList(); // Pronalazi sve korisnike
        }

        public User GetByUsername(string username)
        {
            return _users.Find(user => user.Username == username).FirstOrDefault(); // Pronalazi korisnika po korisničkom imenu
        }

        public void Create(User user)
        {
            _users.InsertOne(user); // Unosi novog korisnika
        }
    }
}
