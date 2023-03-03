using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardedAdIncome : MonoBehaviour
{
string[] incomerewardedAdUnits = {"700fee22-0486-4e9f-8a9b-9a18bff69856","5f3e291b-d531-4d84-94d3-db0549e685c9"}; 
string incomerewardedAdUnitId;
//public IncomeAd IncAd;

void Awake()
{

#if UNITY_IOS
    incomerewardedAdUnitId = "700fee22-0486-4e9f-8a9b-9a18bff69856";
#elif UNITY_ANDROID
    incomerewardedAdUnitId = "5f3e291b-d531-4d84-94d3-db0549e685c9";
#endif
}
void Start()
{
    MesonManager.SdkInitializedEvent += OnSdkInitializedEvent;  
    Meson.LoadInterstitialPluginsForAdUnits(incomerewardedAdUnits);
    InitializeRewardedAds();
}
private void OnSdkInitializedEvent(string adUnitId)
{
    // The SDK is initialized here. Ready to make ad requests.
    
    LoadRewardedIncome();
}

public void LoadRewardedIncome()
{
    Meson.RequestInterstitialAd(incomerewardedAdUnitId);
}

public void ShowRewardedIncome()
{
    Meson.ShowInterstitialAd(incomerewardedAdUnitId);   
}

public void InitializeRewardedAds()
{
    MesonManager.InterstitialAdLoadedEvent += IncomeInterstitialAdLoaded;
    MesonManager.InterstitialAdLoadFailedEvent += IncomeInterstitialAdLoadFailed;
    MesonManager.InterstitialAdDismissedEvent += IncomeInterstitialAdDismissed;
    MesonManager.InterstitialAdClickedEvent += IncomeInterstitialAdClicked;
    MesonManager.InterstitialAdDisplayFailedEvent += IncomeInterstitialAdDisplayFailed;
    MesonManager.InterstitialAdUserLeftApplicationEvent += IncomeInterstitialAdUserLeftApplication;
    MesonManager.InterstitialAdDisplayedEvent += IncomeInterstitialAdDisplayed;
    MesonManager.InterstitialAdImpressionTrackedEvent += IncomeInterstitialAdImpressionTracked;
    MesonManager.RewardedVideoAdReceivedRewardsEvent += IncomeRewardedVideoAdReceived;
}

private void IncomeInterstitialAdLoaded(string adUnitId)
{
    
    //IncomeAd.AdReady = true;
}

private void IncomeInterstitialAdLoadFailed(string adUnitId, string message)
{
    //IncomeAd.AdReady = false;
}

private void IncomeInterstitialAdDismissed(string adUnitId){}

private void IncomeInterstitialAdClicked(string adunitId, Dictionary<string, object> networkData){}

private void IncomeInterstitialAdDisplayFailed(string adUnitId){}

private void IncomeInterstitialAdUserLeftApplication(string adUnitId){}

private void IncomeInterstitialAdDisplayed(string adUnitId){}

private void IncomeInterstitialAdImpressionTracked(string adUnitId, MesonAdData adData){}

private void IncomeRewardedVideoAdReceived(string adunitId, Dictionary<string, object> networkdata)
    {
        var impressionDataStr = "";

        foreach (string key in networkdata.Keys)
        {
            impressionDataStr += key + ": " + networkdata[key] + "\n";
        }

        MesonLog.Log(adunitId + "with newtork data: "
            + impressionDataStr, "Message");

        MesonLog.Log("Callback Fired", "RewardedAd");
        //IncAd.Income_Ad();
    }
}

