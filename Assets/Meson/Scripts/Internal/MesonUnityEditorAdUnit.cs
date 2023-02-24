#if UNITY_EDITOR

/// <summary>
/// Bridge between the Meson Unity AdUnit-specific API and In-Editor implementation.
/// </summary>
/// <para>
/// Publishers integrating with Meson should make all calls through the <see cref="Meson"/> class, and handle any
/// desired Meson Events from the <see cref="MesonManager"/> class.
/// </para>
/// <para>
/// For other platform-specific implementations, see
/// <see cref="MesonAndroidAdUnit"/> and <see cref="MesoniOSAdUnit"/>.
/// </para>
internal class MesonUnityEditorAdUnit : MesonAdUnit
{
    private bool _requested;

    internal MesonUnityEditorAdUnit(string adUnitId, string adType = null) : base(adUnitId, adType) { }

    internal override bool IsPluginReady()
    {
        return _requested;
    }

    #region Banners

    internal override void RequestBanner(float width, float height, Meson.AdPosition position)
    {
        RequestAdUnit();
    }

    internal override void DestroyBanner()
    {
        CheckAdUnitRequested();
    }

    #endregion

    #region Interstitials

    internal override void RequestInterstitialAd()
    {
        RequestAdUnit();
        MesonUnityEditor.WaitOneFrame(() => {
            MesonManager.Instance.EmitInterstitialAdLoadedEvent(MesonUnityEditor.ArgsToJson(AdUnitId));
        });
    }

    internal override bool IsInterstitialReady()
    {
        return _requested;
    }

    internal override void ShowInterstitialAd()
    {
        if (!CheckAdUnitRequested()) return;

        MesonUnityEditor.WaitOneFrame(() => {
            var json = MesonUnityEditor.ArgsToJson(AdUnitId);
            MesonManager.Instance.EmitInterstitialAdDisplayedEvent(json);
            MesonUnityEditor.WaitOneFrame(() => {
                MesonManager.Instance.EmitInterstitialAdDismissedEvent(json);
                MesonUnityEditor.SimulateApplicationResume();
            });
        });
    }

    internal override void DestroyInterstitialAd()
    {
        CheckAdUnitRequested();
    }

    #endregion

    private void RequestAdUnit()
    {
        _requested = true;
    }

    private bool CheckAdUnitRequested()
    {
        return CheckPluginReady() && _requested;
    }
}
#endif
