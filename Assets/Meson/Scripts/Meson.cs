using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Meson Unity API for publishers, including documentation. Support classes are located in the <see cref="MesonBase"/>
/// class.
/// </summary>
/// <para>
/// Publishers integrating with Meson should make all calls through this class, and handle any desired Meson Events from
/// the <see cref="MesonManager"/> class.
/// </para>
public abstract class Meson : MesonBase
{

    /// <summary>
    /// The version for the Meson Unity Plugin, which includes specific versions of the Meson Android and iOS SDKs.
    /// </summary>
    public const string MesonUnityPluginVersion = "1.0.0-beta5";

    public const string MESON_GDPR_CONSENT_AVAILABLE = "gdpr_consent_available";
    public const string MESON_GDPR_CONSENT_GDPR_APPLIES = "gdpr";
    public const string MESON_GDPR_CONSENT_IAB = "gdpr_consent";

    public Dictionary<string, string> consentDict;

    #region SdkSetup

    /// <summary>
    /// Asynchronously initializes the relevant (Android or iOS) Meson SDK.
    /// See <see cref="MesonManager.OnSdkInitializedEvent"/> for the resulting triggered event.
    /// </summary>
    /// <param name="sdkConfiguration">The configuration including at least an ad unit.
    /// See <see cref="Meson.SdkConfiguration"/> for details.</param>
    /// <remarks>The Meson SDK needs to be initialized on Start() to ensure all other objects have been enabled first.
    /// (Start() rather than Awake() so that MesonManager has had time to Awake() and OnEnable() in order to receive
    /// event callbacks.)</remarks>
    public static void InitializeSdk(SdkConfiguration sdkConfiguration)
    {
        CachedLogLevel = sdkConfiguration.LogLevel;
        MesonLog.Log("InitializeSdk", MesonLog.SdkLogEvent.InitStarted);

        ValidateAppIdForSdkInit(sdkConfiguration.AndroidAppId, sdkConfiguration.IOSAppId);

        MesonManager.MesonPlatformApi.InitializeSdk(sdkConfiguration);
    }


    /// <summary>
    /// Updates the GDPR Consent (Android or iOS) Meson SDK.
    /// </summary>
    /// <param name="consent">Dictionary with updated consent.</param>
    public static void UpdateGDPRConsent(Dictionary<string, string> consent)
    {
        MesonManager.MesonPlatformApi.UpdateGDPRConsent(new SdkConfiguration {ConsentDict = consent});
    }

    /// <summary>
    /// Sets the extra data (Android or iOS) Meson SDK.
    /// </summary>
    /// <param name="extras">Dictionary with extra data.</param>
    public static void SetExtras(Dictionary<string, object> extras)
    {
        MesonManager.MesonPlatformApi.SetExtras(new SdkConfiguration { ExtrasDict = extras });
    }

    /// <summary>
    /// Gets the extra data (Android or iOS) Meson SDK.
    /// </summary>
    public static Dictionary<string, object> GetExtras()
    {
        return new SdkConfiguration().GetExtrasDict(MesonManager.MesonPlatformApi.GetExtras());
    }

    /// <summary>
    /// Sets the Publisher Provided Identifier (Android or iOS) Meson SDK.
    /// /// <param name="pPID"> String to set the PPID.</param>
    /// </summary>
    public static void SetPPID(string pPID)
    {
        MesonManager.MesonPlatformApi.SetPPID(pPID);
    }

    /// <summary>
    /// Gets the PPID (Android or iOS) Meson SDK.
    /// </summary>
    public static string GetPPID()
    {
        return MesonManager.MesonPlatformApi.GetPPID();
    }

    /// <summary>
    /// Cleans User Information (Android or iOS) Meson SDK.
    /// </summary>
    public static void CleanUserInfo()
    {
        MesonManager.MesonPlatformApi.CleanUserInfo();
    }

    /// <summary>
    /// Sets the location (Android or iOS) Meson SDK.
    /// </summary>
    /// <param name="latitude"> Double to set the latitude.</param>
    /// <param name="longitude"> Double to set the longitude.</param>
    public static void SetLocation(double latitude, double longitude)
    {
        MesonManager.MesonPlatformApi.SetLocation(latitude, longitude);
    }

    /// <summary>
    /// Initializes a platform-specific Meson SDK banner plugin for each given ad unit.
    /// </summary>
    /// <param name="adUnitIds">The ad units to initialize plugins for</param>
    public static void LoadBannerPluginsForAdUnits(string[] adUnitIds)
    {
        LoadPluginsForAdUnits(adUnitIds, "Banner");
    }

    /// <summary>
    /// Initializes a platform-specific Meson SDK interstitial plugin for each given ad unit.
    /// </summary>
    /// <param name="adUnitIds">The ad units to initialize plugins for</param>
    public static void LoadInterstitialPluginsForAdUnits(string[] adUnitIds)
    {
        LoadPluginsForAdUnits(adUnitIds, "Interstitial");
    }

