using System.Collections.Generic;
using UnityEngine;
using MiniJson = MesonInternal.ThirdParty.MiniJSON;

/// <summary>
/// Support classes used by the <see cref="Meson"/> Unity API for publishers.
/// </summary>
public abstract class MesonBase : MesonBaseInternal
{
    public enum AdPosition
    {
        TopLeft,
        TopCenter,
        TopRight,
        Centered,
        BottomLeft,
        BottomCenter,
        BottomRight
    }

    /// <summary>
    /// The ad size, in density-independent pixels (DIPs), an ad should have.
    /// Banner > 320x50
    /// Large Banner > 320x100
    /// Medium Rectangle > 300x250
    /// Full-size banner > 468x60
    /// Leaderboard > 728x90
    /// </summary>
    public enum AdSize
    {
        Banner,
        LargeBanner,
        MediumRectangle,
        FullSizeBanner,
        Leaderboard
    }


    public enum LogLevel
    {
        None = 0,
        Error = 1,
        Debug = 2,
    }

    /// <summary>
    /// Data object holding any SDK initialization parameters.
    /// </summary>
    public class SdkConfiguration
    {
        /// <summary>
        /// Any app id that your app uses for Android.
        /// </summary>
        public string AndroidAppId;

        /// <summary>
        /// Any app id that your app uses for iOS.
        /// </summary>
        public string IOSAppId;

        /// <summary>
        /// Consent to be passed to the app.
        /// </summary>
        public Dictionary<string, string> ConsentDict;

        /// <summary>
        /// Extra data to be passed to the app.
        /// </summary>
        public Dictionary<string, object> ExtrasDict;

        /// <summary>
        /// Meson SDK log level. Defaults to Meson.<see cref="Meson.LogLevel.None"/>
        /// </summary>
        public LogLevel LogLevel
        {
            get { return _logLevel != 0 ? _logLevel : LogLevel.None; }
            set { _logLevel = value; }
        }

        private LogLevel _logLevel = 0;

        public string ConsentString
        {
            get
            {
                return MiniJson.Json.Serialize(ConsentDict);
            }
        }

        public string ExtrasString
        {
            get
            {
                return MiniJson.Json.Serialize(ExtrasDict);
            }
        }

        public Dictionary<string, object> GetExtrasDict(string extras)
        {
            try
            {
                return MiniJson.Json.Deserialize(extras) as Dictionary<string, object>;
            }
            catch
            {
                MesonLog.Log("GetExtrasDict",MesonLog.AdLogEvent.ExtrasNull);
            }
            return new Dictionary<string, object>();
        }

    }

}