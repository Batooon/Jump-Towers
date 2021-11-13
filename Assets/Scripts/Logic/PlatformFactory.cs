using UnityEngine;

public abstract class PlatformFactory : MonoBehaviour
{
    protected PlayerSettings _playerSettings;

    public void Init(PlayerSettings playerSettings)
    {
        _playerSettings = playerSettings;
    }
    
    public abstract PlatformConfig GetConfig(PlatformType platformType);
    public abstract PlatformArgs GetArgs(PlatformType platformType);
}