    /// <summary>
    /// Returns a human-readable string of the Meson SDK being used.
    /// </summary>
    /// <returns>A string with the Meson SDK platform and version.</returns>
    public static string SdkName
    {
        get { return MesonManager.MesonPlatformApi.SdkName; }
    }


    /// <summary>
    /// Flag indicating if the SDK has been initialized.
    /// </summary>
    /// <returns>true if a call to initialize the SDK has been made; false otherwise.</returns>
    public static bool IsSdkInitialized {
        get { return MesonManager.MesonPlatformApi != null && MesonManager.MesonPlatformApi.IsSdkInitialized; }
    }

    /// <summary>
    /// Meson SDK log level. The default value is: `MPLogLevelInfo` before SDK init, `MPLogLevelNone` after SDK init.
    /// See Meson.<see cref="Meson.LogLevel"/> for all possible options. Can also be set via
    /// Meson.<see cref="Meson.SdkConfiguration"/> on
    /// Meson.<see cref="Meson.InitializeSdk(Meson.SdkConfiguration)"/>
    /// </summary>
    public static LogLevel SdkLogLevel
    {
        set {
            MesonManager.MesonPlatformApi.SetSdkLogLevel(value);
            CachedLogLevel = value;
        }
    }


#endregion SdkSetup

#region Banners


    /// <summary>
    /// Requests a banner ad and immediately shows it once loaded.
    /// </summary>
    /// <param name="adUnitId">A string with the ad unit id.</param>
    /// <param name="position">Where in the screen to position the loaded ad. See <see cref="Meson.AdPosition"/>.
    /// </param>
    /// <param name="AdSize">The maximum size of the banner to load. See <see cref="Meson.AdSize"/>.</param>
    /// </param>
    public static void RequestBanner(string adUnitId, AdPosition position,
        AdSize AdSize = AdSize.Banner)
    {
        var width = AdSize.Width();
        var height = AdSize.Height();
        MesonLog.Log("RequestBanner", MesonLog.AdLogEvent.LoadAttempted);
        MesonLog.Log("RequestBanner", "Size requested: " + width + "x" + height);
        AdUnitManager.GetAdUnit(adUnitId).RequestBanner(width, height, position);
    }

    /// <summary>
    /// Destroys the banner ad and removes it from the view.
    /// </summary>
    /// <param name="adUnitId">A string with the ad unit id.</param>
    public static void DestroyBanner(string adUnitId)
    {
        AdUnitManager.GetAdUnit(adUnitId).DestroyBanner();
    }


    /// <summary>
    /// Retrieves the banner ad impression data.
    /// Use this method after the load sucess callback
    /// </summary>
    /// <param name="adUnitId">A string with the ad unit id.</param>
    public static MesonAdData BannerAdData(string adUnitId)
    {
        return AdUnitManager.GetAdUnit(adUnitId).BannerAdData;
    }


    /// <summary>
    /// Retrieves the interstitial ad impression data.
    /// Use this method after the load sucess callback
    /// </summary>
    /// <param name="adUnitId">A string with the ad unit id.</param>
    public static MesonAdData InterstitialAdData(string adUnitId)
    {
        return AdUnitManager.GetAdUnit(adUnitId).InterstitialAdData;
    }

    #endregion


    #region Interstitials


    /// <summary>
    /// Requests an interstitial ad to be loaded. The two possible resulting events
    /// are <see cref="MesonManager.OnInterstitialLoadedEvent"/> and
    /// <see cref="MesonManager.OnInterstitialFailedEvent"/>.
    /// </summary>
    /// <param name="adUnitId">A string with the ad unit id.</param>
    public static void RequestInterstitialAd(string adUnitId)
    {
        MesonLog.Log("RequestInterstitialAd", MesonLog.AdLogEvent.LoadAttempted);
        AdUnitManager.GetAdUnit(adUnitId).RequestInterstitialAd();
    }


    /// <summary>
    /// If the interstitial ad has loaded, this will take over the screen and show the ad.
    /// </summary>
    /// <param name="adUnitId">A string with the ad unit id.</param>
    /// <remarks><see cref="MesonManager.OnInterstitialLoadedEvent"/> must have been triggered already.</remarks>
    public static void ShowInterstitialAd(string adUnitId)
    {
        MesonLog.Log("ShowInterstitialAd", MesonLog.AdLogEvent.ShowAttempted);
        AdUnitManager.GetAdUnit(adUnitId).ShowInterstitialAd();
    }


    /// <summary>
    /// Whether the interstitial ad is ready to be shown or not.
    /// </summary>
    /// <param name="adUnitId">A string with the ad unit id.</param>
    public static bool IsInterstitialAdReady(string adUnitId)
    {
        return AdUnitManager.GetAdUnit(adUnitId).IsInterstitialReady();
    }


    /// <summary>
    /// Destroys an already-loaded interstitial ad.
    /// </summary>
    /// <param name="adUnitId">A string with the ad unit id.</param>
    public static void DestroyInterstitialAd(string adUnitId)
    {
        AdUnitManager.GetAdUnit(adUnitId).DestroyInterstitialAd();
    }

#endregion Interstitials
}
