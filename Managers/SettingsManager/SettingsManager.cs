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
        private Dictionary<string, List<Vector2Int>> m_ResolutionList = null;

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

            m_ResolutionList.Add("4:3", ratio43);
            m_ResolutionList.Add("16:9", ratio169);
            m_ResolutionList.Add("16:10", ratio1610);
        }

        private void Start()
        {
            PopulateResolutionList();
        }
    }
}