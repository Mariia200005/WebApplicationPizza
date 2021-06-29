using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebApplicationPizza.Models
{
    public class PizzaByUserRecipeService
    {
        IGridFSBucket gridFS;
        IMongoCollection<Ingredient> Ingredients;
        IMongoCollection<Pizza> Orders;

        public PizzaByUserRecipeService()
        {
            string connectionString = "mongodb://localhost:27017/pizzeria";
            var connection = new MongoUrlBuilder(connectionString);
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase database = client.GetDatabase(connection.DatabaseName);
            gridFS = new GridFSBucket(database);
            Ingredients = database.GetCollection<Ingredient>("Ingredient");
            Orders = database.GetCollection<Pizza>("Order");
        }

        public async Task<IEnumerable<Ingredient>> GetIngredients()
        {
            return await Ingredients.Find(Ingredient => true).ToListAsync();
        }

        public async Task<Pizza> CreateOrder(List<string> NamesOfIngredients)
        {
            Pizza order = new Pizza();
            order.Name = "UsersRecipe";
            order.Price = 60;
            order.Ingredients = new List<string> { };
            CheckUnavailableIngredients(NamesOfIngredients);
            foreach (var ingredientName in NamesOfIngredients)
            {
                var ingredient = await Ingredients.Find(p => p.Name == ingredientName).FirstOrDefaultAsync();
                if (ingredient != null)
                {
                    order.Ingredients.Add(ingredient.Name);
                    ingredient.Count--;
                    //Time to get the ingredient out of the fridge
                    Thread.Sleep(2000);
                    order.Price += ingredient.Price;
                }
                else
                {
                    throw new Exception($"{ingredientName} does not exist!");
                }
            }
            //Time for cooking
            Thread.Sleep(10000);
            await Orders.InsertOneAsync(order);
            return order;
        }

        private void CheckUnavailableIngredients(IEnumerable<string> ingredients)
        {
            string exceptionMessage = "no_ingredients:";
            bool isUnavailableIngredients = false;
            foreach (var ingr in ingredients)
            {
                var ingredient = Ingredients.Find(p => p.Name == ingr).FirstOrDefault();
                if (ingredient!=null && ingredient.Count == 0)
                {
                    exceptionMessage += $" {ingredient.Name}";
                    isUnavailableIngredients = true;
                }
            }
            if (isUnavailableIngredients)
            {
                throw new Exception(exceptionMessage);
            }
        }
    }
}
