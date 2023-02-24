using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bridge between the Meson Unity AdUnit-specific API and platform-specific implementations.
/// </summary>
/// <para>
/// Publishers integrating with Meson should make all calls through the Meson class, and handle any
/// desired Meson Events from the MesonManager class.
/// </para>
/// <para>
/// For platform-specific implementations, see MesonUnityEditorAdUnit, MesonAndroidAdUnit and MesoniOSAdUnit.
/// </para>
public class MesonAdUnit
{
    public static readonly MesonAdUnit NullMesonAdUnit = new MesonAdUnit(null);

    protected readonly string AdUnitId;
    protected readonly string AdType;

    protected MesonAdUnit(string adUnitId, string adType = null)
    {
        AdUnitId = adUnitId;
        AdType = adType;
    }

    internal static MesonAdUnit CreateMesonAdUnit(string adUnitId, string adType = null)
    {
        if (adType != "Banner" && adType != "Interstitial" && adType != "Native")
            MesonLog.Log("CreateMesonAdUnit",MesonLog.AdLogEvent.InvalidAdType, adType);

        // Choose created class based on target platform...
        return new
#if UNITY_EDITOR
            MesonUnityEditorAdUnit
#elif UNITY_ANDROID
            MesonAndroidAdUnit
#else
            MesoniOSAdUnit
#endif
        (adUnitId, adType);
    }


    internal virtual bool IsPluginReady()
    {
        return false;
    }


    #region Banners


    internal virtual void RequestBanner(float width, float height, Meson.AdPosition position) { }

    internal virtual void DestroyBanner() { }

    internal virtual string GetBannerAdData() { return ""; }


    #endregion Banners


    #region Interstitials

    internal virtual void RequestInterstitialAd() { }

    internal virtual bool IsInterstitialReady()
    {
        return false;
    }


    internal virtual void ShowInterstitialAd() { }

    internal virtual void DestroyInterstitialAd() { }

    internal virtual string GetInterstitialAdData() { return ""; }

    #endregion Interstitials

    protected bool CheckPluginReady()
    {
        var isReady = IsPluginReady();

        if (!isReady)
            MesonLog.Log("CheckPluginReady", MesonLog.AdLogEvent.InvalidCallToMesonAPI, AdType, AdUnitId);

        return isReady;

    }

    internal MesonAdData InterstitialAdData
    {
        get
        {
            return MesonUtils.DecodeAdData(GetInterstitialAdData());
        }
    }

    internal MesonAdData BannerAdData
    {
        get
        {
            return MesonUtils.DecodeAdData(GetBannerAdData());
        }
    }
}
