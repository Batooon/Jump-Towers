using UnityEngine;

public class ColorChangingPlatform : Platform
{
    private Color _color;
    
    public override bool TryAccept(Player player)
    {
        _jumpedOn = true;
        return player.Renderer.material.color == _color;
    }

    public override void SetArgs(PlatformArgs args)
    {
        base.SetArgs(args);
        _color = args.PlatformColor;
    }
}