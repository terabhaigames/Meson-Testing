using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterstitialAdMeson : MonoBehaviour
{
string[] interstitialAdUnits = {"b93edb8f-6d5f-4891-8224-c7b05ca6c515","6d38a20e-8460-4c0c-abcd-aad7be414d8d"};
string interstitialAdUnitId;

void Awake()
{

#if UNITY_IOS
    interstitialAdUnitId = "b93edb8f-6d5f-4891-8224-c7b05ca6c515";
#elif UNITY_ANDROID
    interstitialAdUnitId = "6d38a20e-8460-4c0c-abcd-aad7be414d8d";
#endif
}

void Start()
{
    MesonManager.SdkInitializedEvent += OnSdkInitializedEvent;
    Meson.LoadInterstitialPluginsForAdUnits(interstitialAdUnits);

    InvokeRepeating("LoadInterstitial", 120f,120f);
    InvokeRepeating("ShowInterstitial", 123f, 123f);
}

private void OnSdkInitializedEvent(string adUnitId)
{
    // The SDK is initialized here. Ready to make ad requests.
    
    InitializeInterstitialAds();
}

public void LoadInterstitial()
{
    Meson.RequestInterstitialAd(interstitialAdUnitId);
}

public void ShowInterstitial()
{
    Meson.ShowInterstitialAd(interstitialAdUnitId);
}

public void InitializeInterstitialAds()
{
    MesonManager.InterstitialAdLoadedEvent += InterstitialAdLoaded;
    MesonManager.InterstitialAdLoadFailedEvent += InterstitialAdLoadFailed;
    MesonManager.InterstitialAdDismissedEvent += InterstitialAdDismissed;
    MesonManager.InterstitialAdClickedEvent += InterstitialAdClicked;
    MesonManager.InterstitialAdDisplayFailedEvent += InterstitialAdDisplayFailed;
    MesonManager.InterstitialAdUserLeftApplicationEvent += InterstitialAdUserLeftApplication;
    MesonManager.InterstitialAdDisplayedEvent += InterstitialAdDisplayed;
    MesonManager.InterstitialAdImpressionTrackedEvent += InterstitialAdImpressionTracked;
}

private void InterstitialAdLoaded(string adUnitId){}

private void InterstitialAdLoadFailed(string adUnitId, string message){}

private void InterstitialAdDismissed(string adUnitId){}

private void InterstitialAdClicked(string adunitId, Dictionary<string, object> networkData){}

private void InterstitialAdDisplayFailed(string adUnitId){}

private void InterstitialAdUserLeftApplication(string adUnitId){}

private void InterstitialAdDisplayed(string adUnitId){}

private void InterstitialAdImpressionTracked(string adUnitId, MesonAdData adData){}
}