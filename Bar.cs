using Algiers;
using System;

public static class Bar
{
    class Drink : GameObject
    {
        public int price;
        public string description;

        public Drink(string id) : base(id, true) {}

        public string What()
        {
            return description;
        }

        public Func<string> DrinkResponse(Player player)
        {
            return () =>
            {
                player.RemoveFromInventory(ID);
                return "You drink the " + ID + ". This place has good stuff";
            };
        }
    }

    public static Func<string, string> Buy(Player player)
    {  
        return (drinkName) =>
        {
            Drink drink = MakeDrink(drinkName);
            drink.SetTransitiveResponse("what", drink.What);
            drink.SetTransitiveResponse("drink", drink.DrinkResponse(player));
            
            player.AddToInventory(drink);
            player.IncrementCounter("money", -drink.price);
            return "You hand the barkeep " + drink.price + "Ð and they slide you a " + drinkName + ".";
        };
    }

    static Drink MakeDrink(string name)
    {
        Drink drink = new Drink(name);
        switch(name)
        {
            case ("whiskey"):
                drink.price = 3;
                drink.description = "A glass of aged amber whiskey.";
                break;
            case ("tequila"):
                drink.price = 5;
                drink.description = "A glass of Mexican tequila.";
                break;
            case ("gin"):
                drink.price = 5;
                drink.description = "A glass of imported gin.";
                break;
            case ("moonshine"):
                drink.price = 8;
                drink.description = "A glass of ... you're not sure what. It reeks, though.";
                break;
            case ("bottle of whiskey"):
                drink.price = 14;
                drink.description = "A whole bottle of aged amber whiskey";
                break;
            default:
                break;
        }
        return drink;
    }
}