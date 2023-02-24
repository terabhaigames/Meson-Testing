using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Handler for Meson integration across publisher apps and Unity Editor.
/// </summary>
/// <para>
/// Publishers integrating with Meson should make all calls through the <see cref="Meson"/> class, and handle any
/// desired Meson Events from this class.
/// </para>
[SuppressMessage("ReSharper", "InconsistentNaming")]
public class MesonManager : MonoBehaviour
{
    #region MesonEvents


    // Fired when the SDK has finished initializing
    public static event Action<string> SdkInitializedEvent;

    // Fired when an ad loads in the banner. Includes the ad height.
    public static event Action<string> BannerAdLoadedEvent;

    // Fired when an ad fails to load for the banner
    public static event Action<string, string> BannerAdLoadFailedEvent;

    // Fired when a banner ad is clicked
    public static event Action<string, Dictionary<string, object>> BannerAdClickedEvent;

    // Fired when the user is taken out of the application.
    public static event Action<string> BannerAdUserLeftApplicationEvent;

    //Fired when the banner has finished presenting screen.
    public static event Action<string> BannerAdPresentedScreenEvent;

    //Fired when the banner has dismissed the presented screen.
    public static event Action<string> BannerAdCollapsedScreenEvent;

    // Fired when an interstitial ad is loaded and ready to be shown
    public static event Action<string> InterstitialAdLoadedEvent;

    // Fired when an interstitial ad fails to load
    public static event Action<string, string> InterstitialAdLoadFailedEvent;

    // Fired when an interstitial ad is dismissed
    public static event Action<string> InterstitialAdDismissedEvent;

    // Fired when an interstitial ad is clicked
    public static event Action<string, Dictionary<string, object>> InterstitialAdClickedEvent;

    // Fired when an interstitial ad failed to display
    public static event Action<string> InterstitialAdDisplayFailedEvent;

    // Fired when the user is taken away from the application 
    public static event Action<string> InterstitialAdUserLeftApplicationEvent;

    // Fired when an interstitial ad is displayed
    public static event Action<string> InterstitialAdDisplayedEvent;


    // Fired when a rewarded video completes. Includes all the data available about the reward.
    public static event Action<string, Dictionary<string, object>> RewardedVideoAdReceivedRewardsEvent;

    //Fired when impression data is received.
    public static event Action<string, MesonAdData> BannerAdImpressionTrackedEvent;

    //Fired when impression data is received.
    public static event Action<string, MesonAdData> InterstitialAdImpressionTrackedEvent;

    #endregion MesonEvents


    // Singleton.
    public static MesonManager Instance { get; protected set; }

    #region MesonManagerPrefab


    [Header("Initialization")]

    [Tooltip("If enabled, the SDK will be initialized at start, based on the values provided in this script and in any attached NetworkConfig scripts.")]
    public bool AutoInitializeOnStart;

    [Tooltip("Any app id, used to identify which Meson Android account this app will use.")]
    public string AndroidAppId;

    [Tooltip("Any app id, used to identify which Meson iOS account this app will use.")]
    public string iOSAppId;

    [Tooltip("Set the logging verbosity level for the Meson SDK.")]
    public Meson.LogLevel LogLevel = Meson.LogLevel.None;

    [Tooltip("Set GDPR Consent availability.")]
    public string MesonGDPRConsentAvailable = "true";

    [Tooltip("Set GDPR Consent applies.")]
    public string MesonGDPRConsentGDPRApplies = "0";

    [Tooltip("Set GDPR Consent IAB.")]
    public string MesonGDPRConsentIAB = "IAB String v1 or v2";

    /// <summary>
    /// Collects the information from the above fields and objects into a
    /// single <see cref="Meson.SdkConfiguration"/> struct.
    /// </summary>
    public Meson.SdkConfiguration SdkConfiguration
    {
        get {

            Dictionary<string, string> consentDictionary = new Dictionary<string, string>();
            consentDictionary.Add(Meson.MESON_GDPR_CONSENT_AVAILABLE, MesonGDPRConsentAvailable);
            consentDictionary.Add(Meson.MESON_GDPR_CONSENT_GDPR_APPLIES, MesonGDPRConsentGDPRApplies);
            consentDictionary.Add(Meson.MESON_GDPR_CONSENT_IAB, MesonGDPRConsentIAB);

            var config = new Meson.SdkConfiguration {
                AndroidAppId = AndroidAppId,
                IOSAppId = iOSAppId,
                LogLevel = LogLevel,
                ConsentDict = consentDictionary
            };

            if (config.LogLevel != MesonBase.LogLevel.None &&
                config.LogLevel != MesonBase.LogLevel.Error &&
                config.LogLevel != MesonBase.LogLevel.Debug)
                config.LogLevel = MesonBase.LogLevel.None;

            return config;
        }
    }


    // This enables the event to appear in the inspector panel.
    [Serializable] public class InitializedEvent : UnityEvent<string> { }

    [Header("Callback")]

    // Add any callbacks to this event that must execute once the SDK has initialized.
    public InitializedEvent Initialized;

