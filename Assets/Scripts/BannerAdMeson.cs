using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BannerAdMeson : MonoBehaviour
{
    string[] bannerAdUnits = {"dbfd4dee-7785-419e-ae1c-924a20283375","3eed747a-9709-4ca0-a5b6-a2df49261e9b"};
    string bannerAdUnitId;

    void Awake()
    {

#if UNITY_IOS
    bannerAdUnitId = "dbfd4dee-7785-419e-ae1c-924a20283375";
#elif UNITY_ANDROID
    bannerAdUnitId = "3eed747a-9709-4ca0-a5b6-a2df49261e9b";
#endif
    
    }
    void Start()
    {
        MesonManager.SdkInitializedEvent += OnSdkInitializedEvent;
        Meson.LoadBannerPluginsForAdUnits(bannerAdUnits);   
    }

    private void OnSdkInitializedEvent(string adUnitId)
    {
        // The SDK is initialized here. Ready to make ad requests.
        // RequestBanner();
        InitializeBannerAds();
    }

   public void RequestBanner()
   {
        Meson.RequestBanner(bannerAdUnitId, MesonBase.AdPosition.BottomCenter, MesonBase.AdSize.Banner);
   }

   public void DestroyBanner()
   {
        Meson.DestroyBanner(bannerAdUnitId);
   }

    public void InitializeBannerAds()
    {
        MesonManager.BannerAdLoadedEvent += BannerAdLoaded;
        MesonManager.BannerAdLoadFailedEvent += BannerAdLoadFailed;
        MesonManager.BannerAdClickedEvent += BannerAdClicked;
        MesonManager.BannerAdUserLeftApplicationEvent += BannerAdUserLeftApplication;
        MesonManager.BannerAdPresentedScreenEvent += BannerAdPresentedScreen;
        MesonManager.BannerAdCollapsedScreenEvent += BannerAdCollapsedScreen;
        MesonManager.BannerAdImpressionTrackedEvent += BannerAdImpressionTracked;
    }


    private void BannerAdLoaded(string adUnitId){} 

    private void BannerAdLoadFailed(string adUnitId, string message){}

    private void BannerAdClicked(string adunitId, Dictionary<string, object> networkData){}

    private void BannerAdUserLeftApplication(string adUnitId){}

    private void BannerAdPresentedScreen(string adUnitId){}

    private void BannerAdCollapsedScreen(string adUnitId){}

    private void BannerAdImpressionTracked(string adUnitId, MesonAdData adData){}

}