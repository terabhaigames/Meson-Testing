//
//  MesonManager.m
//  Meson
//
//  Copyright (c) 2017 Meson. All rights reserved.
//

#import "MesonManager.h"
#import <MesonSDK/MesonSDK-Swift.h>
#import "MesonUtils.h"

#ifdef __cplusplus
extern "C" {
#endif
    // life cycle management
    void UnityPause(int pause);
    void UnitySendMessage(const char* obj, const char* method, const char* msg);
#ifdef __cplusplus
}
#endif

@implementation MesonManager
@synthesize interstitial = _interstitial, banner = _banner;
///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark NSObject

// Manager to be used for methods that do not require a specific adunit to operate on.
+ (MesonManager*)sharedManager
{
    static MesonManager* sharedManager = nil;

    if (!sharedManager)
        sharedManager = [[MesonManager alloc] init];

    return sharedManager;
}

// Manager to be used for adunit specific methods
+ (MesonManager*)managerForAdunit:(NSString*)adUnitId
{
    static NSMutableDictionary* managerDict = nil;

    if (!managerDict)
        managerDict = [[NSMutableDictionary alloc] init];
    MesonManager* manager = [managerDict valueForKey:adUnitId];
    if (!manager) {
        manager = [[MesonManager alloc] initWithAdUnit:adUnitId];
        managerDict[adUnitId] = manager;
    }

    return manager;
}


+ (UIViewController*)unityViewController
{
    return [[[UIApplication sharedApplication] keyWindow] rootViewController];
}

+ (void)sendUnityEvent:(NSString*)eventName withArgs:(NSArray*)args
{
    NSData* data = [NSJSONSerialization dataWithJSONObject:args options:0 error:nil];
    NSString* json = [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding];
   
    UnitySendMessage("MesonManager", eventName.UTF8String, json.UTF8String);
}

- (void)sendUnityEvent:(NSString*)eventName
{
    [[self class] sendUnityEvent:eventName withArgs:@[_adUnitId]];
}

///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark Private

