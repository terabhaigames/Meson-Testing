using System.Collections;
using MJ = MesonInternal.ThirdParty.MiniJSON;
#if UNITY_EDITOR
using System;

/// <summary>
/// Bridge between the Meson Unity Instance-wide API and In-Editor implementation.
/// </summary>
/// <para>
/// Publishers integrating with Meson should make all calls through the <see cref="Meson"/> class, and handle any
/// desired Meson Events from the <see cref="MesonManager"/> class.
/// </para>
/// <para>
/// For other platform-specific implementations, see <see cref="MesonAndroid"/> and <see cref="MesoniOS"/>.
/// </para>
/// <remarks>
/// Some properties have added public setters in order to facilitate testing in Play mode.
/// </remarks>
internal class MesonUnityEditor : MesonPlatformApi
{
    #region SdkSetup

    private static bool _isInitialized;

    internal override void InitializeSdk(Meson.SdkConfiguration sdkConfiguration)
    {
        WaitOneFrame(() => {
            _isInitialized = true;
            MesonManager.Instance.EmitSdkInitializedEvent(ArgsToJson(sdkConfiguration.IOSAppId,
                                                              ((int) sdkConfiguration.LogLevel).ToString()));
        });
    }

    internal override void UpdateGDPRConsent(MesonBase.SdkConfiguration sdkConfiguration) { }

    internal override void SetExtras(MesonBase.SdkConfiguration sdkConfiguration) { }

    internal override void SetLocation(double latitude, double longitude) { }

    internal override string GetExtras() { return ""; }

    internal override void CleanUserInfo() { }

    internal override string GetPPID() { return ""; }

    internal override void SetPPID(string pPID) { }

    internal override string SdkName
    {
        get { return "no SDK loaded (not on a mobile device)"; }
    }


    internal override bool IsSdkInitialized
    {
        get { return _isInitialized; }
    }


    internal override void SetSdkLogLevel(MesonBase.LogLevel logLevel) { }


    #endregion SdkSetup


    #region MockEditor
    private static IEnumerator WaitOneFrameCoroutine(Action action)
    {
        yield return null;
        action();
    }


    public static void WaitOneFrame(Action action)
    {
        MesonManager.Instance.StartCoroutine(WaitOneFrameCoroutine(action));
    }

    public static void SimulateApplicationResume()
    {
        WaitOneFrame(() => {
            MesonLog.Log("SimulateApplicationResume", "Simulating application resume.");
        });
    }


    public static string ArgsToJson(params string[] args)
    {
        return MJ.Json.Serialize(args);
    }

    


    #endregion MockEditor
}
#endif
