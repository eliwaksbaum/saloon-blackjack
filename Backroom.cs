using Algiers;
using System;

public class Backroom : Room
{
    State darkState = new State();

    Goon kwim = new Goon("kwim", 0.0f);
    Goon letta = new Goon("letta", 0.5f);
    Goon skinner = new Goon("skinner", 0.0f);

    public Backroom() : base("backroom")
    {
        description = "The room behind the door is pretty sparse. A single LIGHTBULB hangs from the ceiling, making it feel like a basement.";

        GameObject can = new GameObject("can");
        can.SetTransitiveResponse("shoot", () => {return "clang";});
        AddObject(can);

        kwim.SetTransitiveResponse("what", () => {
            return "A skinny Brotherhood goon. You think they might have shot you before, or maybe that was Letta.";
        });
        AddObject(kwim);
        
        
        letta.SetTransitiveResponse("what", () => {
            return "A fat Brotherhood goon. You think they might have shot you before, or maybe that was Kwim.";
        });
        AddObject(letta);
        
        skinner.SetTransitiveResponse("what", () => {
            return "The boss in charge here. This one you've heard of. Supposedly he was there when they took Pitr.";
        });
        AddObject(skinner);

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
}