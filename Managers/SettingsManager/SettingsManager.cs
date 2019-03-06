///====================================================================================================
///
///     SettingsManager by
///     - CantyCanadian
///
///====================================================================================================

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Canty.Managers
{
    public class SettingsManager : Singleton<SettingsManager>
    {
        #region Public Properties

        public PresetDictionary Presets;

        public bool UsePresetAsDefault = false;
        [Space(10)]
        [ConditionalField("UsePresetAsDefault")] public string PresetUsedAsDefault;

        // Screen
        [ConditionalField("UsePresetAsDefault", true)] public AspectRatio AspectRatio = AspectRatio.Aspect16by9;
        [ConditionalField("UsePresetAsDefault", true)] public int ResolutionIndex = -1;
        [ConditionalField("UsePresetAsDefault", true)] public bool Fullscreen = true;
        [ConditionalField("UsePresetAsDefault", true)] public int RefreshRate = 60;

        // Graphics
        [ConditionalField("UsePresetAsDefault", true)] public int PixelLightCount = 4;
        [ConditionalField("UsePresetAsDefault", true)] public TextureQualityTypes TextureQuality = TextureQualityTypes.FullRes;
        [ConditionalField("UsePresetAsDefault", true)] public bool AnisotropicFiltering = true;
        [ConditionalField("UsePresetAsDefault", true)] public AntiAliasingTypes AntiAliasing = AntiAliasingTypes.MSAAx8;
        [ConditionalField("UsePresetAsDefault", true)] public bool SoftParticles = true;
        [ConditionalField("UsePresetAsDefault", true)] public bool RealtimeReflectionProbe = true;
        [ConditionalField("UsePresetAsDefault", true)] public int VSyncCount = 1;

        // Shadows
        [ConditionalField("UsePresetAsDefault", true)] public ShadowQuality ShadowQualityType = ShadowQuality.All;
        [ConditionalField("UsePresetAsDefault", true)] public ShadowResolution ShadowResolutionType = ShadowResolution.VeryHigh;
        [ConditionalField("UsePresetAsDefault", true)] public ShadowProjection ShadowProjectionType = ShadowProjection.StableFit;
        [ConditionalField("UsePresetAsDefault", true)] public ShadowDistanceTypes ShadowDistance = ShadowDistanceTypes.Ultra;
        [ConditionalField("UsePresetAsDefault", true)] public ShadowmaskMode ShadowmaskModeType = ShadowmaskMode.DistanceShadowmask;
        [ConditionalField("UsePresetAsDefault", true)] public int ShadowNearPlaneOffset = 3;
        [ConditionalField("UsePresetAsDefault", true)] public ShadowCascadeTypes ShadowCascadeType = ShadowCascadeTypes.FourCascade;

        // Other
        public Dictionary<AspectRatio, Vector2Int[]> ResolutionData { get { return m_ResolutionList; } }

        #endregion

        #region Private Properties

        // Screen
        private UnityEditor.AspectRatio m_CurrentAspectRatio;
        private int m_CurrentResolutionIndex;
        private bool m_CurrentlyFullscreen;
        private int m_CurrentRefreshRate;

        // Graphics
        private int m_CurrentPixelLightCount;
        private TextureQualityTypes m_CurrentTextureQualityType;
        private bool m_CurrentAnisotropicFiltering;
        private AntiAliasingTypes m_CurrentAntiAliasingType;
        private bool m_CurrentSoftParticles;
        private bool m_CurrentRealtimeReflectionProbe;
        private int m_CurrentVSyncCount;

        // Shadows
        private ShadowQuality m_CurrentShadowQualityType;
        private ShadowResolution m_CurrentShadowResolutionType;
        private ShadowProjection m_CurrentShadowProjectionType;
        private ShadowDistanceTypes m_CurrentShadowDistanceType;
        private ShadowmaskMode m_CurrentShadowmaskModeType;
        private int m_CurrentShadowNearPlaneOffset;
        private ShadowCascadeTypes m_CurrentShadowCascadeType;

        // Other
        private Dictionary<UnityEditor.AspectRatio, Vector2Int[]> m_ResolutionList = null;

        #endregion

        #region Setting Types

        public enum TextureQualityTypes
        {
            // Best to worst
            FullRes = 0,
            HalfRes = 1,
            QuarterRes = 2,
            EighthRes = 3
        }

        public enum AntiAliasingTypes
        {
            // Worst to best
            Disabled = 0,
            MSAAx2 = 2,     // Multi Sampling x2
            MSAAx4 = 4,     // Multi Sampling x4
            MSAAx8 = 8      // Multi Sampling x8
        }

        public enum ShadowDistanceTypes // Values made up considering that default is 150. Change if desired.
        {
            // Worst to best
            Low = 0,
            Medium = 50,
            High = 100,
            Ultra = 150
        }

        public enum ShadowCascadeTypes
        {
            // Worst to best
            NoCascade = 0,
            TwoCascade = 2,
            FourCascade = 4
        }

        public void ApplyChanges(bool overwrite = false)
        {
            // Screen
            if (overwrite || (m_CurrentResolutionIndex != ResolutionIndex || m_CurrentAspectRatio != AspectRatio || m_CurrentlyFullscreen != Fullscreen || m_CurrentRefreshRate != RefreshRate))
            {
                m_CurrentAspectRatio = AspectRatio;
                m_CurrentResolutionIndex = ResolutionIndex;
                m_CurrentlyFullscreen = Fullscreen;
                m_CurrentRefreshRate = RefreshRate;
                Vector2Int resolution = m_ResolutionList[m_CurrentAspectRatio][m_CurrentResolutionIndex];
                Screen.SetResolution(resolution.x, resolution.y, m_CurrentlyFullscreen, m_CurrentRefreshRate);

                PlayerPrefsUtil.SetEnum("SETTINGSMANAGER_ASPECTRATIO", m_CurrentAspectRatio);
                PlayerPrefs.SetInt("SETTINGSMANAGER_RESOLUTIONINDEX", m_CurrentResolutionIndex);
                PlayerPrefsUtil.SetBool("SETTINGSMANAGER_FULLSCREEN", m_CurrentlyFullscreen);
                PlayerPrefs.GetInt("SETTINGSMANAGER_REFRESHRATE", m_CurrentRefreshRate);
            }

            // Graphics
            if (overwrite || (m_CurrentPixelLightCount != PixelLightCount))
            {
                m_CurrentPixelLightCount = PixelLightCount;
                QualitySettings.pixelLightCount = m_CurrentPixelLightCount;

                PlayerPrefs.SetInt("SETTINGSMANAGER_PIXELLIGHTCOUNT", m_CurrentPixelLightCount);
            }

            if (overwrite || (m_CurrentTextureQualityType != TextureQuality))
            {
                m_CurrentTextureQualityType = TextureQuality;
                QualitySettings.masterTextureLimit = (int)m_CurrentTextureQualityType;

                PlayerPrefsUtil.SetEnum("SETTINGSMANAGER_TEXTUREQUALITYTYPE", m_CurrentTextureQualityType);
            }

            if (overwrite || (m_CurrentAnisotropicFiltering != AnisotropicFiltering))
            {
                m_CurrentAnisotropicFiltering = AnisotropicFiltering;
                QualitySettings.anisotropicFiltering = m_CurrentAnisotropicFiltering ? UnityEngine.AnisotropicFiltering.Enable : UnityEngine.AnisotropicFiltering.Disable;

                PlayerPrefsUtil.SetBool("SETTINGSMANAGER_ANISOTROPICFILTERING", m_CurrentAnisotropicFiltering);
            }

            if (overwrite || (m_CurrentAntiAliasingType != AntiAliasing))
            {
                m_CurrentAntiAliasingType = AntiAliasing;
                QualitySettings.antiAliasing = (int)m_CurrentAntiAliasingType;

                PlayerPrefsUtil.SetEnum("SETTINGSMANAGER_ANTIALIASINGTYPE", m_CurrentAntiAliasingType);
            }

            if (overwrite || (m_CurrentSoftParticles != SoftParticles))
            {
                m_CurrentSoftParticles = SoftParticles;
                QualitySettings.softParticles = m_CurrentSoftParticles;

                PlayerPrefsUtil.SetBool("SETTINGSMANAGER_SOFTPARTICLES", m_CurrentSoftParticles);
            }

            if (overwrite || (m_CurrentRealtimeReflectionProbe != RealtimeReflectionProbe))
            {
                m_CurrentRealtimeReflectionProbe = RealtimeReflectionProbe;
                QualitySettings.realtimeReflectionProbes = m_CurrentRealtimeReflectionProbe;

                PlayerPrefsUtil.SetBool("SETTINGSMANAGER_REALTIMEREFLECTIONPROBE", m_CurrentRealtimeReflectionProbe);
            }

            if (overwrite || (m_CurrentVSyncCount != VSyncCount))
            {
                m_CurrentVSyncCount = VSyncCount;
                QualitySettings.vSyncCount = VSyncCount;

                PlayerPrefs.SetInt("SETTINGSMANAGER_VSYNCCOUNT", m_CurrentVSyncCount);
            }

            // Shadows
            if (overwrite || (m_CurrentShadowQualityType != ShadowQualityType))
            {
                m_CurrentShadowQualityType = ShadowQualityType;
                QualitySettings.shadows = m_CurrentShadowQualityType;

                PlayerPrefsUtil.SetEnum("SETTINGSMANAGER_SHADOWQUALITYTYPE", m_CurrentShadowQualityType);
            }

            if (overwrite || (m_CurrentShadowResolutionType != ShadowResolutionType))
            {
                m_CurrentShadowResolutionType = ShadowResolutionType;
                QualitySettings.shadowResolution = m_CurrentShadowResolutionType;

                PlayerPrefsUtil.SetEnum("SETTINGSMANAGER_SHADOWRESOLUTIONTYPE", m_CurrentShadowQualityType);
            }

            if (overwrite || (m_CurrentShadowProjectionType != ShadowProjectionType))
            {
                m_CurrentShadowProjectionType = ShadowProjectionType;
                QualitySettings.shadowProjection = m_CurrentShadowProjectionType;

                PlayerPrefsUtil.SetEnum("SETTINGSMANAGER_SHADOWPROJECTIONTYPE", m_CurrentShadowProjectionType);
            }

            if (overwrite || (m_CurrentShadowDistanceType != ShadowDistance))
            {
                m_CurrentShadowDistanceType = ShadowDistance;
                QualitySettings.shadowDistance = (int)m_CurrentShadowDistanceType;

                PlayerPrefsUtil.SetEnum("SETTINGSMANAGER_SHADOWDISTANCETYPE", m_CurrentShadowProjectionType);
            }

            if (overwrite || (m_CurrentShadowmaskModeType != ShadowmaskModeType))
            {
                m_CurrentShadowmaskModeType = ShadowmaskModeType;
                QualitySettings.shadowmaskMode = m_CurrentShadowmaskModeType;

                PlayerPrefsUtil.SetEnum("SETTINGSMANAGER_SHADOWMASKMODE", m_CurrentShadowmaskModeType);
            }

            if (overwrite || (m_CurrentShadowNearPlaneOffset != ShadowNearPlaneOffset))
            {
                m_CurrentShadowNearPlaneOffset = ShadowNearPlaneOffset;
                QualitySettings.shadowNearPlaneOffset = m_CurrentShadowNearPlaneOffset;

                PlayerPrefs.SetInt("SETTINGSMANAGER_SHADOWNEARPLANEOFFSET", m_CurrentShadowNearPlaneOffset);
            }

            if (overwrite || (m_CurrentShadowCascadeType != ShadowCascadeType))
            {
                m_CurrentShadowCascadeType = ShadowCascadeType;
                QualitySettings.shadowCascades = (int)m_CurrentShadowCascadeType;

                PlayerPrefsUtil.SetEnum("SETTINGSMANAGER_SHADOWCASCADETYPE", m_CurrentShadowCascadeType);
            }
        }

        public void LoadValues(SettingsPreset preset)
        {
            PixelLightCount = preset.PixelLightCount;
            TextureQuality = preset.TextureQuality;
            AnisotropicFiltering = preset.AnisotropicFiltering;
            AntiAliasing = preset.AntiAliasing;
            SoftParticles = preset.SoftParticles;
            RealtimeReflectionProbe = preset.RealtimeReflectionProbe;
            VSyncCount = preset.VSyncCount;

            ShadowQualityType = preset.ShadowQualityType;
            ShadowResolutionType = preset.ShadowResolutionType;
            ShadowProjectionType = preset.ShadowProjectionType;
            ShadowDistance = preset.ShadowDistance;
            ShadowmaskModeType = preset.ShadowmaskModeType;
            ShadowNearPlaneOffset = preset.ShadowNearPlaneOffset;
            ShadowCascadeType = preset.ShadowCascadeType;
        }

        public void LoadValues(SettingsPreset preset, bool loadResolution)
        {
            if (loadResolution)
            {
                ResolutionIndex = PlayerPrefs.GetInt("SETTINGSMANAGER_RESOLUTIONINDEX", ResolutionIndex);

                if (ResolutionIndex <= -1)
                {
                    Vector2Int currentResolution = new Vector2Int(Screen.currentResolution.width, Screen.currentResolution.height);

                    bool exit = false;
                    foreach (KeyValuePair<AspectRatio, Vector2Int[]> aspectRatio in m_ResolutionList)
                    {
                        for (int i = 0; i < aspectRatio.Value.Length; i++)
                        {
                            if (currentResolution == aspectRatio.Value[i])
                            {
                                ResolutionIndex = i;
                                AspectRatio = aspectRatio.Key;
                                exit = true;
                                break;
                            }
                        }

                        if (exit)
                        {
                            break;
                        }
                    }

                    if (ResolutionIndex == -1)
                    {
                        // Standard 1080p
                        ResolutionIndex = 2;
                        AspectRatio = AspectRatio.Aspect16by9;
                    }
                }
                else
                {
                    AspectRatio = PlayerPrefsUtil.GetEnum("SETTINGSMANAGER_ASPECTRATIO", AspectRatio);
                }

                Fullscreen = PlayerPrefsUtil.GetBool("SETTINGSMANAGER_FULLSCREEN", Fullscreen);
                RefreshRate = PlayerPrefs.GetInt("SETTINGSMANAGER_REFRESHRATE", RefreshRate);
            }

            PixelLightCount = preset.PixelLightCount;
            TextureQuality = preset.TextureQuality;
            AnisotropicFiltering = preset.AnisotropicFiltering;
            AntiAliasing = preset.AntiAliasing;
            SoftParticles = preset.SoftParticles;
            RealtimeReflectionProbe = preset.RealtimeReflectionProbe;
            VSyncCount = preset.VSyncCount;

            ShadowQualityType = preset.ShadowQualityType;
            ShadowResolutionType = preset.ShadowResolutionType;
            ShadowProjectionType = preset.ShadowProjectionType;
            ShadowDistance = preset.ShadowDistance;
            ShadowmaskModeType = preset.ShadowmaskModeType;
            ShadowNearPlaneOffset = preset.ShadowNearPlaneOffset;
            ShadowCascadeType = preset.ShadowCascadeType;
        }

        private void LoadValues()
        {
            ResolutionIndex = PlayerPrefs.GetInt("SETTINGSMANAGER_RESOLUTIONINDEX", ResolutionIndex);

            if (ResolutionIndex <= -1)
            {
                Vector2Int currentResolution = new Vector2Int(Screen.currentResolution.width, Screen.currentResolution.height);

                bool exit = false;
                foreach (KeyValuePair<AspectRatio, Vector2Int[]> aspectRatio in m_ResolutionList)
                {
                    for (int i = 0; i < aspectRatio.Value.Length; i++)
                    {
                        if (currentResolution == aspectRatio.Value[i])
                        {
                            ResolutionIndex = i;
                            AspectRatio = aspectRatio.Key;
                            exit = true;
                            break;
                        }
                    }

                    if (exit)
                    {
                        break;
                    }
                }

                if (ResolutionIndex == -1)
                {
                    // Standard 1080p
                    ResolutionIndex = 2;
                    AspectRatio = AspectRatio.Aspect16by9;
                }
            }
            else
            {
                AspectRatio = PlayerPrefsUtil.GetEnum("SETTINGSMANAGER_ASPECTRATIO", AspectRatio);
            }

            Fullscreen = PlayerPrefsUtil.GetBool("SETTINGSMANAGER_FULLSCREEN", Fullscreen);
            RefreshRate = PlayerPrefs.GetInt("SETTINGSMANAGER_REFRESHRATE", RefreshRate);

            PixelLightCount = PlayerPrefs.GetInt("SETTINGSMANAGER_PIXELLIGHTCOUNT", PixelLightCount);
            TextureQuality = PlayerPrefsUtil.GetEnum("SETTINGSMANAGER_TEXTUREQUALITYTYPE", TextureQuality);
            AnisotropicFiltering = PlayerPrefsUtil.GetBool("SETTINGSMANAGER_ANISOTROPICFILTERING", AnisotropicFiltering);
            AntiAliasing = PlayerPrefsUtil.GetEnum("SETTINGSMANAGER_ANTIALIASING", AntiAliasing);
            SoftParticles = PlayerPrefsUtil.GetBool("SETTINGSMANAGER_SOFTPARTICLES", SoftParticles);
            RealtimeReflectionProbe = PlayerPrefsUtil.GetBool("SETTINGSMANAGER_REALTIMEREFLECTIONPROBE", RealtimeReflectionProbe);
            VSyncCount = PlayerPrefs.GetInt("SETTINGSMANAGER_VSYNCCOUNT", VSyncCount);

            ShadowQualityType = PlayerPrefsUtil.GetEnum("SETTINGSMANAGER_SHADOWQUALITYTYPE", ShadowQualityType);
            ShadowResolutionType = PlayerPrefsUtil.GetEnum("SETTINGSMANAGER_SHADOWRESOLUTIONTYPE", ShadowResolutionType);
            ShadowProjectionType = PlayerPrefsUtil.GetEnum("SETTINGSMANAGER_SHADOWPROJECTIONTYPE", ShadowProjectionType);
            ShadowDistance = PlayerPrefsUtil.GetEnum("SETTINGSMANAGER_SHADOWDISTANCETYPE", ShadowDistance);
            ShadowmaskModeType = PlayerPrefsUtil.GetEnum("SETTINGSMANAGER_SHADOWMASKMODE", ShadowmaskModeType);
            ShadowNearPlaneOffset = PlayerPrefs.GetInt("SETTINGSMANAGER_SHADOWNEARPLANEOFFSET", ShadowNearPlaneOffset);
            ShadowCascadeType = PlayerPrefsUtil.GetEnum("SETTINGSMANAGER_SHADOWCASCADETYPE", ShadowCascadeType);
        }

        private void PopulateResolutionList()
        {
            if (m_ResolutionList != null)
            {
                return;
            }

            m_ResolutionList = new Dictionary<AspectRatio, Vector2Int[]>();

            Vector2Int[] ratio43 =
            {
                new Vector2Int(1024, 768),
                new Vector2Int(1280, 960),
                new Vector2Int(1440, 1080),
                new Vector2Int(1600, 1200),
                new Vector2Int(1920, 1440)
            };

            Vector2Int[] ratio169 =
            {
                new Vector2Int(1280, 720),
                new Vector2Int(1600, 900),
                new Vector2Int(1920, 1080),
                new Vector2Int(2560, 1440),
                new Vector2Int(3840, 2160)
            };

            Vector2Int[] ratio1610 =
            {
                new Vector2Int(1280, 800),
                new Vector2Int(1440, 900),
                new Vector2Int(1680, 1050),
                new Vector2Int(1920, 1200),
                new Vector2Int(2560, 1600)
            };

            m_ResolutionList.Add(UnityEditor.AspectRatio.Aspect4by3, ratio43);
            m_ResolutionList.Add(UnityEditor.AspectRatio.Aspect16by9, ratio169);
            m_ResolutionList.Add(UnityEditor.AspectRatio.Aspect16by10, ratio1610);
        }

        #endregion

        private void Start()
        {
            PopulateResolutionList();

            if (UsePresetAsDefault)
            {
                LoadValues(Presets[PresetUsedAsDefault], true);
            }
            else
            {
                LoadValues();
            }

            ApplyChanges(true);
        }

        [Serializable] public class PresetDictionary : SerializableDictionary<string, SettingsPreset> { }
    }
}