using Algiers;
using System;

public class Backroom : Room
{
    State fightState = new State();
    State darkState = new State();
    Player player;

    Goon kwim = new Goon("kwim", 0.5f);
    Goon letta = new Goon("letta", 0.5f);
    Goon skinner = new Goon("skinner", 0.7f);

    public Backroom(Player player) : base("backroom")
    {
        description = "The room behind the door is pretty sparse. A single LIGHTBULB hangs from the ceiling, making it feel like a basement."
            + " The door back to the SALOON stands slightly ajar behind you; there's a door that must lead to a BACK ALLEY at the back.";

        this.player = player;
        player.State = fightState;
        World.GetWorld.AddTransitiveCommand("shoot", LightShoot, fightState, "Shoot what?");
        World.GetWorld.AddTransitiveCommand("shoot", DarkShoot, darkState, "Shoot what?");
        World.GetWorld.AddIntransitiveCommand("look", () => {return "You can't see anything with the light out, besides the sliver of light coming from door to the SALOON.";}, darkState);
        World.GetWorld.AddTransitiveCommand("enter", LightScram, fightState, "Enter where?");
        World.GetWorld.AddTransitiveCommand("enter", DarkScram, darkState, "Enter where?");

        GameObject can = new GameObject("can");
        can.SetTransitiveResponse("shoot", () => {return "clang";});
        AddObject(can);

        kwim.SetTransitiveResponse("what", () => {
            return "A skinny Brotherhood goon. You think they might have shot you before, or maybe that was Letta.";
        });
        kwim.SetTransitiveResponse("look", kLook);
        kwim.SetTransitiveResponse("talk", kTalk);
        AddObject(kwim);
        
        
        letta.SetTransitiveResponse("what", () => {
            return "A fat Brotherhood goon. You think they might have shot you before, or maybe that was Kwim.";
        });
        letta.SetTransitiveResponse("look", lLook);
        letta.SetTransitiveResponse("talk", lTalk);
        AddObject(letta);
        
        skinner.SetTransitiveResponse("what", () => {
            return "The boss in charge here. This one you've heard of. Supposedly he was there when they took Pitr.";
        });
        skinner.SetTransitiveResponse("look", sLook);
        skinner.SetTransitiveResponse("talk", sTalk);
        AddObject(skinner);

        GameObject cabinet = new GameObject("filing cabinet");
            cabinet.SetTransitiveResponse("what", () => {
                return "I guess the Brotherhood likes to keep their files in order.";
            });
        AddObject(cabinet);

        GameObject bulb = new GameObject("lightbulb");
        bulb.SetTransitiveResponse("what", () => {
            return "A dimly pulsing lightbulb. Keeping with the theme, it looks like it actually has a filament inside.";
        });
        bulb.SetTransitiveResponse("shoot", () => {
            player.State = darkState;
            return "The bulb shatters and the room goes pitch black.";
        });
        AddObject(bulb);
    }

    string kLook()
    {
        return "KWIM is standing in the left corner behind a FILING CABINET, just below a big VENT.";
    }
    string kTalk()
    {
        if (!(skinner.IsDead && letta.IsDead))
        {
            return "KWIM gives you a gesture that makes you think they're not interested in talking.";
        }
        else
        {
            return kwim.Talk();
        }
    }

    string lLook()
    {
        return "LETTA is crouching behind the FRIDGE on the right side of the room.";
    }
    string lTalk()
    {
        if (!(skinner.IsDead && kwim.IsDead))
        {
            return "LETTA gives you a gesture that makes you certain they're not interested in talking.";
        }
        else
        {
            return letta.Talk();
        }
    }

    string sLook()
    {
        return "SKINNER is just visible behind the COUCH.";
    }
    string sTalk()
    {
        if (!(kwim.IsDead && letta.IsDead))
        {
            return "'You're not talking your way out of this dickhead. I'm taking that ring back from your cold dead hands.'";
        }
        else
        {
            return skinner.Talk();
        }
    }

    void UseAmmo()
    {
        int ammo = player.IncrementCounter("ammo", -1);
        string count = "";
        if (ammo == 0)
        {
            count = "Fuck.";
        }
        else
        {
            count = ammo == 1 ? " shot left." : " shots left.";
            count = ammo + count;
        }
        Parser.GetParser.AddAfterword(count);
    }

    string LightShoot(string target)
    {
        if (!player.InRoom(target))
        {
            return "There's no " + target + " here to shoot.";
        }
        else
        {
            GameObject targetObj = player.GetFromRoom(target);
            Func<string> shoot = targetObj.GetTransitiveResponse("shoot");
            if (shoot == null)
            {
                return "You can't shoot the " + target + ".";
            }
            else
            {
                UseAmmo();
                string response = shoot();
                return response == null ? "The bullet ricochets off the " + target + "." : response;
            }
        }
    }

    string DarkShoot(string target)
    {
        UseAmmo();
        return "You try to aim for " + target + ", but it's pitch black in here. You don't think you hit anything.";
    }

    string LightScram(string exit)
    {
        string survivor;
        if (kwim.IsTalking)
        {
            survivor = "Kwim";
        }
        else if (letta.IsTalking)
        {
            survivor = "Letta";
        }
        else if (skinner.IsTalking)
        {
            survivor = "Skinner";
        }
        else if (kwim.IsDead && letta.IsDead && skinner.IsDead)
        {
            survivor = "none";
        }
        else
        {
            survivor = "notsafe";
        }

        if (exit == "back alley")
        {
            switch (survivor)
            {
                case "notsafe":
                    return "You don't think you could make it to the alley door safely.";
                case "none":
                    player.AddWaypoint("endA");
                    return "You leave the bodies behind and dash out to the alley.";
                default:
                    player.AddWaypoint("endA");
                    return "You leave " + survivor + " behind and dash out to the alley.";
            }
        }
        else if (exit == "saloon")
        {
            switch (survivor)
            {
                case "notsafe":
                    return "You don't think you could make it to the saloon door safely.";
                case "none":
                    return "Probably best to leave out the back.";
                default:
                    return "You don't want to turn your back to " + survivor + ". Probably best to leave out the back.";
            }
        }
        else
        {
            return "There's no room named " + exit + " here to enter.";
        }
    }

    string DarkScram(string exit)
    {
        if (exit == "saloon")
        {
            player.AddWaypoint("endB");
            return "You slip back into the saloon as quietly as possible and then book it to the door.";
        }
        else if (exit == "back alley")
        {
            return "You don't think you could sneak all the way to the alley door.";
        }
        else
        {
            return "There's no room named " + exit + " here to enter.";
        }
    }

    //TODO:

        //Shooting the vent freaks kwim out, nab 'em. I guess that means we need a way to change a goon's odds

        //Shooting other objects does stuff?

        //IDK if it goes in this class, but if you run out of bullets you die. Or maybe if you shoot the wrong thing it blows up?
            //then should you start over from scratch or just the fight?
                //just the fight is actually so doable. just reset ammo and make a new backroom object
}