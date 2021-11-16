public class StartingPlatform : Platform
{
    public override bool TryAccept(Player player)
    {
        player.SetupCube();
        _jumpedOn = true;
        return true;
    }
}