    // API to make calls to the platform-specific Meson SDK.
    internal static MesonPlatformApi MesonPlatformApi { get; private set; }

    // Forwards invocations of C# event OnSdkInitializedEvent to UnityEvent OnInitialized.
    protected void fwdSdkInitialized(string appId)
    {
        if (isActiveAndEnabled && Initialized != null)
            Initialized.Invoke(appId);
    }


    void Awake()
    {
        if (Instance == null)
            Instance = this;
        
        if (MesonPlatformApi == null)
            MesonPlatformApi = new
#if UNITY_EDITOR
                MesonUnityEditor
#elif UNITY_ANDROID
                MesonAndroid
#else
                MesoniOS
#endif
                ();

        SdkInitializedEvent += fwdSdkInitialized;
        if (transform.parent == null)
            DontDestroyOnLoad(gameObject);
    }


    void Start()
    {
        if (Instance != this || !AutoInitializeOnStart || Meson.IsSdkInitialized) return;
        Meson.InitializeSdk(SdkConfiguration);
    }

    void OnDestroy()
    {
        SdkInitializedEvent -= fwdSdkInitialized;
        if (Instance == this)
            Instance = null;
    }


    #endregion MesonManagerPrefab


    #region PlatformCallbacks

    public void EmitSdkInitializedEvent(string argsJson)
    {
        var args = MesonUtils.DecodeArgs(argsJson, min: 1);
        var appId = args[0];
        
        MesonLog.Log("EmitSdkInitializedEvent", MesonLog.SdkLogEvent.InitFinished);
        var evt = SdkInitializedEvent;
        if (evt != null) evt(appId);
    }

    // Banner Listeners


    public void EmitBannerAdLoadedEvent(string argsJson)
    {
        var args = MesonUtils.DecodeArgs(argsJson, min: 1);
        var adUnitId = args[0];

        MesonLog.Log("EmitBannerAdLoadedEvent", MesonLog.AdLogEvent.LoadSuccess);
       
        var evt = BannerAdLoadedEvent;
        if (evt != null) evt(adUnitId);
    }


    public void EmitBannerAdLoadFailedEvent(string argsJson)
    {
        var args = MesonUtils.DecodeArgs(argsJson, min: 2);
        var adUnitId = args[0];
        var error = args[1];

        MesonLog.Log("EmitBannerAdLoadFailedEvent", MesonLog.AdLogEvent.LoadFailed, adUnitId, error);
        var evt = BannerAdLoadFailedEvent;
        if (evt != null) evt(adUnitId, error);
    }


    public void EmitBannerAdClickedEvent(string argsJson)
    {
        var args = MesonUtils.DecodeArgs(argsJson, min: 2);
        var adUnitId = args[0];
        var paramJson = args[1];
        Dictionary<string, object> param = new Dictionary<string, object>();

        if (paramJson.Length > 0) {
            param = MesonUtils.DecodeArgs(paramJson);
        }

        MesonLog.Log("EmitBannerAdClickedEvent", MesonLog.AdLogEvent.Tapped);
        var evt = BannerAdClickedEvent;
        if (evt != null) evt(adUnitId, param);
    }

    public void EmitBannerAdUserLeftApplicationEvent(string argsJson)
    {
        var args = MesonUtils.DecodeArgs(argsJson, min: 1);
        var adUnitId = args[0];

        MesonLog.Log("EmitBannerAdUserLeftApplicationEvent", MesonLog.AdLogEvent.UserLeftApplication);
        var evt = BannerAdUserLeftApplicationEvent;
        if (evt != null) evt(adUnitId);
    }

    public void EmitBannerAdPresentedScreenEvent(string argsJson)
    {
        var args = MesonUtils.DecodeArgs(argsJson, min: 1);
        var adUnitId = args[0];

        MesonLog.Log("EmitBannerAdPresentedScreenEvent", MesonLog.AdLogEvent.Presented);
        var evt = BannerAdPresentedScreenEvent;
        if (evt != null) evt(adUnitId);
    }

    public void EmitBannerAdCollapsedScreenEvent(string argsJson)
    {
        var args = MesonUtils.DecodeArgs(argsJson, min: 1);
        var adUnitId = args[0];

        MesonLog.Log("EmitBannerAdCollapsedScreenEvent", MesonLog.AdLogEvent.Collapsed);
        var evt = BannerAdCollapsedScreenEvent;
        if (evt != null) evt(adUnitId);
    }


    // Interstitial Listeners


    public void EmitInterstitialAdLoadedEvent(string argsJson)
    {
        var args = MesonUtils.DecodeArgs(argsJson, min: 1);
        var adUnitId = args[0];

        MesonLog.Log("EmitInterstitialAdLoadedEvent", MesonLog.AdLogEvent.LoadSuccess);
        var evt = InterstitialAdLoadedEvent;
        if (evt != null) evt(adUnitId);
    }


