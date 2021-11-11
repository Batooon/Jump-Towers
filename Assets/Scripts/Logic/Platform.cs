using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public abstract class Platform : MonoBehaviour
{
    [field: SerializeField]
    public Transform JumpPoint
    {
        get;
        private set;
    }

    public abstract bool TryAccept(Player player);
    
    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
}

public class StartingPlatform : Platform
{
    public override bool TryAccept(Player player)
    {
        return true;
    }
}

public class FormChangingPlatform : Platform
{
    public override bool TryAccept(Player player)
    {
        //Если форма игрока совпадает - return true;
        return true;
    }
}

public abstract class PlayerRandomizer
{
    protected readonly Player _player;
    protected readonly PlayerSettings _playerSettings;

    protected PlayerRandomizer(Player player, PlayerSettings playerSettings)
    {
        _player = player;
        _playerSettings = playerSettings;
    }

    public abstract void Randomize();
}

public class ColorRandomizer : PlayerRandomizer
{
    private int _counter;
    public ColorRandomizer(Player player, PlayerSettings playerSettings) : base(player, playerSettings)
    {
    }
    
    public override void Randomize()
    {
        var colors = _playerSettings.Colors;
        _counter %= colors.Count;
        _player.Renderer.material.color = colors[_counter++];
    }
}

[Serializable]
public class PlayerSettings
{
    public List<Color> Colors;
}
