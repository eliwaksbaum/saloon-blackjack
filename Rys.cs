using Algiers;
using System.Collections.Generic;

public class Rys : GameObject
{
    public Rys(SPlayer player) : base("rys")
    {
        SetTransitiveResponse("look", () => {
            return " RYS sits at one of the tables, smirking, nursing his tequila.";
        });

        SetTransitiveResponse("what", () => {
            return "Rys. Motherfuckin. Lee. Not one bigger asshole (or asshole more allergic to nuts) in the solar system. You don't know" +
            " why he's here but you do know it means trouble. Rys knows your not a 'brother in arms' or whatever. You're going to have to do" +
            " something about this.";
        });

        SetTransitiveResponse("talk", () => {
            return "'I don't know what you're doing here, but I'm watching you.'";
        });

        SetDitransitiveResponse("give", (gift) => {
            List<string> drinks = new List<string> {"whiskey", "gin", "moonshine", "bottle of whiskey"};

            if (gift == "peanut")
            {
                return "'Very funny. Fuck off.'";
            }
            else if (gift == "peanut tequila")
            {
                player.RemoveFromInventory("peanut tequila");
                player.AddWaypoint("stage4");
                Delete();
                return "'Burying the hatchet? Humility looks bad on you, but I'll take the free tequila.'";
            }
            else if (gift == "tequila")
            {
                player.RemoveFromInventory("tequila");
                return "'Burying the hatchet? Humility looks bad on you, but I'll take the free tequila.'";
            }
            else if (drinks.Contains(gift))
            {
                return "'If you're trying to get me into bed, " + gift + "'s not my drink.'";
            }
            else if (gift.Split(" ").Length == 2 && gift.Split(" ")[0] == "peanut" && drinks.Contains(gift.Split(" ")[1]))
            {
                return "'If you're trying to get me into bed, " + gift + "'s not my drink.'";
            }
            else
            {
                string ind = Parser.StartsWithVowel(gift) ? "an" : "a";
                return "Rys isn't going to want " + ind + " " + gift + ".";
            }
        });

        SetDitransitiveResponse("show", (item) => {
            if (item == "signet")
            {
                return "Better not. Rys will know how you got that ring and might blow your cover.";
            }
            else
            {
                return "Rys doesn't seem interested in the " + item + ".";
            }
        });
    }
}