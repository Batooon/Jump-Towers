using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class GeneralPlatformFactory : PlatformFactory
{
    [SerializeField] private PlatformConfig _colorChangingPlatform, _formChangingPlatform, _rulesChangingPlatform, _startingPlatform;
    private int _lastColorIndex, _lastFormIndex;
    public override PlatformConfig GetConfig(PlatformType platformType)
    {
        return platformType switch
        {
            PlatformType.ColorChanging => _colorChangingPlatform,
            PlatformType.FormChanging => _formChangingPlatform,
            PlatformType.RulesChanging => _rulesChangingPlatform,
            PlatformType.StartingPlatform => _startingPlatform,
            _ => throw new ArgumentOutOfRangeException(nameof(platformType), platformType,
                $"No config for: {platformType}")
        };
    }

    public override PlatformArgs GetArgs(PlatformType platformType)
    {
        var args = new PlatformArgs();
        switch (platformType)
        {
            case PlatformType.ColorChanging:
            {
                var randomColorIndex = Random.Range(0, _playerSettings.Colors.Count);
                _lastColorIndex = randomColorIndex;
                args.PlatformColor = _playerSettings.Colors[randomColorIndex];
                args.Shape = (Shapes)_lastFormIndex;
                return args;
            }
            case PlatformType.FormChanging:
            {
                var randomShapeIndex = Random.Range(0, 2);
                _lastFormIndex = randomShapeIndex;
                args.Shape = (Shapes)randomShapeIndex;
                args.PlatformColor = _playerSettings.Colors[_lastColorIndex];
                return args;
            }
            case PlatformType.RulesChanging:
            {
                args.PlatformColor = _playerSettings.Colors[_lastColorIndex];
                args.Shape = (Shapes)_lastFormIndex;
                return args;
            }
            case PlatformType.StartingPlatform:
            {
                args.PlatformColor = _playerSettings.Colors[Random.Range(0, _playerSettings.Colors.Count)];
                args.Shape = Shapes.Square;
                return args;
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(platformType), platformType,
                    $"No args for: {platformType}");
        }
    }
}