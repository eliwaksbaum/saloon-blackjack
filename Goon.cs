using Algiers;
using System;

public class Goon : GameObject
{
    Random rand = new Random();
    string name;
    float odds;

    bool isTalking;
    public bool IsTalking => isTalking;
    int talkCount;
    bool isDead = false;
    public bool IsDead => isDead;

    public Goon(string name, float odds) : base(name)
    {
        this.name = Parser.CapitalizeFirst(name);
        UpdateOdds(odds);
        SetTransitiveResponse("shoot", Shoot);
    }

    string Shoot()
    {
        if (isTalking)
        {
            return "You see a moment of panic on " + name + "'s face but it doesn't last long. They join their coworkers on the floor.";
        }
        else
        {
            double roll = rand.NextDouble();
            if (roll > odds)
            {
                isDead = true;
                Delete();
                return "You take careful aim and " + name + " hits the floor.";
            }
            else
            {
                return "Crap. " + name + " got out of the way.";
            }
        }
    }

    public string Talk()
    {
        isTalking = true;
        talkCount++;
        switch (talkCount)
        {
            case 1:
                return "Okay, okay. What do you want to know?";
            case 2:
                return "Pitr? How should I know? That was years ago!";
            case 3:
                return "I swear! I swear! I'm a nobody ok, they've got me out here working this small-time outpost in the middle of nowhere. I don't know where they took him!";
            default:
                return "I don't know! I swear!";
        }
    }

    public void UpdateOdds(float newodds)
    {
        if (0 <= newodds && newodds < 1)
        {
            odds = newodds;
        }
        else
        {
            throw new Exception("A Goon's odds must be within [0, 1)");
        }
    }
}