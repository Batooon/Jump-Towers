using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Deform;

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
    [SerializeField] private CylindrifyDeformer _deformer;

    protected bool _jumpedOn;
    protected Shapes _currentShape;

    public virtual bool TryAccept(Player player)
    {
        _jumpedOn = true;
        return true;
    }

    public virtual void SetArgs(PlatformArgs args)
    {
        _renderer.material.color = args.PlatformColor;
        UpdateShape(args.Shape);
    }

    private void OnBecameInvisible()
    {
        if (_jumpedOn == false)
            return;
        gameObject.SetActive(false);
        _jumpedOn = false;
        OnDisappeared?.Invoke(this);
    }

    private void UpdateShape(Shapes newShape)
    {
        if(_currentShape==newShape)
            return;
        _currentShape = newShape;
        switch (_currentShape)
        {
            case Shapes.Circle:
                UpdateShape(1f); 
            break;
            case Shapes.Square:
                UpdateShape(0f); 
            break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void UpdateShape(float endValue)
    {
        _deformer.Factor=endValue;
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

public class ShapeRandomizer : PlayerRandomizer
{
    private int _counter;
    private float _shapeChangeDuration = 0.1f;
    
    public ShapeRandomizer(Player player, PlayerSettings playerSettings) : base(player, playerSettings)
    {
    }

    public override void Randomize()
    {
        var newShape = (Shapes)_counter++;
        _player.SetNewShape(newShape);
        switch (newShape)
        {
            case Shapes.Circle:
                ChangeShape(1f);
                break;
            case Shapes.Square:
                ChangeShape(0f);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        _counter %= 2;
    }

    private void ChangeShape(float endValue)
    {
        DOTween.To(()=>_player.Deformer.Factor, x=>_player.Deformer.Factor=x, endValue, _shapeChangeDuration);
    }
}

[Serializable]
public class PlayerSettings
{
    public List<Color> Colors;
    public PlatformType StartingRules;
}

public enum Shapes
{
    Square = 0,
    Circle = 1
}
