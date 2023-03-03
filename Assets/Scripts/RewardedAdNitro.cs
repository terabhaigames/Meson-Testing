using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardedAdNitro : MonoBehaviour
{
string[] nitrorewardedAdUnits = {"1fd55b41-d176-4a03-9c34-a0d11af29a25","029cd5c2-12d0-4bfe-8571-2a3a08ef8e3d"};
string nitrorewardedAdUnitId;
//public NitroAd ad;

void Awake()
{

#if UNITY_IOS
    nitrorewardedAdUnitId = "1fd55b41-d176-4a03-9c34-a0d11af29a25";
#elif UNITY_ANDROID
    nitrorewardedAdUnitId = "029cd5c2-12d0-4bfe-8571-2a3a08ef8e3d";
#endif
}
void Start()
{
    MesonManager.SdkInitializedEvent += OnSdkInitializedEvent;
    Meson.LoadInterstitialPluginsForAdUnits(nitrorewardedAdUnits);

}
private void OnSdkInitializedEvent(string adUnitId)
{
    // The SDK is initialized here. Ready to make ad requests.
    InitializeRewardedAds();
    LoadRewardedNitro();
}

public void LoadRewardedNitro()
{
    Meson.RequestInterstitialAd(nitrorewardedAdUnitId);
}

public void ShowRewardedNitro()
{
    Meson.ShowInterstitialAd(nitrorewardedAdUnitId);
}

public void InitializeRewardedAds()
{
    MesonManager.InterstitialAdLoadedEvent += NitroInterstitialAdLoaded;
    MesonManager.InterstitialAdLoadFailedEvent += NitroInterstitialAdLoadFailed;
    MesonManager.InterstitialAdDismissedEvent += NitroInterstitialAdDismissed;
    MesonManager.InterstitialAdClickedEvent += NitroInterstitialAdClicked;
    MesonManager.InterstitialAdDisplayFailedEvent += NitroInterstitialAdDisplayFailed;
    MesonManager.InterstitialAdUserLeftApplicationEvent += NitroInterstitialAdUserLeftApplication;
    MesonManager.InterstitialAdDisplayedEvent += NitroInterstitialAdDisplayed;
    MesonManager.InterstitialAdImpressionTrackedEvent += NitroInterstitialAdImpressionTracked;
    MesonManager.RewardedVideoAdReceivedRewardsEvent += NitroRewardedVideoAdReceived;
}

private void NitroInterstitialAdLoaded(string adUnitId)
{
    //NitroAd.AdReady = true;
}

private void NitroInterstitialAdLoadFailed(string adUnitId, string message)
{
    //NitroAd.AdReady = false;
}

private void NitroInterstitialAdDismissed(string adUnitId){}

private void NitroInterstitialAdClicked(string adunitId, Dictionary<string, object> networkData){}

private void NitroInterstitialAdDisplayFailed(string adUnitId){}

private void NitroInterstitialAdUserLeftApplication(string adUnitId){}

private void NitroInterstitialAdDisplayed(string adUnitId){}

private void NitroInterstitialAdImpressionTracked(string adUnitId, MesonAdData adData){}

private void NitroRewardedVideoAdReceived(string adunitId, Dictionary<string, object> networkdata)
    {
        var impressionDataStr = "";

        foreach (string key in networkdata.Keys)
        {
            impressionDataStr += key + ": " + networkdata[key] + "\n";
        }

        MesonLog.Log(adunitId + "with newtork data: "
            + impressionDataStr, "Message");

        MesonLog.Log("Callback Fired", "RewardedAd");
        //ad.Nitro_Ad();
    } 
}