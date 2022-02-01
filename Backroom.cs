using Algiers;

public class Backroom : Room
{
    public Backroom() : base("backroom")
    {
        GameObject can = new GameObject("can");
        can.SetTransitiveResponse("shoot", () => {return "clang";});
        AddObject(can);
    }
}