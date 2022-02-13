using Algiers;

public class Door : GameObject
{
    SPlayer player;

    public Door(SPlayer player) : base("door")
    {
        this.player = player;

        SetTransitiveResponse("what", () => {
            return "A heavy metal door with a sign that says 'Staff Only'. It's locked with a keypad.";
        });

        SetTransitiveResponse("open", () => {
            Parser.GetParser.GoRaw(Keypad);
            return "Enter the three-digit code for the door:";
        });
    }

    string Keypad(string input)
    {
        int value = -1;
        if ((input == "000") || (int.TryParse(input, out value) && value > 0 && value < 1000))
        {
            Parser.GetParser.GoStandard();
            if (input == player.DoorCode)
            {
                player.AddWaypoint("stage5");
                player.current_room.OnExit();
                player.current_room = new Backroom(player);
                return Backroom.on_enter;
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