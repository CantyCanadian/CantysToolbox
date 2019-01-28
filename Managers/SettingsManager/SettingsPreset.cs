using Canty;
using UnityEngine;
using UnityEditor;
using Canty.Managers;

public class SettingsPreset : ScriptableObject
{
    [Space(20)]

    // Screen
    public UnityEditor.AspectRatio AspectRatio = UnityEditor.AspectRatio.Aspect16by9;
    public int ResolutionIndex = 0;
    public bool Fullscreen = true;
    public int RefreshRate = 60;
    [Space(20)]

    // Graphics
    public int PixelLightCount = 4;
    public SettingsManager.TextureQualityTypes TextureQuality = SettingsManager.TextureQualityTypes.FullRes;
    public bool AnisotropicFiltering = true;
    public SettingsManager.AntiAliasingTypes AntiAliasing = SettingsManager.AntiAliasingTypes.MSAAx8;
    public bool SoftParticles = true;
    public bool RealtimeReflectionProbe = true;
    public int VSyncCount = 1;
    [Space(20)]

    // Shadows
    public ShadowQuality ShadowQualityType = ShadowQuality.All;
    public ShadowResolution ShadowResolutionType = ShadowResolution.VeryHigh;
    public ShadowProjection ShadowProjectionType = ShadowProjection.StableFit;
    public SettingsManager.ShadowDistanceTypes ShadowDistance = SettingsManager.ShadowDistanceTypes.Ultra;
    public ShadowmaskMode ShadowmaskModeType = ShadowmaskMode.DistanceShadowmask;
    public int ShadowNearPlaneOffset = 3;
    public SettingsManager.ShadowCascadeTypes ShadowCascadeType = SettingsManager.ShadowCascadeTypes.FourCascade;

    [MenuItem("Assets/Create/Canty/Managers/Settings Preset")]
    public static void CreateAsset()
    {
        EditorUtil.CreateScriptableObject<SettingsPreset>();
    }
}
