//
//  MesonUtils.h
//  UnityFramework
//
//  Created by Abdul Basit on 27/06/22.
//

#ifndef MesonUtils_h
#define MesonUtils_h

#import <objc/runtime.h>
#import <Foundation/Foundation.h>

// Converts C style string to NSString
#define GetStringParam(_x_) ((_x_) != NULL ? [NSString stringWithUTF8String:_x_] : [NSString stringWithUTF8String:""])
#define GetNullableStringParam(_x_) ((_x_) != NULL ? [NSString stringWithUTF8String:_x_] : nil)

@interface MesonUtils : NSObject
+(char*) cStringCopy: (NSString*) input;
+(NSMutableDictionary<NSString*, NSDictionary<NSString*, id>*>*) getDictionaryFromJson:(const char*) json;
+(NSDictionary *) dictionaryFromObject:(id)obj;
+(NSString *) stringFromDict: (NSDictionary*) dict;
@end

#endif /* MesonUtils_h */
