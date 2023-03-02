using UnityEngine;


public static class Axis
{
    internal const string Horizontal = "Horizontal", Vertical = "Vertical";
}

public static class AnimatorParams
{
    internal static readonly int Move = Animator.StringToHash("Move");
    internal static readonly int Start = Animator.StringToHash("Start");
}

public static class PlayerPrefsKey
{
    public const string GamePlayCount = "GamePlayCount";
    public const string LevelIndex = "LevelIndex";
    public const string GoogleMobileAdsiOSAppId = "[GoogleMobileAds]iOSAppId";
    public const string GoogleMobileAdsAndroidAppId = "[GoogleMobileAds]AndroidAppId";
    public const string Coins = "Coins";
    public const string Attempts = "Attempts";
    public const string Money = "Money";
    public const string UnlockCount = "UnlockCount";
}

public static class InputManager
{
    internal static bool IsOn
    {
        get
        {
            Vector2 input = Vector2.zero.With(x: InputX, y: InputY);
            return input.magnitude > 0;
        }
    }

    internal static float InputX =>
#if MOBILE_INPUT
        ETCInput.GetAxis("Horizontal");
#else
            ETCInput.GetAxis("Horizontal");
#endif

    internal static float InputY =>
#if MOBILE_INPUT
        ETCInput.GetAxis("Vertical");
#else
            ETCInput.GetAxis("Vertical");
#endif
}
