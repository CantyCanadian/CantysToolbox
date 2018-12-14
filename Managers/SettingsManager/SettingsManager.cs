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

        // Shadows
        public ShadowDistanceTypes ShadowDistance;
        public ShadowmaskMode ShadowmaskModeType;
        public ShadowResolution ShadowResolutionType;
        public ShadowQuality ShadowQualityType;
        public ShadowProjection ShadowProjectionType;

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

        // Shadows
        private ShadowDistanceTypes m_CurrentShadowDistanceType;
        private ShadowmaskMode m_CurrentShadowmaskModeType;
        private ShadowResolution m_CurrentShadowResolutionType;
        private ShadowQuality m_CurrentShadowQualityType;
        private ShadowProjection m_CurrentShadowProjectionType;

        // Other
        private Dictionary<AspectRatios, List<Vector2Int>> m_ResolutionList = null;

        #endregion

        #region SettingTypes

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

        private void PopulateResolutionList()
        {
            if (m_ResolutionList != null)
            {
                return;
            }

            m_ResolutionList = new Dictionary<string, Vector2Int[]>();

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