//
//  MesonBinding.m
//  Meson
//
//  Copyright (c) 2017 Meson. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <CoreLocation/CoreLocation.h>

#import "MesonManager.h"
#import <MesonSDK/MesonSDK.h>
#import "MesonUtils.h"

///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark - Helpers

// Ensures cross-platform compatiblity (specifically, in simulators) via explicit casting
static bool MesonExplicitBool(BOOL objcBool)
{
#if TARGET_OS_SIMULATOR
    NSLog(@"[Meson-Unity] Preventing compiler optimizations on simulators from ignoring explicit casting by printing value: %d", objcBool);
#endif
    return (objcBool ? true : false);
}

///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark - SDK Setup
void _MesonInitializeSdk(const char* appIdString, const char* consentJson, int logLevel)
{

    NSString* appId = GetStringParam(appIdString);
    NSDictionary *gdprConsent = [MesonUtils getDictionaryFromJson:consentJson];

    MesonSdkConfiguration *sdkConfiguration = [[MesonSdkConfiguration alloc] initWithAppId:appId consent:gdprConsent];

    void (^completionBlock)(NSError*) = ^( NSError* _Nullable  error) {
        if (error) {
            NSLog(@"[Meson-Unity] %@", error.description);
        } else {
            [Meson setLogLevel:logLevel];
            [MesonManager sendUnityEvent:@"EmitSdkInitializedEvent" withArgs:@[appId]];
        }
    };

    [Meson initializeWithSdkConfiguration:sdkConfiguration completion:completionBlock];
}

void _MesonSetLogLevel(int logLevel) {
    [Meson setLogLevel:logLevel];
}

void _MesonUpdateGDPRConsent(const char* gdprConsentJson) {
    NSDictionary *gdprConsent = [MesonUtils getDictionaryFromJson:gdprConsentJson];
    if (gdprConsent != NULL) {
        [Meson updateGDPRConsent:gdprConsent];
    }
}

void _MesonSetLocation(double latitude, double longitude) {
    [Meson setLocation:[[CLLocation alloc] initWithLatitude:latitude longitude:longitude]];
}

bool _MesonIsSdkInitialized()
{
    return MesonExplicitBool([Meson isSDKInitialized]);
}

const char* _MesonGetSDKVersion()
{
    return [MesonUtils cStringCopy:[Meson getSDKVersion]];
}

void _MesonSetExtras(char *extrasJson) {
    NSDictionary *extras = [MesonUtils getDictionaryFromJson:extrasJson];
    if (extras != NULL) {
        [Meson setExtras: extras];
    }
}

char* _MesonGetExtras() {
    NSDictionary *extras = [Meson getExtras];
    if (extras == nil) {
        return nil;
    }
    return [MesonUtils cStringCopy:[MesonUtils stringFromDict:extras]];
}

void _MesonSetPPID(char *pPID) {
    [Meson setPPID: GetStringParam(pPID)];
}

char* _MesonGetPPID() {
    NSString* pPID = [Meson getPPID];
    return [MesonUtils cStringCopy:pPID];
}

void _MesonCleanUserInfo() {
    [Meson cleanUserInfo];
}

bool _MesonIsPluginReady(const char* adUnitId)
{
    return true;
}

///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark - Banners

void _MesonRequestBanner(float width, float height, int bannerPosition, const char* adUnitId)
{
    MesonAdPosition position = (MesonAdPosition)bannerPosition;
    
    [[MesonManager managerForAdunit:GetStringParam(adUnitId)] requestBanner:width
                                                                     height:height
                                                                 atPosition:position];
}

void _MesonDestroyBanner(const char* adUnitId)
{
    [[MesonManager managerForAdunit:GetStringParam(adUnitId)] destroyBanner];
}

char* _MesonBannerAdData(const char* adUnitId) {
    MesonAdData *adData = [[MesonManager managerForAdunit:GetStringParam(adUnitId)] bannerAdData];
    if (adData == nil) {
        return nil;
    }
    NSDictionary *dict = [MesonUtils dictionaryFromObject:adData];
    return [MesonUtils cStringCopy:[MesonUtils stringFromDict:dict]];
}

///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark - Interstitials

void _MesonRequestInterstitialAd(const char* adUnitId)
{
    [[MesonManager managerForAdunit:GetStringParam(adUnitId)] requestInterstitialAd];
}


bool _MesonIsInterstitialReady(const char* adUnitId)
{
    MesonManager* mgr = [MesonManager managerForAdunit:GetStringParam(adUnitId)];
    return mgr != nil && [mgr interstitialIsReady];
}


void _MesonShowInterstitialAd(const char* adUnitId)
{
    [[MesonManager managerForAdunit:GetStringParam(adUnitId)] showInterstitialAd];
}


void _MesonDestroyInterstitialAd(const char* adUnitId)
{
    [[MesonManager managerForAdunit:GetStringParam(adUnitId)] destroyInterstitialAd];
}

char* _MesonInterstitialAdData(const char* adUnitId) {
    MesonAdData *adData = [[MesonManager managerForAdunit:GetStringParam(adUnitId)] interstitialAdData];
    if (adData == nil) {
        return nil;
    }
    NSDictionary *dict = [MesonUtils dictionaryFromObject:adData];
    return [MesonUtils cStringCopy:[MesonUtils stringFromDict:dict]];
}
