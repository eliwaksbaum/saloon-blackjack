using Algiers;
using System;
using System.Collections.Generic;

public class BlackJack
{
    List<int> dealer;
    int dealerScore {get{return TotalScore(dealer);}}

    List<int> better;
    int playerScore {get{return TotalScore(better);}}

    int[] dealt;
    Random rand = new Random();
    int bet;

    static State inOrOutState = new State();
    static State hitOrStayState = new State();
    public static State blackjackState = inOrOutState.Compose(hitOrStayState);
    
    static Command hit;
    static Command stay;
    static Command play;
    static Command leave;

    public string instructions;
    State saloonState;
    World world;
    SPlayer player;

    public static void AddCommands(World world)
    {
        hit = world.AddIntransitiveCommand("hit", null, hitOrStayState);
        stay = world.AddIntransitiveCommand("stay", null, hitOrStayState);
        play = world.AddIntransitiveCommand("play", null, inOrOutState);
        leave = world.AddIntransitiveCommand("leave", null, inOrOutState);
    }

    public BlackJack(World _world)
    {
        world = _world;
        player = (SPlayer) world.player;
        saloonState = world.player.State;

        world.SetIntransitiveResponse(hit, Hit);
        world.SetIntransitiveResponse(stay, Stay);
        world.SetIntransitiveResponse(play, Start);
        world.SetIntransitiveResponse(leave, Leave);
    }

    string Hit()
    {
        int newCard;
        if (player.HasWaypoint("stage4"))
        {
            newCard = RiggedDeal(player.DoorCode.Substring(2));
            player.AddWaypoint("hascode");
        }
        else
        {
            newCard = Deal();
        }
        better.Add(newCard);

        if (player.HasWaypoint("stage4"))
        {
            return "The dealer places a " + CardName(newCard) + " on the table and gives you the briefest glance. Better not hit again." +
                "\n" + Stay();
        }
        else
        {
            string ending;
            if (playerScore > 21)
            {
                player.State = inOrOutState;
                ending = "You're over 21!\n" + PayOut(false) + "\nPlay again or leave?";
            }
            else
            {
                ending = Announce() + "\nWould you like to hit or stay?";
            }
            return "The dealer places a " + CardName(newCard) + " on the table.\n" + ending;
        }
    }

    string Stay()
    {
        string dealerPlay = DealersPlay();
        player.State = inOrOutState;
        if (dealerScore > 21)
        {
            return dealerPlay + "The dealer's over 21!\n" + PayOut(true) + "\nPlay again or leave?";
        }
        else
        {
            string result;
            if (playerScore == dealerScore)
            {
                result = "It's a tie. You keep your wager.";
            }
            else
            {
                result = PayOut(playerScore > dealerScore);
            }
            return dealerPlay + "The dealer has " + dealerScore + " and you have " + playerScore +
                ".\n" + result + "\nPlay again or leave?";
        }
    }

    string Leave()
    {
        if (!player.HasWaypoint("firstgame"))
        {
            player.AddWaypoint("firstgame");
        }
        player.State = saloonState;
        return "You stand up from the game table. You've had enough BlackJack for now.";
    }

    string PayOut(bool didWin)
    {
        string result = didWin ? "You win " : "You lose ";
        int earn = didWin ? bet : -bet;
        player.IncrementCounter("money", earn);
        result += bet + "Ð! Now you have " + player.GetCounter("money") + "Ð total.";
        return result;
    }

    string Announce()
    {
        string cards = "";
        foreach (int card in better)
        {
            cards += " " + CardName(card);
        }
        return "The dealer's face up card is " + CardName(dealer[0]) + ". Your cards are" + cards + ".";
    }

    public string Start()
    {
        if (player.GetCounter("money") == 0)
        {
            player.State = saloonState;
            return "You gotta have money to play BlackJack.";
        }
        else
        {
            Parser.GetParser.GoRaw(TakeBet);
            return "How much would you like to bet?";
        }
    }

    string FirstDeal()
    {
        string message = "You bet " + bet + "Ð.\nThe dealer shuffles and deals.\n";

        dealt = new int[13];
        dealer = new List<int>();
        better = new List<int>();

        dealer.Add(Deal());
        dealer.Add(Deal());

        if (player.HasWaypoint("stage4"))
        {
            better.Add(RiggedDeal(player.DoorCode.Substring(0,1)));
            better.Add(RiggedDeal(player.DoorCode.Substring(1,1)));
        }
        else
        {
            better.Add(Deal());
            better.Add(Deal());
        }

        message += Announce();
        if (playerScore == 21)
        {
            message += "\nBlackjack! " + PayOut(true) + "\nPlay again or leave?";
        }
        else
        {
            player.State = hitOrStayState;
            message += "\nWould you like to hit or stay?";
        }
        return message;
    }

    string TakeBet(string input)
    {
        if (int.TryParse(input, out bet) && bet > 0)
        {
            if (bet <= player.GetCounter("money"))
            {
                Parser.GetParser.GoStandard();
                return FirstDeal();
            }
            else
            {
                return "You only have " + player.GetCounter("money") + "D to wager.";
            }
        }
        else
        {
            return "Please type a number greater than 0 for your bet.";
        }
    }

    string DealersPlay()
    {
        string readoff = dealerScore < 17? ". They draw " : "";
        while (dealerScore < 17)
        {
            int newCard = Deal();
            dealer.Add(newCard);
            readoff += CardName(newCard) + " ";
        }
        return "The dealer's cards are " + CardName(dealer[0]) + " and " + CardName(dealer[1]) +
            readoff + ".\n";
    }

    int Deal()
    {
        int draw = rand.Next(1, 14);
        if (dealt[draw-1] < 4)
        {
            dealt[draw-1] += 1;
            return draw;
        }
        else
        {
            return Deal();
        }
    }

    int RiggedDeal(string rig)
    {
        int draw;
        int.TryParse(rig, out draw);
        dealt[draw-1] += 1;
        return draw;
    }

    string CardName(int value)
    {
        if (value < 1 || value > 13)
        {
            throw new Exception("A card's value must be between 1 and 13.");
        }
        else if (value >= 2 && value <= 10)
        {
            return value.ToString();
        }
        else
        {
            switch(value)
            {
                case 1:
                    return "Ace";
                case 11:
                    return "Jack";
                case 12:
                    return "Queen";
                case 13:
                    return "King";
                default:
                    return "";
            }
        }
    }

    int CardScore(int card)
    {
        if (card == 1)
        {
            return 11;
        }
        if (card > 10)
        {
            return 10;
        }
        else
        {
            return card;
        }
    }

    int TotalScore(List<int> cards)
    {
        int score = 0;
        if (!cards.Contains(1))
        {
            foreach(int card in cards)
            {
                score += CardScore(card);
            }
        }
        else
        {
            List<int> copy = new List<int>(cards);
            int aces = copy.RemoveAll((x) => x == 1);
            foreach(int card in copy)
            {
                score += CardScore(card);
            }
            for (int i = 0; i < aces; i++)
            {
                int value = score + 11 + (aces - i - 1) <= 21 ? 11 : 1;
                score += value;
            }
        }
        return score;
    }
}