using UnityEngine;

public class ToastToShow : MonoBehaviour
{
    private static AndroidJavaObject currentActivity;
    private static AndroidJavaObject toast;

    public static void ShowToast(string message)
    {
        if (currentActivity == null)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        }

        currentActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
        {
            AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
            toast = toastClass.CallStatic<AndroidJavaObject>("makeText", currentActivity, message, 0);
            toast.Call("show");
        }));
    }
}