    public void EmitInterstitialAdLoadFailedEvent(string argsJson)
    {
        var args = MesonUtils.DecodeArgs(argsJson, min: 2);
        var adUnitId = args[0];
        var error = args[1];

        MesonLog.Log("EmitInterstitialAdLoadFailedEvent", MesonLog.AdLogEvent.LoadFailed, adUnitId, error);
        var evt = InterstitialAdLoadFailedEvent;
        if (evt != null) evt(adUnitId, error);
    }


    public void EmitInterstitialAdDismissedEvent(string argsJson)
    {
        var args = MesonUtils.DecodeArgs(argsJson, min: 1);
        var adUnitId = args[0];

        MesonLog.Log("EmitInterstitialAdDismissedEvent", MesonLog.AdLogEvent.Dismissed);
        var evt = InterstitialAdDismissedEvent;
        if (evt != null) evt(adUnitId);
    }

    public void EmitInterstitialAdDisplayFailedEvent(string argsJson)
    {
        var args = MesonUtils.DecodeArgs(argsJson, min: 1);
        var adUnitId = args[0];

        MesonLog.Log("EmitInterstitialAdDisplayFailedEvent", MesonLog.AdLogEvent.FailedToDisplay);
        var evt = InterstitialAdDisplayFailedEvent;
        if (evt != null) evt(adUnitId);
    }

    public void EmitInterstitialAdUserLeftApplicationEvent(string argsJson)
    {
        var args = MesonUtils.DecodeArgs(argsJson, min: 1);
        var adUnitId = args[0];

        MesonLog.Log("EmitInterstitialAdUserLeftApplicationEvent", MesonLog.AdLogEvent.UserLeftApplication);
        var evt = InterstitialAdUserLeftApplicationEvent;
        if (evt != null) evt(adUnitId);
    }

    public void EmitInterstitialAdDisplayedEvent(string argsJson)
    {
        var args = MesonUtils.DecodeArgs(argsJson, min: 1);
        var adUnitId = args[0];

        MesonLog.Log("EmitInterstitialAdDisplayedEvent", MesonLog.AdLogEvent.Displayed);
        var evt = InterstitialAdDisplayedEvent;
        if (evt != null) evt(adUnitId);
    }

    public void EmitInterstitialAdClickedEvent(string argsJson)
    {
        var args = MesonUtils.DecodeArgs(argsJson, min: 2);
        var adUnitId = args[0];
        var paramJson = args[1];
        Dictionary<string, object> param = new Dictionary<string, object>();

        if (paramJson.Length > 0)
        {
            param = MesonUtils.DecodeArgs(paramJson);
        }

        MesonLog.Log("EmitInterstitialAdClickedEvent", MesonLog.AdLogEvent.Tapped);
        var evt = InterstitialAdClickedEvent;
        if (evt != null) evt(adUnitId, param);
    }

    public void EmitRewardedVideoAdReceivedRewardsEvent(string argsJson)
    {
        var args = MesonUtils.DecodeArgs(argsJson, min: 2);
        var adUnitId = args[0];
        Dictionary<string, object> rewards = null;
        if (args.Length > 1)
        {
            rewards = MesonUtils.DecodeArgs(args[1]);
        }

        MesonLog.Log("EmitRewardedVideoAdReceivedRewardsEvent", MesonLog.AdLogEvent.ShouldReward,rewards.First().Key, ": "+rewards.First().Value);
       
        var evt = RewardedVideoAdReceivedRewardsEvent;
        if (evt != null) evt(adUnitId, rewards);
    }

    public void EmitBannerAdImpressionTrackedEvent(string argsJson)
    {
        var args = MesonUtils.DecodeArgs(argsJson, min: 1);
        var adUnitId = args[0];
        MesonAdData impressionData = null;
        if (args.Length > 1)
        {
            impressionData = MesonUtils.DecodeAdData(args[1]);
        }
        MesonLog.Log("EmitBannerAdImpressionTrackedEvent", MesonLog.AdLogEvent.Impression, ImpressionData(impressionData));
        var evt = BannerAdImpressionTrackedEvent;
        if (evt != null) evt(adUnitId, impressionData);
    }

    public void EmitInterstitialAdImpressionTrackedEvent(string argsJson)
    {
        var args = MesonUtils.DecodeArgs(argsJson, min: 1);
        var adUnitId = args[0];
        MesonAdData impressionData = null;
        if (args.Length > 1)
        {
            impressionData = MesonUtils.DecodeAdData(args[1]);
        }

        MesonLog.Log("EmitInterstitialAdImpressionTrackedEvent", MesonLog.AdLogEvent.Impression, ImpressionData(impressionData));
        var evt = InterstitialAdImpressionTrackedEvent;
        if (evt != null) evt(adUnitId, impressionData);
    }


    #endregion PlatformCallbacks

    private string ImpressionData(MesonAdData impressionData)
    {
        var impressionDataStr = "";
        IList<FieldInfo> fields = new List<FieldInfo>(impressionData.GetType().GetFields());
        foreach (FieldInfo field in fields)
        {
            object value = field.GetValue(impressionData);
            impressionDataStr += field.Name + ": " + value + ", ";
        }
        return impressionDataStr;
    }
}
