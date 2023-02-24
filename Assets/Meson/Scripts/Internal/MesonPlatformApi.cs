using System.Collections.Generic;
/// <summary>
/// Bridge between the Meson Unity Instance-wide API and platform-specific implementations.
/// </summary>
/// <para>
/// Publishers integrating with Meson should make all calls through the <see cref="Meson"/> class, and handle any
/// desired Meson Events from the <see cref="MesonManager"/> class.
/// </para>
/// <para>
/// For platform-specific implementations, see
/// <see cref="MesonUnityEditor"/>, <see cref="MesonAndroid"/>, and <see cref="MesoniOS"/>.
/// </para>
internal abstract class MesonPlatformApi
{


    #region SdkSetup

    internal abstract void InitializeSdk(Meson.SdkConfiguration sdkConfiguration);

    internal abstract void UpdateGDPRConsent(Meson.SdkConfiguration sdkConfiguration);

    internal abstract void SetExtras(Meson.SdkConfiguration sdkConfiguration);

    internal abstract void SetSdkLogLevel(Meson.LogLevel logLevel);

    internal abstract string GetExtras();

    internal abstract void SetPPID(string pPID);

    internal abstract string GetPPID();

    internal abstract void CleanUserInfo();

    internal abstract void SetLocation(double latitude, double longitude);

    internal abstract string SdkName { get; }

    internal abstract bool IsSdkInitialized { get; }

    #endregion SdkSetup

}
