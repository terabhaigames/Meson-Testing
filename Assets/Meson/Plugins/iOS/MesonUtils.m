//
//  MesonUtils.m
//  UnityFramework
//
//  Created by Abdul Basit on 27/06/22.
//

#import <Foundation/Foundation.h>
#import "MesonUtils.h"

@implementation MesonUtils

// Converts an NSString into a const char* ready to be sent to Unity
+(char*) cStringCopy: (NSString*) input
{
    const char* string = [input UTF8String];
    return string ? strdup(string) : NULL;
}

+(NSMutableDictionary<NSString*, NSDictionary<NSString*, id>*>*) getDictionaryFromJson:(const char*) json
{
    NSString* jsonString = GetStringParam(json);
    if (jsonString.length == 0)
        return nil;
    NSMutableDictionary<NSString*, NSDictionary<NSString*, id>*>* dict =
        [NSJSONSerialization JSONObjectWithData:[jsonString dataUsingEncoding:NSUTF8StringEncoding]
                                        options:NSJSONReadingMutableContainers
                                          error:nil];
    return dict.count > 0 ? dict : nil;
}

+(NSDictionary *) dictionaryFromObject:(id)obj
{
    if (obj == nil)
        return nil;
    
    NSMutableDictionary *dict = [NSMutableDictionary dictionary];

    unsigned count;
    objc_property_t *properties = class_copyPropertyList([obj class], &count);

    for (int i = 0; i < count; i++) {
        NSString *key = [NSString stringWithUTF8String:property_getName(properties[i])];
        if([obj valueForKey:key] != nil) {
            [dict setObject:[obj valueForKey:key] forKey:key];
        }
    }

    return [NSDictionary dictionaryWithDictionary:dict];
}

+(NSString *) stringFromDict: (NSDictionary*) dict
{
    if([NSJSONSerialization isValidJSONObject:dict]) {
        NSData *data = [NSJSONSerialization dataWithJSONObject:dict options:NSJSONWritingPrettyPrinted error:nil];
        NSString * jsonString = [[NSString alloc] initWithData:data
                                                  encoding:NSUTF8StringEncoding];
        return jsonString;
    }
    return nil;
}

@end
