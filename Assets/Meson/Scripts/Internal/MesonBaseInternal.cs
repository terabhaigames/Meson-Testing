using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Internal functionality used by the Meson API; publishers should not call these methods directly and use the
/// <see cref="Meson"/> class instead.
/// </summary>
public class MesonBaseInternal
{
    public static MesonBase.LogLevel CachedLogLevel;

    protected static void LoadPluginsForAdUnits(string[] adUnitIds, string adType = null)
    {
        AdUnitManager.InitAdUnits(adUnitIds, adType);
    }


    protected static void ValidateAppIdForSdkInit(string androidAppId, string iOSAppId)
    {
#if UNITY_ANDROID
        ValidateInit(androidAppId, "FATAL ERROR: A valid app ID is needed to initialize the Meson SDK.\n" +
                     "Please enter an ad unit ID from your app into the MesonManager GameObject.");
#else 
        ValidateInit(iOSAppId, "FATAL ERROR: A valid app ID is needed to initialize the Meson SDK. " +
                     "Please enter an ad unit ID from your app into the MesonManager GameObject.");
#endif

    }


    protected static void ValidateInit(string appId, string message = null)
    {
        if (!string.IsNullOrEmpty(appId)) return;

        message = message ?? "FATAL ERROR: Meson SDK has not been successfully initialized.";
        MesonLog.Log("ValidateInit",message);
    }


    private static void ReportAdUnitNotFound(string adUnitId)
    {
        MesonLog.Log("ReportAdUnitNotFound",MesonLog.AdLogEvent.AdUnitNotFound, adUnitId);
    }


    static MesonBaseInternal()
    {
        InitManager();
    }


    // Allocate the MesonManager singleton, which receives all callback events from the native SDKs.
    // This is done in case the app is not using the new MesonManager prefab, for backwards compatibility.
    private static void InitManager()
    {
        if (MesonManager.Instance == null)
            new GameObject("MesonManager", typeof(MesonManager));
    }


    protected static class AdUnitManager
    {
        private static readonly Dictionary<string, MesonAdUnit> AdUnits = new Dictionary<string, MesonAdUnit>();

        public static void InitAdUnits(string[] adUnitIds, string adType)
        {
            foreach (var adUnitId in adUnitIds) {
                if (!AdUnits.ContainsKey(adUnitId)) {
                    AdUnits[adUnitId] = MesonAdUnit.CreateMesonAdUnit(adUnitId, adType);
                    MesonLog.Log("InitAdUnits",MesonLog.AdLogEvent.AdUnitsInitialized, adType, adUnitId);
                }
                else {
                    MesonLog.Log("InitAdUnits",MesonLog.AdLogEvent.AdUnitsAlreadyInitialized,
                        adType, adUnitId);
                }
            }
        }

        public static MesonAdUnit GetAdUnit(string adUnitId)
        {
            if (!Meson.IsSdkInitialized)
            {
                MesonLog.Log("GetAdUnit", MesonLog.AdLogEvent.SDKNotInitialized);
                return MesonAdUnit.NullMesonAdUnit;
            }

            MesonAdUnit adUnit;
            if (AdUnits.TryGetValue(adUnitId, out adUnit))
                return adUnit;

            ReportAdUnitNotFound(adUnitId);
            return MesonAdUnit.NullMesonAdUnit;
        }
    }
}
