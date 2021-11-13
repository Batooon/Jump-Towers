using System;
using UnityEngine;

public class GeneralPlatformFactory : PlatformFactory
{
    [SerializeField] private PlatformConfig _colorChangingPlatform, _formChangingPlatform, _rulesChangingPlatform;
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
}