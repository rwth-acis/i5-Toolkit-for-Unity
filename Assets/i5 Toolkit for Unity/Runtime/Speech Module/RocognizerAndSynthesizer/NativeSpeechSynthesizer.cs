using System.Threading.Tasks;
using UnityEngine;
using SpeechLib;
using ylib.Services;
using HoloToolkit.Unity;
using System;

namespace i5.Toolkit.Core.SpeechModule
{
    /// <summary>
    /// A native speech synthesizer for Windows and Andorid, only supports English.
    /// On Windows Standalone and Unity Editor (only with Mono backend and .NET 4.x), it uses the Microsoft SAPI, see https://docs.microsoft.com/en-us/previous-versions/windows/desktop/ee125663(v=vs.85).
    /// On Universal Windows Platform, it uses the TextToSpeech from the HoloToolkit, see https://github.com/microsoft/MixedRealityToolkit-Unity/blob/htk_release/Assets/HoloToolkit/Utilities/Scripts/TextToSpeech.cs.
    /// On Android, it uses the repository https://github.com/nir-takemi/UnityTTS, it also supports IOS but we discarded it.
    /// </summary>
    public class NativeSpeechSynthesizer : MonoBehaviour, ISpeechSynthesizer
    {

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        private SpVoiceClass windowsSynthesizer;

        
#endif

#if !UNITY_EDITOR && UNITY_WSA
        private TextToSpeechUWP synthesizer;
#endif
        /// <summary>
        /// The native synthesizer only supports English on both Windows and Android.
        /// The synthesizer's language on Windows actually depends on the system language, and we can only use the voice tokens that are installed on users' devices.
        /// Since we don't know it for sure, we'd better only use English. All languages support English TTS.
        /// The tokens on your windows computer can be found in the Registry Editor (regedit) under HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Speech\Voices\Tokens
        /// </summary>
        public Language Language { get; set; }

        /// <summary>
        /// The native synthesizer can only output the audio to speakers.
        /// </summary>
        public AudioDataOutputForm OutputForm { get; set; }

        /// <summary>
        /// The native recognizer is applicable if the component is active.
        /// </summary>
        public bool IsApplicable => enabled && true;

        public event Action<SynthesisResult> OnSynthesisResultReceived;

        // Start is called before the first frame update
        void Start() {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            windowsSynthesizer = new SpVoiceClass();
#elif UNITY_WSA
            synthesizer = gameObject.AddComponent<TextToSpeechUWP>();
#elif UNITY_ANDROID
            UnityTTS.Init();
#endif
        }

        void OnDestroy() {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            windowsSynthesizer = null;
#endif
        }

        /// <summary>
        /// Start TTS. Please subscribe the OnSynthesisResultReceived Event because the return value is meaningless.
        /// </summary>
        public Task<SynthesisResult> StartSynthesizingAndSpeakingAsync(string text) {
            SynthesisResult result;
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            windowsSynthesizer.Speak(text, SpeechVoiceSpeakFlags.SVSFlagsAsync);
#elif UNITY_WSA
            //Only support output to stream and directly play it, not via SpeechProvider.
            synthesizer.StartSpeaking(text);
#elif UNITY_ANDROID
            //Only support output to speaker.
            UnityTTS.Speech(text);
#endif
            result = ParseNativeSynthesisResult(text);
            OnSynthesisResultReceived?.Invoke(result);
            return Task.FromResult(result);
        }

        private SynthesisResult ParseNativeSynthesisResult(string text) 
        {
            SynthesisResult result = new SynthesisResult();
            result.State = ResultState.Succeeded;
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_WSA
            result.Message = $"Speech synthesized for text: {text} using Windows native synthesizer";
#elif UNITY_ANDROID
            result.Message = $"Speech synthesized for text: {text} using Android native synthesizer";
#endif
            Debug.Log(result.Message);
            return result;
        }
       
    }
}
