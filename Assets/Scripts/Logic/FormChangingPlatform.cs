public class FormChangingPlatform : Platform
{
    public override bool TryAccept(Player player)
    {
        _jumpedOn = true;
        return player.CurrentShape==_currentShape;
    }
}