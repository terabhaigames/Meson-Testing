using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

/// <summary>
/// Bridge between the Meson Unity AdUnit-specific API and Android implementation.
/// </summary>
/// <para>
/// Publishers integrating with Meson should make all calls through the <see cref="Meson"/> class, and handle any
/// desired Meson Events from the <see cref="MesonManager"/> class.
/// </para>
/// <para>
/// For other platform-specific implementations, see
/// <see cref="MesonUnityEditorAdUnit"/> and <see cref="MesoniOSAdUnit"/>.
/// </para>
[SuppressMessage("ReSharper", "UnusedMember.Global")]
internal class MesonAndroidAdUnit : MesonAdUnit {
    private readonly AndroidJavaObject _plugin;

    internal MesonAndroidAdUnit(string adUnitId, string adType = null) : base (adUnitId, adType)
    {
        _plugin = new AndroidJavaObject("ai.meson.unity.Meson" + adType + "UnityPlugin", adUnitId);
    }


    internal override bool IsPluginReady()
    {
        return _plugin.Call<bool>("isPluginReady");
    }


    #region Banners

    internal override void RequestBanner(float width, float height, Meson.AdPosition position)
    {
        _plugin.Call("requestBanner", width, height, (int) position);
    }

    internal override void DestroyBanner()
    {
        if (!CheckPluginReady()) return;

        _plugin.Call("destroyBanner");
    }


    internal override string GetBannerAdData()
    {
        return _plugin.Call<string>("getMesonAdData");
    }

    #endregion

    #region Interstitials

    internal override void RequestInterstitialAd()
    {
        _plugin.Call("request");
    }


    internal override void ShowInterstitialAd()
    {
        if (!CheckPluginReady()) return;

        _plugin.Call("show");
    }


    internal override bool IsInterstitialReady()
    {
        return _plugin.Call<bool>("isReady");
    }


    internal override void DestroyInterstitialAd()
    {
        if (!CheckPluginReady()) return;

        _plugin.Call("destroy");
    }


    internal override string GetInterstitialAdData()
    {
        return _plugin.Call<string>("getMesonAdData");
    }

    #endregion

}
