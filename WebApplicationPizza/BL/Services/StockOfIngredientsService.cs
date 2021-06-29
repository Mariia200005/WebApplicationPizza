using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationPizza.Models
{
    public class StockOfIngredientsService
    {
        IGridFSBucket gridFS;
        IMongoCollection<Ingredient> Ingredients;

        public StockOfIngredientsService()
        {
            string connectionString = "mongodb://localhost:27017/pizzeria";
            var connection = new MongoUrlBuilder(connectionString);
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase database = client.GetDatabase(connection.DatabaseName);
            gridFS = new GridFSBucket(database);
            Ingredients = database.GetCollection<Ingredient>("Ingredient");
        }

        public async Task<IEnumerable<Ingredient>> GetIngredients()
        {
            return await Ingredients.Find(Ingredient => true).ToListAsync();
        }

        public async Task ReplenishStockOfIngredients(string id, int count)
        {
            var ingredient = await Ingredients.Find(new BsonDocument("_id", new ObjectId(id))).FirstOrDefaultAsync();
            if(ingredient != null)
            {
                ingredient.Count += count;
            }
            Ingredients.ReplaceOne(i => i.Name == ingredient.Name, ingredient);
        }
    }
}
