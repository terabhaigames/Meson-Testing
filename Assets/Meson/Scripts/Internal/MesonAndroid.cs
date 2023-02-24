using System.Diagnostics.CodeAnalysis;
using UnityEngine;

/// <summary>
/// Bridge between the Meson Unity Instance-wide API and Android implementation.
/// </summary>
/// <para>
/// Publishers integrating with Meson should make all calls through the <see cref="Meson"/> class, and handle any
/// desired Meson Events from the <see cref="MesonManager"/> class.
/// </para>
/// <para>
/// For other platform-specific implementations, see <see cref="MesonUnityEditor"/> and <see cref="MesoniOS"/>.
/// </para>
[SuppressMessage("ReSharper", "UnusedMember.Global")]
internal class MesonAndroid : MesonPlatformApi
{
    private static readonly AndroidJavaClass PluginClass = new AndroidJavaClass("ai.meson.unity.MesonUnityPlugin");

    #region SdkSetup

    internal override void SetLocation(double latitude, double longitude)
    {
        PluginClass.CallStatic("setLocation", latitude, longitude);
    }

    internal override void SetExtras(MesonBase.SdkConfiguration sdkConfiguration)
    {
        PluginClass.CallStatic("setExtras", sdkConfiguration.ExtrasString);
    }

    internal override string GetExtras()
    {
        return PluginClass.CallStatic<string>("getExtras"); ;
    }

    internal override void CleanUserInfo()
    {
        PluginClass.CallStatic("cleanUserInfo");
    }

    internal override string GetPPID()
    {
        return PluginClass.CallStatic<string>("getPPID");
    }

    internal override void SetPPID(string pPID)
    {
        PluginClass.CallStatic("setPPID", pPID);

    }

    internal override void InitializeSdk(Meson.SdkConfiguration sdkConfiguration)
    {
        PluginClass.CallStatic(
            "initializeSdk", sdkConfiguration.AndroidAppId,
            sdkConfiguration.ConsentString,
            (int) sdkConfiguration.LogLevel);
    }

    internal override void UpdateGDPRConsent(MesonBase.SdkConfiguration sdkConfiguration)
    {
        PluginClass.CallStatic("updateGDPRConsent", sdkConfiguration.ConsentString);
    }

    internal override string SdkName
    {
        get { return "Android SDK v" + PluginClass.CallStatic<string>("getSDKVersion"); }
    }


    internal override bool IsSdkInitialized
    {
        get { return PluginClass.CallStatic<bool>("isSdkInitialized"); }
    }


    internal override void SetSdkLogLevel(MesonBase.LogLevel logLevel)
    {
        PluginClass.CallStatic("setLogLevel", (int)logLevel);
    }


    #endregion SdkSetup
}
