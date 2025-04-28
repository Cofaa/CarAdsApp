using CarAdsApp.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;

namespace CarAdsApp.Services
{
    public class OglasiServices
    {
        private readonly IMongoCollection<Oglas> _oglasi; // Kolekcija oglasa

        public OglasiServices(IConfiguration config) // Konstruktori
        {
            var client = new MongoClient(config["MongoDB:ConnectionString"]); // Konekcija na MongoDB
            var database = client.GetDatabase(config["MongoDB:DatabaseName"]); // Baza podataka
            _oglasi = database.GetCollection<Oglas>(config["MongoDB:OglasiCollection"]); // Kolekcija oglasa
        }

        public List<Oglas> GetAll()
        {
            return _oglasi.Find(oglas => true).ToList(); // Pronalazi sve oglase
        }

        public Oglas GetById(string id)
        {
            return _oglasi.Find(oglas => oglas.Id == id).FirstOrDefault(); // Pronalazi oglas po ID-u
        }

        public List<Oglas> GetOglasByCena()
        {
            return _oglasi.Find(oglas => true).SortByDescending(oglas => oglas.Cena).ToList(); // Pronalazi oglase po ceni
        }

        public void Create(Oglas oglas)
        {
            _oglasi.InsertOne(oglas); // Unosi novi oglas
        }
    }
}
