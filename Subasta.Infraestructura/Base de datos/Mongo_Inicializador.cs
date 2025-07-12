using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using Subasta.Dominio.Entidades;

namespace Subasta.Infraestructura.Base_de_datos
{
    public class Mongo_Inicializador
    {
        private readonly IMongoClient _mongoClient;
        public Mongo_Inicializador(IMongoClient mongoClient)
        { _mongoClient = mongoClient; }
        public void Initialize()
        { var database = _mongoClient.GetDatabase("Subastas_db"); var collection = database.GetCollection<Subastas_Mongo>("subastas"); }
    }
}
