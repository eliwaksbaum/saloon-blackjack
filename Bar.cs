using Algiers;
using System;

public class Bar
{
    class Drink : GameObject
    {
        public int price;
        public string description;

        public Drink(string id) : base(id) {}

        public string What()
        {
            return description;
        }

        public Func<string> DrinkResponse(Player player)
        {
            return () =>
            {
                player.RemoveFromInventory(this);
                return "You drink the " + ID + ". This place has good stuff";
            };
        }
    }

    public static Func<string, string> Buy(Player player)
    {  
        return (drinkName) =>
        {
            Drink drink = MakeDrink(drinkName);
            drink.SetTransitiveCommand("what", drink.What);
            drink.SetTransitiveCommand("drink", drink.DrinkResponse(player));
            
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
                drink.price = 6;
                drink.description = "A glass of imported gin.";
                break;
            case ("moonshine"):
                drink.price = 15;
                drink.description = "A whole bottle of ... you're not sure what. It reeks, though.";
                break;
            default:
                break;
        }
        return drink;
    }
}