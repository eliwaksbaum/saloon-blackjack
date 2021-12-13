using Algiers;
using System.Collections.Generic;

public class Jukebox : GameObject
{
    static Jukebox singleton;

    public Jukebox(SPlayer player) : base("jukebox")
    {
        singleton = this;
        SetCondition("on", false);
        SetCondition("playing", false);

        SetTransitiveResponse("what", () => {
            if (GetCondition("playing"))
            {
                return "It's a very old music player. It's playing the song you chose. You kind of like it.";
            }
            else if (GetCondition("on"))
            {
                return "It's a very old music player. You can play some music if you CHOOSE A SONG.";
            }
            else
            {
                return "An ancient looking thing. You've heard of them, they play music. You're not sure how to" +
                " get it going though. You see a little slot, but what would go in it?";
            }
        });

        SetDitransitiveResponse("use", (tool) => {
            if (tool == "quarter")
            {
                player.RemoveFromInventory("quarter");
                SetCondition("on", true);
                return "The jukebox whirs to life.";
            }
            else
            {
                return null;
            }
        });
    }

    public static string ChooseSong()
    {
        if (singleton.GetCondition("playing"))
        {
            return "You'd need another one of those coins to play another song.";
        }
        else if (singleton.GetCondition("on"))
        {
            Parser.GetParser.GoRaw(SongSelection);
            return "There's a small keypad on the machine. It looks like to pick a song, you need to press a number 0-9 and a letter A-J.";
        }
        else
        {
            return "You'll have to figure out how to turn the jukebox on before you can use it.";
        }
    }

    static string SongSelection(string code)
    {
        List<string> numbers = new List<string> {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9"};
        List<string> letters = new List<string> {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J"};
        code = code.ToUpper();
        if (code.Length == 2 && numbers.Contains(code.Substring(0,1)) && letters.Contains(code.Substring(1)))
        {
            Parser.GetParser.GoStandard();
            if (code == "2H")
            {
                singleton.SetCondition("playing", true);
                return "With a 'kachunk', the old speakers start putting out a tune you don't recognize.";
            }
            else
            {
                return "Huh. That didn't seem to do anything. Some of the songs must be broken.";
            }
        }
        else
        {
            return "To play a song, you need to type a number 0-9 and a letter A-J.";
        }
    }
}