///====================================================================================================
///
///     SettingsManager by
///     - CantyCanadian
///
///====================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Canty.Managers
{
    public class SettingsManager : Singleton<SettingsManager>
    {
        #region Public Properties

        // Resolution
        public AspectRatios AspectRatio;
        public int ResolutionIndex;

        // Graphics
        public TextureQualityTypes TextureQuality;
        public bool AnisotropicFiltering;
        public AntiAliasingTypes AntiAliasing;
        public bool SoftParticles;
        public bool RealtimeReflectionProbe;
        public int VSyncCount;

        // Shadows
        public ShadowDistanceTypes ShadowDistance;
        public ShadowmaskMode ShadowmaskModeType;
        public ShadowResolution ShadowResolutionType;
        public ShadowQuality ShadowQualityType;
        public ShadowProjection ShadowProjectionType;
        public int ShadowDistance;
        public int ShadowNearPlaneOffset;
        public ShadowCascadeTypes ShadowCascadeType;

        // Other
        public Dictionary<AspectRatios, List<Vector2Int>> ResolutionData { get { return m_ResolutionList; } }

        #endregion

        #region Private Properties

        // Resolution
        private AspectRatios m_CurrentAspectRatio;
        private int m_CurrentResolutionIndex;

        // Graphics
        private TextureQualityTypes m_CurrentTextureQualityType;
        private bool m_CurrentAnisotropicFiltering;
        private AntiAliasingTypes m_CurrentAntiAliasingType;
        private bool m_CurrentSoftParticles;
        private bool m_CurrentRealtimeReflectionProbe;
        private int m_CurrentVSyncCount;

        // Shadows
        private ShadowDistanceTypes m_CurrentShadowDistanceType;
        private ShadowmaskMode m_CurrentShadowmaskModeType;
        private ShadowResolution m_CurrentShadowResolutionType;
        private ShadowQuality m_CurrentShadowQualityType;
        private ShadowProjection m_CurrentShadowProjectionType;
        private int m_CurrentShadowDistance;
        private int m_CurrentShadowNearPlaneOffset;
        private ShadowCascadeTypes m_CurrentShadowCascadeType;

        // Other
        private Dictionary<AspectRatios, List<Vector2Int>> m_ResolutionList = null;

        #endregion

        #region Setting Types

        public enum AspectRatios
        {
            AR43,           // 4:3
            AR169,          // 16:9
            AR1610          // 16:10
        }

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
            NoCascade,
            TwoCascade,
            FourCascade
        }

        public void ApplyChanges()
        {

        }

        private void LoadInitialValues()
        {            
            ResolutionIndex = PlayerPrefs.GetInt("SETTINGSMANAGER_RESOLUTIONINDEX", -1);

            if (ResolutionIndex == -1)
            {
                Vector2Int currentResolution = new Vector2Int(Screen.currentResolution.width, Screen.currentResolution.height);

                bool exit = false;
                foreach(KeyValuePair<AspectRatios, Vector2Int[]> aspectRatio in m_ResolutionList)
                {
                    for(int i = 0; i < aspectRatio.Value.Length; i++)
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
                    AspectRatio = AspectRatios.AR169;
                }
            }
            else
            {
                AspectRatio = PlayerPrefsUtil.GetEnum<AspectRatios>("SETTINGSMANAGER_ASPECTRATIO", AspectRatios.AR169);
            }


            TextureQuality = PlayerPrefsUtil.GetEnum<TextureQualityTypes>("SETTINGSMANAGER_TEXTUREQUALITYTYPE", TextureQualityTypes.FullRes);
            AnisotropicFiltering = PlayerPrefsUtil.GetBool("SETTINGSMANAGER_ANISOTROPICFILTERING", false);
            AntiAliasing = PlayerPrefsUtil.GetEnum<AntiAliasingTypes>("SETTINGSMANAGER_ANTIALIASING", AntiAliasingTypes.Disabled);
            SoftParticles = PlayerPrefsUtil.GetBool("SETTINGSMANAGER_SOFTPARTICLES", false);
            RealtimeReflectionProbe = PlayerPrefsUtil.GetBool("SETTINGSMANAGER_REALTIMEREFLECTIONPROBE", false);
            VSyncCount = PlayerPrefs.GetInt("SETTINGSMANAGER_VSYNCCOUNT", 0);

            ShadowDistance = PlayerPrefsUtil.GetEnum<ShadowDistanceTypes>("SETTINGSMANAGER_SHADOWDISTANCE", ShadowDistanceTypes.Ultra);
            ShadowmaskModeType = PlayerPrefsUtil.GetEnum<ShadowmaskMode>("SETTINGSMANAGER_SHADOWMASKMODE", ShadowmaskMode.Shadowmask);
            ShadowResolutionType;
            m_CurrentShadowQualityType;
            m_CurrentShadowProjectionType;
            m_CurrentShadowDistance;
            m_CurrentShadowNearPlaneOffset;
            m_CurrentShadowCascadeType;
        }

        private void PopulateResolutionList()
        {
            if (m_ResolutionList != null)
            {
                return;
            }

            m_ResolutionList = new Dictionary<AspectRatios, Vector2Int[]>();

            Vector2Int[] ratio43 = new Vector2Int[]
            {
                new Vector2Int(1024, 768),
                new Vector2Int(1280, 960),
                new Vector2Int(1440, 1080),
                new Vector2Int(1600, 1200),
                new Vector2Int(1920, 1440)
            };

            Vector2Int[] ratio169 = new Vector2Int[]
            {
                new Vector2Int(1280, 720),
                new Vector2Int(1600, 900),
                new Vector2Int(1920, 1080),
                new Vector2Int(2560, 1440),
                new Vector2Int(3840, 2160)
            };

            Vector2Int[] ratio1610 = new Vector2Int[]
            {
                new Vector2Int(1280, 800),
                new Vector2Int(1440, 900),
                new Vector2Int(1680, 1050),
                new Vector2Int(1920, 1200),
                new Vector2Int(2560, 1600)
            };

            m_ResolutionList.Add(AspectRatios.AR43, ratio43);
            m_ResolutionList.Add(AspectRatios.AR169, ratio169);
            m_ResolutionList.Add(AspectRatios.AR1610, ratio1610);
        }

        #endregion

        private void Start()
        {
            PopulateResolutionList();
        }
    }
}