using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardedAdMeson : MonoBehaviour
{
    string[] rewardedAdUnits = {"1fd55b41-d176-4a03-9c34-a0d11af29a25","029cd5c2-12d0-4bfe-8571-2a3a08ef8e3d"};
    string rewardedAdUnitId;

    void Awake()
    {

#if UNITY_IOS
    rewardedAdUnitId = "1fd55b41-d176-4a03-9c34-a0d11af29a25";
#elif UNITY_ANDROID
    rewardedAdUnitId = "029cd5c2-12d0-4bfe-8571-2a3a08ef8e3d";
#endif
    
    }
    void Start()
    {
        MesonManager.SdkInitializedEvent += OnSdkInitializedEvent;
        Meson.LoadInterstitialPluginsForAdUnits(rewardedAdUnits);        
    }
    private void OnSdkInitializedEvent(string adUnitId)
    {
        // The SDK is initialized here. Ready to make ad requests.  
        //InvokeRepeating("LoadRewarded", 5f,5f);
        //InvokeRepeating("ShowRewarded", 7f,7f);
        InitializeRewardedAds();
    }

   public void LoadRewarded()
   {
        Meson.RequestInterstitialAd(rewardedAdUnitId);
   }

   public void ShowRewarded()
   {
        Meson.ShowInterstitialAd(rewardedAdUnitId);
   }  

   public void InitializeRewardedAds()
    {
        MesonManager.InterstitialAdLoadedEvent += InterstitialAdLoaded;
        MesonManager.InterstitialAdLoadFailedEvent += InterstitialAdLoadFailed;
        MesonManager.InterstitialAdDismissedEvent += InterstitialAdDismissed;
        MesonManager.InterstitialAdClickedEvent += InterstitialAdClicked;
        MesonManager.InterstitialAdDisplayFailedEvent += InterstitialAdDisplayFailed;
        MesonManager.InterstitialAdUserLeftApplicationEvent += InterstitialAdUserLeftApplication;
        MesonManager.InterstitialAdDisplayedEvent += InterstitialAdDisplayed;
        MesonManager.InterstitialAdImpressionTrackedEvent += InterstitialAdImpressionTracked;
        MesonManager.RewardedVideoAdReceivedRewardsEvent += RewardedVideoAdReceivedRewards;
    }

    private void InterstitialAdLoaded(string adUnitId){} 

    private void InterstitialAdLoadFailed(string adUnitId, string message){}

    private void InterstitialAdDismissed(string adUnitId){}

    private void InterstitialAdClicked(string adunitId, Dictionary<string, object> networkData){}

    private void InterstitialAdDisplayFailed(string adUnitId){}

    private void InterstitialAdUserLeftApplication(string adUnitId){}

    private void InterstitialAdDisplayed(string adUnitId){}

    private void InterstitialAdImpressionTracked(string adUnitId, MesonAdData adData){}

    private void RewardedVideoAdReceivedRewards(string adunitId, Dictionary<string, object> networkData)
    {
        
    }
}