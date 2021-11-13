using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public abstract class Platform : MonoBehaviour
{
    public event Action<Platform> OnDisappeared;
    
    [field: SerializeField]
    public Transform JumpPoint
    {
        get;
        private set;
    }

    [SerializeField] protected Renderer _renderer;

    protected bool _jumpedOn;

    public virtual bool TryAccept(Player player)
    {
        _jumpedOn = true;
        return true;
    }

    public virtual void SetArgs(PlatformArgs args)
    {
        _renderer.material.color = args.PlatformColor;
    }

    private void OnBecameInvisible()
    {
        if (_jumpedOn == false)
            return;
        gameObject.SetActive(false);
        _jumpedOn = false;
        OnDisappeared?.Invoke(this);
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
