using UnityEngine;

using AdSize = Meson.AdSize;

public static class MesonAdSizeMapping
{
    public static float Width(this AdSize adSize)
    {
        switch (adSize)
        {
            case AdSize.Banner:
            case AdSize.LargeBanner:
                return 320;
            case AdSize.MediumRectangle:
                return 300;
            case AdSize.FullSizeBanner:
                return 468;
            case AdSize.Leaderboard:
                return 728;
            default:
                // fallback to default size: Banner
                return 320;
        }
    }


    public static float Height(this AdSize adSize)
    {
        switch (adSize)
        {
            case AdSize.Banner:
                return 50;
            case AdSize.LargeBanner:
                return 100;
            case AdSize.MediumRectangle:
                return 250;
            case AdSize.FullSizeBanner:
                return 60;
            case AdSize.Leaderboard:
                return 90;
            default:
                // fallback to default size: Banner
                return 50;
        }
    }
}
