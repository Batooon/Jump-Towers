using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class GeneralPlatformFactory : PlatformFactory
{
    [SerializeField] private PlatformConfig _colorChangingPlatform, _formChangingPlatform, _rulesChangingPlatform;
    private int _lastColorIndex, _lastFormIndex;
    public override PlatformConfig GetConfig(PlatformType platformType)
    {
        return platformType switch
        {
            PlatformType.ColorChanging => _colorChangingPlatform,
            PlatformType.FormChanging => _formChangingPlatform,
            PlatformType.RulesChanging => _rulesChangingPlatform,
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
                return args;
            }
            case PlatformType.FormChanging:
            {
                args.PlatformColor = _playerSettings.Colors[_lastColorIndex];
                return args;
            }
            case PlatformType.RulesChanging:
            {
                args.PlatformColor = _playerSettings.Colors[_lastColorIndex];
                return args;
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(platformType), platformType,
                    $"No args for: {platformType}");
        }
    }
}