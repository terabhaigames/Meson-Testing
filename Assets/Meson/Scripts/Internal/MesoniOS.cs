using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using UnityEngine;

#if UNITY_IOS

/// <summary>
/// Bridge between the Meson Unity Instance-wide API and iOS implementation.
/// </summary>
/// <para>
/// Publishers integrating with Meson should make all calls through the <see cref="Meson"/> class, and handle any
/// desired Meson Events from the <see cref="MesonManager"/> class.
/// </para>
/// <para>
/// For other platform-specific implementations, see <see cref="MesonUnityEditor"/> and <see cref="MesonAndroid"/>.
/// </para>
[SuppressMessage("ReSharper", "UnusedMember.Global")]
internal class MesoniOS : MesonPlatformApi
{
#region SdkSetup

    internal override void SetLocation(double latitude, double longitude)
    {
        _MesonSetLocation(latitude, longitude);
    }

    internal override void InitializeSdk(Meson.SdkConfiguration sdkConfiguration)
    {
        _MesonInitializeSdk(sdkConfiguration.IOSAppId,
                            sdkConfiguration.ConsentString,
                            (int)sdkConfiguration.LogLevel);
    }

    internal override void UpdateGDPRConsent(Meson.SdkConfiguration sdkConfiguration)
    {
        _MesonUpdateGDPRConsent(sdkConfiguration.ConsentString);
    }

    internal override void SetExtras(Meson.SdkConfiguration sdkConfiguration)
    {
        _MesonSetExtras(sdkConfiguration.ExtrasString);
    }

    internal override string GetExtras()
    {
        return _MesonGetExtras();
    }

    internal override void CleanUserInfo()
    {
        _MesonCleanUserInfo();
    }

    internal override string GetPPID()
    {
        return _MesonGetPPID();
    }

    internal override void SetPPID(string pPID)
    {
        _MesonSetPPID(pPID);
    }    

    internal override string SdkName
    {
        get { return "iOS SDK v" + _MesonGetSDKVersion(); }
    }

    internal override bool IsSdkInitialized
    {
        get { return _MesonIsSdkInitialized(); }
    }


    internal override void SetSdkLogLevel(MesonBase.LogLevel logLevel) {
        _MesonSetLogLevel((int)logLevel);
    }

#endregion SdkSetup

#region DllImports

    [DllImport("__Internal")]
    private static extern void _MesonSetLocation(double latitude, double longitude);

    [DllImport("__Internal")]
    private static extern void _MesonInitializeSdk(string appId, string consentString,
        int logLevel);

    [DllImport("__Internal")]
    private static extern void _MesonUpdateGDPRConsent(string gdprConsentJson);

    [DllImport("__Internal")]
    private static extern void _MesonSetExtras(string extrasJson);

    [DllImport("__Internal")]
    private static extern string _MesonGetExtras();

    [DllImport("__Internal")]
    private static extern void _MesonSetPPID(string pPID);

    [DllImport("__Internal")]
    private static extern string _MesonGetPPID();

    [DllImport("__Internal")]
    private static extern void _MesonCleanUserInfo();

    [DllImport("__Internal")]
    private static extern bool _MesonIsSdkInitialized();


    [DllImport("__Internal")]
    private static extern string _MesonGetSDKVersion();


    [DllImport("__Internal")]
    private static extern void _MesonSetLogLevel(int logLevel);

#endregion DllImports
}

#endif
