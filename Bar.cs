using Algiers;
using System;

public static class Bar
{
    class Drink : GameObject
    {
        int price;
        public int Price => price;
        string description;

        public Drink(string id, Player player, int price, string description) : base(id, true)
        {
            this.price = price;
            this.description = description;
            SetTransitiveResponse("what", What);
            SetTransitiveResponse("drink", DrinkResponse(player));
        }

        public string What()
        {
            return description;
        }

        public Func<string> DrinkResponse(Player player)
        {
            if (ID == "bottle of whiskey")
            {
                return () =>
                {
                    return "You take a swig of whiskey from the bottle. Not bad.";
                };
            }
            else
            {
                return () =>
                {
                    player.RemoveFromInventory(ID);
                    return "You drink the " + ID + ". Not bad.";
                };
            }
        }
    }

    public static Func<string, string> Buy(SPlayer player)
    {  
        return (drinkName) =>
        {
            if (!player.HasWaypoint("firstdrink"))
            {
                player.AddWaypoint("firstdrink");
            }
            Drink drink = MakeDrink(drinkName, player);

            if (drink.Price == 0)
            {
                return "The bartender gives you a look. They must not have any " + drinkName + ".";
            }

            if (player.GetCounter("money") >= drink.Price)
            {
                player.AddToInventory(drink);
                player.IncrementCounter("money", -drink.Price);
                return "You hand the barkeep " + drink.Price + "Ð and they slide you a " + drinkName + ".";
            }
            return "You can't afford a " + drinkName + ".";
        };
    }

    static Drink MakeDrink(string name, Player player)
    {
        int price = 0;
        string description = "";
        switch(name)
        {
            case ("whiskey"):
                price = 3;
                description = "A glass of aged amber whiskey.";
                break;
            case ("tequila"):
                price = 5;
                description = "A glass of Mexican tequila.";
                break;
            case ("gin"):
                price = 5;
                description = "A glass of imported gin.";
                break;
            case ("moonshine"):
                price = 8;
                description = "A glass of ... you're not sure what. It reeks, though.";
                break;
            case ("bottle of whiskey"):
                price = 14;
                description = "A whole bottle of aged amber whiskey";
                break;
            default:
                break;
        }

        Drink drink = new Drink(name, player, price, description);
        drink.SetDitransitiveResponse("use", (tool) => {
            if (tool == "peanut")
            {
                player.RemoveFromInventory(name);
                player.RemoveFromInventory("peanut");

                GameObject poison = new GameObject("peanut " + name);
                poison.SetTransitiveResponse("what", () => {
                    return "A " + name + " with trace amounts of peanut in it.";
                });
                player.AddToInventory(poison);

                return "You break off the tiniest pieces of the peanut you can and slip them into the " + name + ".";
            }
            else
            {
                return null;
            }
        });
        return drink;
    }
}