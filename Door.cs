using Algiers;
using System;

public class Door : GameObject
{
    string code;
    Random rand = new Random();

    public Door(Player player) : base("door")
    {
        code = SetCode();
        Console.Write(code);

        SetTransitiveResponse("what", () => {
            return "A heavy metal door with a sign that says 'Staff Only'. It's locked with a keypad.";
        });

        SetTransitiveResponse("open", Open);
        string Open()
        {
            Parser.GetParser.GoRaw(Keypad);
            return "Enter the three-digit code for the door:";
        }

        string Keypad(string input)
        {
            int value = -1;
            if ((input == "000") || (int.TryParse(input, out value) && value > 0 && value < 1000))
            {
                Parser.GetParser.GoStandard();
                if (input == code)
                {
                    return "You're in!";
                }
                else
                {
                    return "The door stays shut. That must not be the right code.";
                }
            }
            else
            {
                return "Enter three digits 0-9:";
            }
        }
    }

    string SetCode()
    {
        string[] code = new string[3];
        string[,] codes = new string[,] {{"7","7","7"}, {"6","8","7"}, {"5","9","7"}, {"5","8","8"}, {"4","8","9"}, {"3","9","9"}};
        int setI = rand.Next(6);
        int firstI = rand.Next(3);

        code[0] = codes[setI, firstI];
        code[1] = codes[setI, (firstI + 1)%3];
        code[2] = codes[setI, (firstI + 2)%3];
        return code[0]+code[1]+code[2];
    }
}