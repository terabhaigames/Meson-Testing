using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

#if UNITY_IOS

/// <summary>
/// Bridge between the Meson Unity AdUnit-specific API and iOS implementation.
/// </summary>
/// <para>
/// Publishers integrating with Meson should make all calls through the <see cref="Meson"/> class, and handle any
/// desired Meson Events from the <see cref="MesonManager"/> class.
/// </para>
/// <para>
/// For platform-specific implementations, see
/// <see cref="MesonUnityEditorAdUnit"/> and <see cref="MesonAndroidAdUnit"/>.
/// </para>
[SuppressMessage("ReSharper", "UnusedMember.Global")]
internal class MesoniOSAdUnit : MesonAdUnit
{
    internal MesoniOSAdUnit(string appId, string adType = null) : base (appId, adType) { }


    internal override bool IsPluginReady()
    {
        return _MesonIsPluginReady(AdUnitId);
    }


#region Banners

    internal override void RequestBanner(float width, float height, Meson.AdPosition position)
    {
        _MesonRequestBanner(width, height, (int) position, AdUnitId);
    }


    internal override void DestroyBanner()
    {
        if (!CheckPluginReady()) return;

        _MesonDestroyBanner(AdUnitId);
    }


    internal override string GetBannerAdData()
    {
        return _MesonBannerAdData(AdUnitId);
    }

#endregion

#region Interstitials

    internal override void RequestInterstitialAd()
    {
        _MesonRequestInterstitialAd(AdUnitId);
    }


    internal override bool IsInterstitialReady() {
        return _MesonIsInterstitialReady(AdUnitId);
    }


    internal override void ShowInterstitialAd()
    {
        if (!CheckPluginReady()) return;

        _MesonShowInterstitialAd(AdUnitId);
    }


    internal override void DestroyInterstitialAd()
    {
        if (!CheckPluginReady()) return;

        _MesonDestroyInterstitialAd(AdUnitId);
    }


    internal override string GetInterstitialAdData()
    {
        return _MesonInterstitialAdData(AdUnitId);
    }

#endregion

#region DllImports

    [DllImport("__Internal")]
    private static extern bool _MesonIsPluginReady(string adUnitId);

    [DllImport("__Internal")]
    private static extern void _MesonRequestBanner(float width, float height, int position, string adUnitId);


    [DllImport("__Internal")]
    private static extern void _MesonDestroyBanner(string adUnitId);


    [DllImport("__Internal")]
    private static extern void _MesonRequestInterstitialAd(string adUnitId);


    [DllImport("__Internal")]
    private static extern bool _MesonIsInterstitialReady(string adUnitId);


    [DllImport("__Internal")]
    private static extern void _MesonShowInterstitialAd(string adUnitId);


    [DllImport("__Internal")]
    private static extern void _MesonDestroyInterstitialAd(string adUnitId);


    [DllImport("__Internal")]
    private static extern string _MesonInterstitialAdData(string adUnitId);


    [DllImport("__Internal")]
    private static extern string _MesonBannerAdData(string adUnitId);

#endregion
}

#endif
