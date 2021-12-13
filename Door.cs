using Algiers;

public class Door : GameObject
{

    public Door(SPlayer player) : base("door")
    {

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
                if (input == player.DoorCode)
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

    
}