- (void)adjustAdViewFrameToShowAdView
{
    if (@available(iOS 11.0, *)) {
        UIView* superview = _banner.superview;
        if (superview) {
            _banner.translatesAutoresizingMaskIntoConstraints = NO;
            NSMutableArray<NSLayoutConstraint*>* constraints = [NSMutableArray arrayWithArray:@[
                [_banner.widthAnchor constraintEqualToConstant:CGRectGetWidth(_banner.frame)],
                [_banner.heightAnchor constraintEqualToConstant:CGRectGetHeight(_banner.frame)],
            ]];
            switch(_bannerPosition) {
                case MesonAdPositionTopLeft:
                    [constraints addObjectsFromArray:@[[_banner.topAnchor constraintEqualToAnchor:superview.safeAreaLayoutGuide.topAnchor],
                                                       [_banner.leftAnchor constraintEqualToAnchor:superview.safeAreaLayoutGuide.leftAnchor]]];
                    break;
                case MesonAdPositionTopCenter:
                    [constraints addObjectsFromArray:@[[_banner.topAnchor constraintEqualToAnchor:superview.safeAreaLayoutGuide.topAnchor],
                                                       [_banner.centerXAnchor constraintEqualToAnchor:superview.safeAreaLayoutGuide.centerXAnchor]]];
                    break;
                case MesonAdPositionTopRight:
                    [constraints addObjectsFromArray:@[[_banner.topAnchor constraintEqualToAnchor:superview.safeAreaLayoutGuide.topAnchor],
                                                       [_banner.rightAnchor constraintEqualToAnchor:superview.safeAreaLayoutGuide.rightAnchor]]];
                    break;
                case MesonAdPositionCentered:
                    [constraints addObjectsFromArray:@[[_banner.centerXAnchor constraintEqualToAnchor:superview.safeAreaLayoutGuide.centerXAnchor],
                                                       [_banner.centerYAnchor constraintEqualToAnchor:superview.safeAreaLayoutGuide.centerYAnchor]]];
                    break;
                case MesonAdPositionBottomLeft:
                    [constraints addObjectsFromArray:@[[_banner.bottomAnchor constraintEqualToAnchor:superview.safeAreaLayoutGuide.bottomAnchor],
                                                       [_banner.leftAnchor constraintEqualToAnchor:superview.safeAreaLayoutGuide.leftAnchor]]];
                    break;
                case MesonAdPositionBottomCenter:
                    [constraints addObjectsFromArray:@[[_banner.bottomAnchor constraintEqualToAnchor:superview.safeAreaLayoutGuide.bottomAnchor],
                                                       [_banner.centerXAnchor constraintEqualToAnchor:superview.safeAreaLayoutGuide.centerXAnchor]]];
                    break;
                case MesonAdPositionBottomRight:
                    [constraints addObjectsFromArray:@[[_banner.bottomAnchor constraintEqualToAnchor:superview.safeAreaLayoutGuide.bottomAnchor],
                                                       [_banner.rightAnchor constraintEqualToAnchor:superview.safeAreaLayoutGuide.rightAnchor]]];
                    break;
            }
            [NSLayoutConstraint activateConstraints:constraints];
            NSLog(@"[Meson-Unity] setting adView frame: %@", NSStringFromCGRect(_banner.frame));
        } else
            NSLog(@"[Meson-Unity] _banner.superview was nil! Was the ad view not added to another view?@");
    } else {
        // fetch screen dimensions and useful values
        CGRect origFrame = _banner.frame;

        CGFloat screenHeight = [UIScreen mainScreen].bounds.size.height;
        CGFloat screenWidth = [UIScreen mainScreen].bounds.size.width;

        switch(_bannerPosition) {
            case MesonAdPositionTopLeft:
                origFrame.origin.x = 0;
                origFrame.origin.y = 0;
                _banner.autoresizingMask = (UIViewAutoresizingFlexibleRightMargin | UIViewAutoresizingFlexibleBottomMargin);
                break;
            case MesonAdPositionTopCenter:
                origFrame.origin.x = (screenWidth / 2) - (origFrame.size.width / 2);
                _banner.autoresizingMask = (UIViewAutoresizingFlexibleLeftMargin | UIViewAutoresizingFlexibleRightMargin | UIViewAutoresizingFlexibleBottomMargin);
                break;
            case MesonAdPositionTopRight:
                origFrame.origin.x = screenWidth - origFrame.size.width;
                origFrame.origin.y = 0;
                _banner.autoresizingMask = (UIViewAutoresizingFlexibleLeftMargin | UIViewAutoresizingFlexibleBottomMargin);
                break;
            case MesonAdPositionCentered:
                origFrame.origin.x = (screenWidth / 2) - (origFrame.size.width / 2);
                origFrame.origin.y = (screenHeight / 2) - (origFrame.size.height / 2);
                _banner.autoresizingMask = (UIViewAutoresizingFlexibleRightMargin | UIViewAutoresizingFlexibleLeftMargin | UIViewAutoresizingFlexibleTopMargin | UIViewAutoresizingFlexibleBottomMargin);
                break;
            case MesonAdPositionBottomLeft:
                origFrame.origin.x = 0;
                origFrame.origin.y = screenHeight - origFrame.size.height;
                _banner.autoresizingMask = (UIViewAutoresizingFlexibleRightMargin | UIViewAutoresizingFlexibleTopMargin);
                break;
            case MesonAdPositionBottomCenter:
                origFrame.origin.x = (screenWidth / 2) - (origFrame.size.width / 2);
                origFrame.origin.y = screenHeight - origFrame.size.height;
                _banner.autoresizingMask = (UIViewAutoresizingFlexibleLeftMargin | UIViewAutoresizingFlexibleRightMargin | UIViewAutoresizingFlexibleTopMargin);
                break;
            case MesonAdPositionBottomRight:
                origFrame.origin.x = screenWidth - _banner.frame.size.width;
                origFrame.origin.y = screenHeight - origFrame.size.height;
                _banner.autoresizingMask = (UIViewAutoresizingFlexibleLeftMargin | UIViewAutoresizingFlexibleTopMargin);
                break;
        }

        _banner.frame = origFrame;
        NSLog(@"[Meson-Unity] setting adView frame: %@", NSStringFromCGRect(origFrame));
    }
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark Public

- (id)initWithAdUnit:(NSString*)adUnitId
{
    self = [super init];
    if (self) self->_adUnitId = adUnitId;
    return self;
}

- (void)requestBanner:(float)width height:(float)height atPosition:(MesonAdPosition)position {
    // kill the current adView if we have one
    if (_banner)
        [self destroyBanner];

    _bannerPosition = position;

    CGSize requestedBannerSize = CGSizeMake(width, height);
    _banner = [[MesonBanner alloc] initWithAdUnitId:_adUnitId adSize:requestedBannerSize];

    _banner.delegate = self;
    [[MesonManager unityViewController].view addSubview:_banner];
    [_banner load];
}

- (void)destroyBanner
{
    [_banner removeFromSuperview];
    _banner.delegate = nil;
    self.banner = nil;
}

- (void)requestInterstitialAd
{
    _interstitial = [[MesonInterstitial alloc] initWithAdUnitId:_adUnitId delegate:self];
    [_interstitial load];
}


- (BOOL)interstitialIsReady
{
    return _interstitial.isReady;
}


- (void)showInterstitialAd
{
    [_interstitial showFromViewController:[MesonManager unityViewController]];
}


- (void)destroyInterstitialAd
{
    _interstitial = nil;
}

- (MesonAdData*)interstitialAdData
{
    return _interstitial.adData;
}

- (MesonAdData*)bannerAdData
{
    return _banner.adData;
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark - MesonBannerDelegate
- (void)mesonBannerDidLoad:(MesonBanner * )banner {
    _banner = banner;

    [self adjustAdViewFrameToShowAdView];
    [_banner setNeedsLayout];
    _banner.hidden = NO;

    [[self class] sendUnityEvent:@"EmitBannerAdLoadedEvent" withArgs:@[_adUnitId, @(_banner.frame.size.width), @(_banner.frame.size.height)]];
}

- (void)mesonBannerDidFailToLoad:(MesonBanner * )banner error:(NSError * )error {
    _banner.hidden = YES;
    [[self class] sendUnityEvent:@"EmitBannerAdLoadFailedEvent" withArgs:@[_adUnitId, error.localizedDescription]];
}

- (void)mesonBannerDidClick:(MesonBanner * )banner params:(NSDictionary<NSString *, id> * _Nullable)params {
    NSString * paramsStr = [MesonUtils stringFromDict:params];
   
    if (paramsStr != nil)
        [[self class] sendUnityEvent:@"EmitBannerAdClickedEvent" withArgs:@[_adUnitId, paramsStr]];
    else
        [self sendUnityEvent:@"EmitBannerAdClickedEvent"];
}

- (void)mesonBannerUserWillLeaveApplication:(MesonBanner * )banner {
    [self sendUnityEvent:@"EmitBannerAdUserLeftApplicationEvent"];
}

- (void)mesonBannerWillPresentScreen:(MesonBanner * )banner {
    //NO OP
}

- (void)mesonBannerDidPresentScreen:(MesonBanner * )banner {
    [self sendUnityEvent:@"EmitBannerAdPresentedScreenEvent"];
}

- (void)mesonBannerWillCollapseScreen:(MesonBanner * )banner {
    //NO OP
}

- (void)mesonBannerDidCollapseScreen:(MesonBanner * )banner {
    [self sendUnityEvent:@"EmitBannerAdCollapsedScreenEvent"];
    UnityPause(false);
    [self destroyBanner];
}

- (void)mesonBannerImpression:(MesonBanner *)banner adData:(MesonAdData *)adData {
    NSDictionary *dict = [MesonUtils dictionaryFromObject:adData];
    NSString *adDataStr = [MesonUtils stringFromDict:dict];
    
    if (adDataStr != nil)
        [[self class] sendUnityEvent:@"EmitBannerAdImpressionTrackedEvent" withArgs:@[_adUnitId, adDataStr]];
     else
         [self sendUnityEvent:@"EmitBannerAdImpressionTrackedEvent"];
}

- (UIViewController * )viewControllerForMesonBannerFullScreen {
    return [MesonManager unityViewController];
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark - MesonInterstitialDelegate
- (void)mesonInterstitialDidLoad:(MesonInterstitial * )interstitial {
    [self sendUnityEvent:@"EmitInterstitialAdLoadedEvent"];
}

- (void)mesonInterstitialDidFailToLoad:(MesonInterstitial * )interstitial error:(NSError * )error {
    [[self class] sendUnityEvent:@"EmitInterstitialAdLoadFailedEvent" withArgs:@[_adUnitId, error.localizedDescription]];
}

- (void)mesonInterstitialDidFailToDisplay:(MesonInterstitial * )interstitial error:(NSError * )error {
    [[self class] sendUnityEvent:@"EmitInterstitialAdDisplayFailedEvent" withArgs: @[_adUnitId, error.localizedDescription]];
}

- (void)mesonInterstitialDidClick:(MesonInterstitial * )interstitial params:(NSDictionary<NSString *, id> * _Nullable)params {
    NSString * paramsStr = [MesonUtils stringFromDict:params];
   
    if (paramsStr != nil)
        [[self class] sendUnityEvent:@"EmitInterstitialAdClickedEvent" withArgs:@[_adUnitId, paramsStr]];
    else
        [self sendUnityEvent:@"EmitInterstitialAdClickedEvent"];
}

- (void)mesonInterstitialUserWillLeaveApplication:(MesonInterstitial * )interstitial {
    [self sendUnityEvent:@"EmitInterstitialAdUserLeftApplicationEvent"];
}

- (void)mesonInterstitialWillDisplay:(MesonInterstitial * )interstitial {
    //NO OP
}

- (void)mesonInterstitialDidDisplay:(MesonInterstitial * )interstitial {
    [self sendUnityEvent:@"EmitInterstitialAdDisplayedEvent"];
    UnityPause(true);
}

- (void)mesonInterstitialWillDismiss:(MesonInterstitial * )interstitial {
    //NO OP
}

- (void)mesonInterstitialDidDismiss:(MesonInterstitial * )interstitial {
    [self sendUnityEvent:@"EmitInterstitialAdDismissedEvent"];
    UnityPause(false);
}

- (void)mesonInterstitialImpression:(MesonInterstitial *)interstitial adData:(MesonAdData *)adData {
    NSDictionary *dict = [MesonUtils dictionaryFromObject:adData];
    NSString *adDataStr = [MesonUtils stringFromDict:dict];
    
    if (adDataStr != nil)
        [[self class] sendUnityEvent:@"EmitInterstitialAdImpressionTrackedEvent" withArgs:@[_adUnitId, adDataStr]];
     else
         [self sendUnityEvent:@"EmitInterstitialAdImpressionTrackedEvent"];
}

- (void)mesonRewardsUnlocked:(MesonInterstitial * )interstitial rewards:(NSDictionary<NSString *, id> * )rewards {
    NSString *rewardsStr = [MesonUtils stringFromDict:rewards];
    
    if (rewardsStr != nil)
        [[self class] sendUnityEvent:@"EmitRewardedVideoAdReceivedRewardsEvent"
                        withArgs:@[_adUnitId, rewardsStr]];
    else
        [self sendUnityEvent:@"EmitRewardedVideoAdReceivedRewardsEvent"];
}

@end

