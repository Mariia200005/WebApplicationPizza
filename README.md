# WebApplicationPizza
Pizzeria
!!!!IMPORTANT!!!!
To use this system you need to download mongodb and do next steps:

1.For creating database:
 use pizzeria
2. For creating collection: "Pizza":
 db.Pizza.insertMany([{"Name":"Calzone","Price":220, "Ingredients":["Ham","Oregano","Mushrooms","Mozzarella"]}, {"Name": "Margarita", "Price": 180, "Ingredients": ["Mozzarella", "Basil", "Tomatoes", "Olive oil"]}, { "Name": "Papperoni",  "Price": 200, "Ingredients": ["Mozzarella", "Papperoni", "Parmesan", "Oregano"]}])
3. For creating collection "Order":
 db.createCollection("Order")
4. For creating collection "Ingredient":
 db.Ingredient.insertMany([{
    "Name": "Parmesan",
    "Price": "28",
    "Count": 43
},{
    "Name": "Ham",
    "Price": "35",
    "Count": 49
},{
    "Name": "Mozzarella",
    "Price": "25",
    "Count": 4
},{
    "Name": "Basil",
    "Price": "10",
    "Count": 10
},{
    "Name": "Oregano",
    "Price": "10",
    "Count": 11
},{
    "Name": "Mushrooms",
    "Price": "20",
    "Count": 18
},{
    "Name": "Tomatoes",
    "Price": "14",
    "Count": 21
},{
    "Name": "Olive oil",
    "Price": "8",
    "Count": 38
},{
    "Name": "Papperoni",
    "Price": "30",
    "Count": 20
} ])
 
