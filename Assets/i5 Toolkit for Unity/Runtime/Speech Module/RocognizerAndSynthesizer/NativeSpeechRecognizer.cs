using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

#if I5_TOOLKIT_USE_NATIVE_SPEECH_RECOGNIZER
using Vosk;
#endif

namespace i5.Toolkit.Core.SpeechModule
{
    /// <summary>
    /// A cross-platform native speech recognizer using the Vosk library. https://alphacephei.com/vosk/index
    /// The models can also be found on the website.
    /// It may crash on the first time because of decompressing issue of the models.
    /// </summary>
    public class NativeSpeechRecognizer : MonoBehaviour, ISpeechRecognizer
    {
        [Tooltip("The path of the English model. It should be just the name with extension of the zip file, which located under the Assets/StreamingAssets folder.")]
        [SerializeField] private string voskModelPathEn_US = "vosk-model-small-en-us-0.15.zip";
        [Tooltip("The path of the German model. It should be just the name with extension of the zip file, which located under the Assets/StreamingAssets folder.")]
        [SerializeField] private string voskModelPathDe_DE = "vosk-model-small-de-0.15.zip";
#if I5_TOOLKIT_USE_NATIVE_SPEECH_RECOGNIZER
        private VoskSpeechToText recognizer;
#endif
        /// <summary>
        /// The supported Language, depends on the downloaded models. You can add any language and corresponding model on Vosk's website.
        /// </summary>
        public Language Language { get; set; }

        /// <summary>
        /// The native recognizer is applicable if the component is active.
        /// </summary>
        public bool IsApplicable => enabled && true;

        public event Action<RecognitionResult> OnRecognitionResultReceived;

        // Start is called before the first frame update
        void Start()
        {
#if I5_TOOLKIT_USE_NATIVE_SPEECH_RECOGNIZER
            VoiceProcessor voiceProcesser = gameObject.AddComponent<VoiceProcessor>();
            recognizer = gameObject.AddComponent<VoskSpeechToText>();
            recognizer.VoiceProcessor = voiceProcesser;
            recognizer.OnTranscriptionResult += GetVoskRecognitionResult;
            recognizer.OnStatusUpdated += OnStatusUpdated;
            recognizer.MaxAlternatives = 1;
            recognizer.MaxRecordLength = 15;
            switch (Language) {
                case Language.en_US:
                    recognizer.ModelPath = voskModelPathEn_US;
                    break;
                case Language.de_DE:
                    recognizer.ModelPath = voskModelPathDe_DE;
                    break;
                default:
                    recognizer.ModelPath = voskModelPathEn_US;
                    break;
            }
            recognizer.StartVoskStt();
#else
            Debug.LogError("The required Vosk models for NativeSpeechRecognizer cannot be found, or the I5_TOOLKIT_USE_NATIVE_SPEECH_RECOGNIZER directive is not defined on current platform.");
#endif
        }

        /// <summary>
        /// Start recording. There is no return value and we must subscribe the OnTranscriptionResultEvent.
        /// </summary>
        public Task<RecognitionResult> StartRecordingAsync() {
#if I5_TOOLKIT_USE_NATIVE_SPEECH_RECOGNIZER
            recognizer.StartRecording();
            RecognitionResult result = new RecognitionResult
            {
                State = ResultState.Succeeded,
                Text = "The native recognizer (Vosk) has no return value, please subscribe to the OnRecognitionResultReceived event"
            };
            return Task.FromResult(result);
#else
            Debug.LogError("The required Vosk models for NativeSpeechRecognizer cannot be found, or the I5_TOOLKIT_USE_NATIVE_SPEECH_RECOGNIZER directive is not defined on current platform.");
            return Task.FromResult(RecognitionResult.RequiredModulesNotFoundResult);
#endif
        }

        /// <summary>
        /// Stop recording and return a CompletedTask.
        /// </summary>
        public Task StopRecordingAsync() {
#if I5_TOOLKIT_USE_NATIVE_SPEECH_RECOGNIZER
            recognizer.StopRecording();
#else
            Debug.LogError("The required Vosk models for NativeSpeechRecognizer cannot be found, or the I5_TOOLKIT_USE_NATIVE_SPEECH_RECOGNIZER directive is not defined on current platform.");
#endif
            return Task.CompletedTask;
        }

#if I5_TOOLKIT_USE_NATIVE_SPEECH_RECOGNIZER
        //Callback for the vosk recognizer.
        private void GetVoskRecognitionResult(string recognizedText) {
            RecognitionResult result = ParseVoskRecognitionResult(recognizedText);
            OnRecognitionResultReceived?.Invoke(result);
        }
        
        private void OnStatusUpdated(string status) {
            Debug.Log($"Vosk Recognizer: {status}");
        }

        private RecognitionResult ParseVoskRecognitionResult(string recognizedText) {
            RecognitionResult result = new RecognitionResult();
            result.State = ResultState.Succeeded;
            RecognizedTextJson recognizedTextJson = JsonUtility.FromJson<RecognizedTextJson>(recognizedText);
            result.Text = recognizedTextJson.alternatives[0].text;
            result.Message = "Recognition Succeeded." + $" Text={result.Text}";
            Debug.Log(result.Message);
            return result;
        }
#endif
        //Json objects for parsing the recognition result.
        [Serializable]
        private class RecognizedTextJson
        {
            public List<AlternativesJson> alternatives;
        }

        [Serializable]
        private class AlternativesJson
        {
            public string confidence = null;
            public string result = null;
            public string text = null;
        }

    }

}
