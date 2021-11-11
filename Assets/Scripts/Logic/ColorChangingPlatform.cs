public class ColorChangingPlatform : Platform
{
    public override bool TryAccept(Player player)
    {
        //Если цвет игрока совпадает - return true;
        return true;
    }
}