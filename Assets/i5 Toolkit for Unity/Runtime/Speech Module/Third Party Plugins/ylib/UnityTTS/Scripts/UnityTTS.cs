using UnityEngine;

#if !UNITY_EDITOR && UNITY_IOS
using System.Runtime.InteropServices;
#endif

namespace ylib.Services
{

    public static class UnityTTS
    {

        public static float Volume = 100.0f;        // only iOS
        public static float Rate = 0.5f;            // base on iOS
        public static float Pitch = 1.0f;
        public static string Language = "en-us";    // only iOS

        /// <summary>
        /// 初期化処理。Speechの直前ではなく、余裕を持って呼び出す必要性あり。
        /// </summary>
        public static void Init()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.yasuragitei.tts.UnityTTSPlugin"))
            {
                androidJavaClass.CallStatic("Init");
            }
#endif
        }

        /// <summary>
        /// 指定テキストの再生
        /// </summary>
        /// <param name="text"></param>
        public static void Speech(string text)
        {
            // 余計な処理はさせないようにする
            if((text == null) || (text.Length == 0))
            {
                Debug.LogError("[UnityTTS]引数にnullか空文字が指定されました");
                return;
            }

#if !UNITY_EDITOR && UNITY_IOS
            _speechText(text, Volume, Rate, Pitch, Language);
#elif !UNITY_EDITOR && UNITY_ANDROID
            _speechText(text, Volume, Rate, Pitch, Language);
#else
            Debug.Log("[UnityTTS]Play:" + text);
#endif
        }

#if !UNITY_EDITOR && UNITY_IOS
        [DllImport ("__Internal")]
        static extern void _speechText(string text, float volume, float rate, float pitch, string language);
#elif !UNITY_EDITOR && UNITY_ANDROID
        public static void _speechText(string text, float volume, float rate, float pitch, string language)
        {
            using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.yasuragitei.tts.UnityTTSPlugin"))
            {
                androidJavaClass.CallStatic("SpeechToText", text, rate*2f, pitch);
            }
        }
        public static void OnInitSuccess(string message)
        {
            Debug.Log(message);
        }
        public static void OnError(string message)
        {
            Debug.LogError(message);
        }
#endif
    }
}
