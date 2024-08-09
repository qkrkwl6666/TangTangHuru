using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vibration : MonoBehaviour
{
    public static void Vibrate(float seconds)
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaObject vibrator = GetVibrator();
        if (vibrator != null)
        {
            long milliseconds = (long)(seconds * 1000);
            vibrator.Call("vibrate", milliseconds);
        }
        #endif
    }

    public static void VibratePattern(long[] pattern, int repeat)
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaObject vibrator = GetVibrator();
        if (vibrator != null)
        {
            vibrator.Call("vibrate", pattern, repeat);
        }
        #endif
    }

    private static AndroidJavaObject GetVibrator()
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");
        return context.Call<AndroidJavaObject>("getSystemService", "vibrator");
    }

    public static void Cancel()
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaObject vibrator = GetVibrator();
        if (vibrator != null)
        {
            vibrator.Call("cancel");
        }
        #endif
    }
}
