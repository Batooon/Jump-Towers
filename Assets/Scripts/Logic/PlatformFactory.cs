using UnityEngine;

public abstract class PlatformFactory : MonoBehaviour
{
    public abstract PlatformConfig GetConfig(PlatformType platformType);
}