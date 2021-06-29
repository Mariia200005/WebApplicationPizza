using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebApplicationPizza.Models
{
    public class StandardPizzaService
    {
        IGridFSBucket gridFS;
        IMongoCollection<Pizza> Pizzas;
        IMongoCollection<Ingredient> Ingredients;
        IMongoCollection<Pizza> Orders;

        public StandardPizzaService()
        {
            string connectionString = "mongodb://localhost:27017/pizzeria";
            var connection = new MongoUrlBuilder(connectionString);
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase database = client.GetDatabase(connection.DatabaseName);
            gridFS = new GridFSBucket(database);
            Pizzas = database.GetCollection<Pizza>("Pizza");
            Ingredients = database.GetCollection<Ingredient>("Ingredient");
            Orders = database.GetCollection<Pizza>("Order");
        }

        public async Task<IEnumerable<Pizza>> GetPizzas()
        {
            return await Pizzas.Find(pizza=>true).ToListAsync();
        }

        public async Task<decimal> CalculateBalance()
        {
            var orders = await Orders.Find(pizza => true).ToListAsync();
            return orders.Sum(order=>order.Price);
        }

        public async Task<Pizza> CreateOrder(string nameOfPizza)
        {
            var pizza = await Pizzas.Find(p => p.Name == nameOfPizza).FirstOrDefaultAsync();
            if (pizza != null)
            {
                await TakeIngredientsFromStock(pizza);
                //Time for cooking
                Thread.Sleep(10000);
            }
            else
            {
                throw new Exception($"{nameOfPizza} does not exist!");
            }
            await Orders.InsertOneAsync(pizza);
            return pizza;
        }
         
        private async Task TakeIngredientsFromStock(Pizza pizza)
        {
            var ingredients = await Ingredients.Find(Ingredient => true).ToListAsync();
            CheckUnavailableIngredients(ingredients);
            foreach (var ingradient in ingredients)
            {
                if(pizza.Ingredients.Any(i=>i==ingradient.Name && ingradient.Count!=0))
                {
                    //Time to get the ingredient out of the fridge
                    Thread.Sleep(2000);
                    ingradient.Count--;
                }
                Ingredients.ReplaceOne(i=>i.Name==ingradient.Name, ingradient);
            }
        }

        private void CheckUnavailableIngredients(IEnumerable<Ingredient> ingredients)
        {
            string exceptionMessage = "no_ingredients:";
            bool isUnavailableIngredients = false;
            foreach (var ingredient in ingredients)
            {
                if(ingredient.Count == 0)
                {
                    exceptionMessage +=  $" {ingredient.Name}";
                    isUnavailableIngredients = true;
                }
            }
            if(isUnavailableIngredients)
            {
                throw new Exception(exceptionMessage);
            }
        }

    }
}
