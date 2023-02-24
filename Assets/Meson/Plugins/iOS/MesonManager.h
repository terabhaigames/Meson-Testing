//
//  MesonManager.h
//  Meson
//
//  Copyright (c) 2017 Meson. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import <MesonSDK/MesonSDK-Swift.h>

typedef enum
{
    MesonAdSize_50Height,
    MesonAdSize_90Height,
    MesonAdSize_250Height,
    MesonAdSize_280Height
} MesonAdSize;

typedef enum
{
    MesonAdPositionTopLeft,
    MesonAdPositionTopCenter,
    MesonAdPositionTopRight,
    MesonAdPositionCentered,
    MesonAdPositionBottomLeft,
    MesonAdPositionBottomCenter,
    MesonAdPositionBottomRight
} MesonAdPosition;

@interface MesonManager : NSObject <MesonBannerDelegate,MesonInterstitialDelegate>
{
@private
    NSString* _adUnitId;
}
@property (nonatomic, strong) MesonBanner* banner;
@property (nonatomic, strong) MesonInterstitial* interstitial;
@property (nonatomic) MesonAdPosition bannerPosition;

+ (MesonManager*)sharedManager;

+ (MesonManager*)managerForAdunit:(NSString*)adUnitId;

+ (UIViewController*)unityViewController;

+ (void)sendUnityEvent:(NSString*)eventName withArgs:(NSArray*)args;

- (void)sendUnityEvent:(NSString*)eventName;

- (id)initWithAdUnit:(NSString*)adUnitId;

- (void)requestBanner:(float)width height:(float)height atPosition:(MesonAdPosition)position;

- (void)destroyBanner;

- (void)requestInterstitialAd;

- (BOOL)interstitialIsReady;

- (void)showInterstitialAd;

- (void)destroyInterstitialAd;

- (MesonAdData*)bannerAdData;

- (MesonAdData*)interstitialAdData;

@end
