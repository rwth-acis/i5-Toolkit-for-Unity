using UnityEngine;
using System.Threading.Tasks;
using System;

#if I5_TOOLKIT_USE_AZURE_SPEECH_RECOGNIZER
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
#endif

namespace i5.Toolkit.Core.SpeechModule
{
    /// <summary>
    /// A speech recognizer (Speech-To-Text) using Azure Congitive Service and Speech SDK. See https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/index-speech-to-text
    /// Need subscribtion key and service region.
    /// </summary>
    public class AzureSpeechRecognizer : MonoBehaviour, ISpeechRecognizer
    {
        [Tooltip("You can find your subscription key on Azure Portal.")]
        [SerializeField] private string subscriptionKey;
        [Tooltip("You can find your service region on Azure Portal.")]
        [SerializeField] private string serviceRegion;
        [Tooltip("The Single Shot mode receives a silence as a stop symbol and only supports audio up to 15 seconds. The Continuous mode requires a manually stop.")]
        [SerializeField] private AzureRecognitionMode mode;

#if I5_TOOLKIT_USE_AZURE_SPEECH_RECOGNIZER
        private SpeechRecognizer speechRecognizer;
        private SpeechConfig speechConfig;
#endif
        void Start() {
#if I5_TOOLKIT_USE_AZURE_SPEECH_RECOGNIZER
            speechConfig = SpeechConfig.FromSubscription(subscriptionKey, serviceRegion);
#else
            Debug.LogError("The required Speech SDK for AzureSpeechRecognizer cannot be found, or the I5_TOOLKIT_USE_AZURE_SPEECH_RECOGNIZER directive is not defined on current platform.");
#endif
        }

        /// <summary>
        /// Fires when the recognizer receives the result.
        /// Please subscribe the OnRecognitionResultReceived event and avoid using the return value, because the result is empty for successful continuous recognition.
        /// </summary>
        public event Action<RecognitionResult> OnRecognitionResultReceived;

        /// <summary>
        /// Supported Language. You may also add any language you want.
        /// </summary>
        public Language Language { get; set; }

        /// Applicable if the component is enabled and there is an internet connection. <summary>
        public bool IsApplicable => enabled && Application.internetReachability != NetworkReachability.NotReachable;

        /// <summary>
        /// Start recording and recognizing according to the recognition mode.
        /// Please subscribe the OnRecognitionResultReceived event and avoid using the return value, because the result is empty for successful continuous recognition.
        /// Note that the continuous recognition is running in another thread, so you cannot call Unity APIs in the OnRecognitionResultReceived event. 
        /// However, you can use a Queue<Action> to enable it.
        /// </summary>
        /// <returns>The result of the recognition.</returns>
        public async Task<RecognitionResult> StartRecordingAsync() {
#if I5_TOOLKIT_USE_AZURE_SPEECH_RECOGNIZER
            RecognitionResult result;
            SourceLanguageConfig sourceLanguageConfig;
            switch (Language) {
                case Language.en_US:
                    sourceLanguageConfig = SourceLanguageConfig.FromLanguage("en-US");
                    break;
                case Language.de_DE:
                    sourceLanguageConfig = SourceLanguageConfig.FromLanguage("de-DE");
                    break;
                default:
                    sourceLanguageConfig = SourceLanguageConfig.FromLanguage("en-US");
                    break;
            }
            var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
            speechRecognizer = new SpeechRecognizer(speechConfig, sourceLanguageConfig, audioConfig);
            if (mode == AzureRecognitionMode.SingleShot) {
                result = await StartSingleShotRecordingAsync();
            }
            else {
                result = await StartContinuousRecordingAsync();
            }
            return result;
#else
            await Task.Run(() => Debug.LogError("The required Speech SDK for AzureSpeechRecognizer cannot be found, or the I5_TOOLKIT_USE_AZURE_SPEECH_RECOGNIZER directive is not defined on current platform."));
            return RecognitionResult.RequiredModulesNotFoundResult;
#endif
        }

        /// <summary>
        /// Stop recording. Only used for countinuous recognition.
        /// </summary>
        public async Task StopRecordingAsync() {
#if I5_TOOLKIT_USE_AZURE_SPEECH_RECOGNIZER
            if (mode == AzureRecognitionMode.Countinuous) {
                await speechRecognizer.StopContinuousRecognitionAsync();
            }
#else
            await Task.Run(() => Debug.LogError("The required Speech SDK for AzureSpeechRecognizer cannot be found, or the I5_TOOLKIT_USE_AZURE_SPEECH_RECOGNIZER directive is not defined on current platform."));
#endif
        }

#if I5_TOOLKIT_USE_AZURE_SPEECH_RECOGNIZER
        private async Task<RecognitionResult> StartSingleShotRecordingAsync() {
            Debug.Log("Speak into your microphone.");
            var speechRecognitionResult = await speechRecognizer.RecognizeOnceAsync();
            RecognitionResult result = ParseAzureRecognitionResult(speechRecognitionResult);
            OnRecognitionResultReceived?.Invoke(result);
            Debug.Log("Recognition Stopped.");
            return result;
        }

        private async Task<RecognitionResult> StartContinuousRecordingAsync() {
            Debug.Log("Speak into your microphone. Stop recording when finished.");
            RecognitionResult result = new RecognitionResult();
            var stopRecognition = new TaskCompletionSource<int>();
            speechRecognizer.Recognizing += (s, e) => Debug.Log($"RECOGNIZING: Text={e.Result.Text}");

            speechRecognizer.Recognized += (s, e) => OnRecognitionResultReceived?.Invoke(ParseAzureRecognitionResult(e.Result));

            speechRecognizer.Canceled += (s, e) =>
            {
                stopRecognition.TrySetResult(0);
                result = ParseAzureRecognitionResult(e.Result);
            };

            speechRecognizer.SessionStopped += (s, e) =>
            {
                Debug.Log("Recognition Stopped");
                stopRecognition.TrySetResult(0);
            };
            await speechRecognizer.StartContinuousRecognitionAsync();
            return result;

        }

        //Parse the SpeechRecognitionResult of Azure to our RecognitionResult.
        private RecognitionResult ParseAzureRecognitionResult(SpeechRecognitionResult speechRecognitionResult) {
            RecognitionResult result = new RecognitionResult();
            switch (speechRecognitionResult.Reason) {
                case ResultReason.RecognizedSpeech:
                    result.State = ResultState.Succeeded;
                    result.Text = speechRecognitionResult.Text;
                    result.Message = "Recognition Succeeded." + $" Text: {result.Text}";
                    break;
                case ResultReason.NoMatch:
                    result.State = ResultState.NoMatch;
                    result.Message = "No Match: Speech could not be recognized.";
                    break;
                case ResultReason.Canceled:
                    var cancellation = CancellationDetails.FromResult(speechRecognitionResult);
                    result.State = ResultState.Failed;
                    result.Message = $"Failed: Reason: {cancellation.Reason}";
                    if (cancellation.Reason == CancellationReason.Error) {
                        result.Message += $" AzureErrorCode={cancellation.ErrorCode}.\nDid you set the speech resource key and region values?";
                    }
                    break;
                default:
                    result.Message = result.Text;
                    break;
            }
            Debug.Log(result.Message);
            return result;
        }
#endif

        private enum AzureRecognitionMode
        {
            SingleShot,
            Countinuous
        }
    }
}
