public class FormChangingPlatform : Platform
{
    public override bool TryAccept(Player player)
    {
        _jumpedOn = true;
        //Если форма игрока совпадает - return true;
        return true;
    }
}