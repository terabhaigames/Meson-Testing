using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
// ReSharper disable AccessToStaticMemberViaDerivedType

public static class MesonLog
{
    public static class SdkLogEvent
    {
        public const string InitStarted = "SDK initialization started";
        public const string InitFinished = "SDK initialized and ready to display ads.";
    }

    public static class AdLogEvent
    {
        public const string LoadAttempted = "Attempting to load ad";
        public const string LoadSuccess = "Ad loaded";
        public const string LoadFailed = "Ad failed to load: ({0}) {1}";
        public const string ShowAttempted = "Attempting to show ad";
        public const string ShowSuccess = "Ad shown";
        public const string Tapped = "Ad tapped";
        public const string Expanded = "Ad expanded";
        public const string Collapsed = "Ad has been collapsed";
        public const string Presented = "Ad has been presented";
        public const string Dismissed = "Ad did disappear";
        public const string ShouldReward = "Ad should reward user with {0} {1}";
        public const string Expired = "Ad expired since it was not shown fast enough";
        public const string UserLeftApplication = "User is taken away from the application";
        public const string FailedToDisplay = "Ad failed to display";
        public const string Displayed = "Ad has been displayed";
        public const string Impression = "Ad has given impression with data {0}";
        public const string InvalidAdType = "FATAL ERROR: Invalid ad type for MesonAdUnit: {0}";
        public const string InvalidCallToMesonAPI = "Invalid call to Meson API; {0} AdUnit with ID {1} has not been requested yet.";
        public const string ExtrasNull = "Extras is null or invalid";
        public const string AdUnitNotFound = "AdUnit {0} not found: no plugin was initialized";
        public const string SDKNotInitialized = "FATAL ERROR: Meson SDK has not been successfully initialized.";
        public const string InvalidJson = "Invalid JSON data: {0}";
        public const string MissingValues = "Missing one or more values: {0} (expected {1})";
        public const string AdUnitsAlreadyInitialized = "WARNING: Skipping {0} AdUnit with id {1} since it was already initialized";
        public const string AdUnitsInitialized = "Initialized {0} AdUnit with id {1}";
    }

    private static readonly Dictionary<string, Meson.LogLevel> logLevelMap =
        new Dictionary<string, Meson.LogLevel>
    {
        { SdkLogEvent.InitStarted, Meson.LogLevel.Debug },
        { SdkLogEvent.InitFinished, Meson.LogLevel.Debug },
        { AdLogEvent.LoadAttempted, Meson.LogLevel.Debug },
        { AdLogEvent.LoadSuccess, Meson.LogLevel.Debug },
        { AdLogEvent.LoadFailed, Meson.LogLevel.Debug },
        { AdLogEvent.ShowAttempted, Meson.LogLevel.Debug },
        { AdLogEvent.ShowSuccess, Meson.LogLevel.Debug },
        { AdLogEvent.Tapped, Meson.LogLevel.Debug },
        { AdLogEvent.Expanded, Meson.LogLevel.Debug },
        { AdLogEvent.Collapsed, Meson.LogLevel.Debug },
        { AdLogEvent.Presented, Meson.LogLevel.Debug },
        { AdLogEvent.UserLeftApplication, Meson.LogLevel.Debug },
        { AdLogEvent.Dismissed, Meson.LogLevel.Debug },
        { AdLogEvent.Displayed, Meson.LogLevel.Debug },
        { AdLogEvent.FailedToDisplay, Meson.LogLevel.Debug },
        { AdLogEvent.ShouldReward, Meson.LogLevel.Debug },
        { AdLogEvent.Expired, Meson.LogLevel.Debug },
        { AdLogEvent.Impression, Meson.LogLevel.Debug },
        { AdLogEvent.InvalidAdType, Meson.LogLevel.Error },
        {AdLogEvent.InvalidCallToMesonAPI, Meson.LogLevel.Error },
        {AdLogEvent.ExtrasNull, Meson.LogLevel.Error },
        {AdLogEvent.AdUnitNotFound, Meson.LogLevel.Error },
        {AdLogEvent.SDKNotInitialized, Meson.LogLevel.Error },
        {AdLogEvent.InvalidJson, Meson.LogLevel.Error },
        {AdLogEvent.MissingValues, Meson.LogLevel.Error },
        {AdLogEvent.AdUnitsAlreadyInitialized, Meson.LogLevel.Debug },
        {AdLogEvent.AdUnitsInitialized, Meson.LogLevel.Debug },
    };

    public static void Log(string callerMethod, string message, params object[] args)
    {
        Meson.LogLevel messageLogLevel;
        if (!logLevelMap.TryGetValue(message, out messageLogLevel))
            messageLogLevel = Meson.LogLevel.Debug;

        if (Meson.CachedLogLevel > messageLogLevel) return;

        var formattedMessage = "[Meson-Unity] [" + callerMethod + "] " + message;
        try {
            Debug.LogFormat(formattedMessage, args);
        } catch (FormatException) {
            Debug.Log("Format exception while logging message { " + formattedMessage + " } with arguments { " +
                       string.Join(",", args.Select(a => a.ToString()).ToArray()) + " }");
        }
    }
}
