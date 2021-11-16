public class RulesChangingPlatfrom : Platform
{
    public override bool TryAccept(Player player)
    {
        _jumpedOn = true;
        player.ChangeRules();
        player.Renderer.material.color = _renderer.material.color;
        return true;
    }
}