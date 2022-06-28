using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System;

namespace i5.Toolkit.Core.SpeechModule
{
    /// <summary>
    /// The SpeechProvider requires at least one ISpeechRecognizer and one ISpeechSynthesizer on the same gameObject.
    /// The ones with higher priorities should be placed on top of other recognizers and synthesizers in the inspector.
    /// It manages the ISpeechRecognizer and ISpeechSynthesizer and exposes their functionalities to users.
    /// So developers only need to re-implement their own ISpeechRecognizer and ISpeechSynthesizer if needed, and don't need to care about the user-interaction aspects.
    /// There maybe also other settings (SerializeField) on the recognizer and synthesizer.
    /// </summary>
    [RequireComponent(typeof(ISpeechRecognizer))]
    [RequireComponent(typeof(ISpeechSynthesizer))]
    public class SpeechProvider : MonoBehaviour
    {
        [Tooltip("Prefered Language, some might not be supported by some recognizers or synthesizers.")]
        [SerializeField] private Language language;
        [Tooltip("Set the primary audio data output form. It will be propagated to all synthesizers. But it may not have any effects due to synthesizer's implementation, " +
            "e.g. some may not support byte stream as the output format. " +
            "For \"AsByteStream\", the output will be converted to an audio clip and can be played by an Audio Source.")]
        [SerializeField] private AudioDataOutputForm primaryAudioOutputForm;

        private ISpeechRecognizer[] recognizers;
        private ISpeechRecognizer currentRecognizer;
        private ISpeechSynthesizer[] synthesizers;
        private ISpeechSynthesizer currentSynthesizer;

        private AudioSource audioSource;

        /// <summary>
        /// By subscribing, it distributed the subscribtion to all recognizers.
        /// Don't subscribe in Awake();
        /// </summary>
        public event Action<RecognitionResult> OnRecognitionResultReceived
        {
            add
            {
                foreach (var recognizer in recognizers) {
                    recognizer.OnRecognitionResultReceived += value;
                }
            }
            remove
            {
                foreach (var recognizer in recognizers) {
                    recognizer.OnRecognitionResultReceived -= value;
                }
            }
        }

        /// <summary>
        /// By subscribing, it distributed the subscribtion to all synthesizers.
        /// Don't subscribe in Awake();
        /// </summary>
        public event Action<SynthesisResult> OnSynthesisResultReceived
        {
            add
            {
                foreach(var synthesizer in synthesizers) {
                    synthesizer.OnSynthesisResultReceived += value;
                }
            }
            remove
            {
                foreach(var synthesizer in synthesizers) {
                    synthesizer.OnSynthesisResultReceived -= value;
                }
            }
        }

        /// <summary>
        /// Current Language. By setting this property, it sets the Language property of all recognizers and synthesizers.
        /// Don't set in Awake();
        /// </summary>
        public Language Language
        {
            get => language;
            set
            {
                language = value;
                foreach(ISpeechRecognizer recognizer in recognizers) {
                    recognizer.Language = value;
                }
                foreach(ISpeechSynthesizer synthesizer in synthesizers) {
                    synthesizer.Language = value;
                }
            }
        }

        /// <summary>
        /// Primary audio output form. By setting this property, it sets this property of all recognizers and synthesizers.
        /// Don't set in Awake();
        /// </summary>
        public AudioDataOutputForm PrimaryAudioOutputForm
        {
            get => primaryAudioOutputForm;
            set
            {
                primaryAudioOutputForm = value;
                foreach (ISpeechSynthesizer synthesizer in synthesizers) {
                    synthesizer.OutputForm = value;
                }
            }
        }

        private void Awake() {
            InitializeRecognizerAndSynthesizer();
        }

        // Start is called before the first frame update
        void Start() {
            //We just add a new one because even if the gameObject has already an AudioSource, it can be used in other purpose.
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = 1;
        }

        /// <summary>
        /// Start recording and recognizing the user's voice.
        /// If the current recognizer is not applicable, it will try to switch to another applicable one but not start re-recognizing since the sound data is not buffered locally.
        /// User may restart recording.
        /// </summary>
        public async Task<RecognitionResult> StartRecordingAsync() {
            RecognitionResult result = await currentRecognizer.StartRecordingAsync();
            if(result.State == ResultState.Failed && !currentRecognizer.IsApplicable) {
                SwitchToApplicableRecognizer();
            }
            return result;
        }

        public async Task StopRecordingAsync() {
            await currentRecognizer.StopRecordingAsync();
        }

        /// <summary>
        /// Synthesizing the given text and speaking the audio out.
        /// If the current synthesizer is not applicable, it will try to switch to another applicable one and automatically restart synthesizing.
        /// </summary>
        /// <param name="inputText"></param>
        public async Task<SynthesisResult> StartSynthesizingAndSpeakingAsync(string inputText) {
            SynthesisResult result = await currentSynthesizer.StartSynthesizingAndSpeakingAsync(inputText);
            if(result.State == ResultState.Failed && !currentSynthesizer.IsApplicable) {
                if (SwitchToApplicableSynthesizer()) {
                    await currentSynthesizer.StartSynthesizingAndSpeakingAsync(inputText);
                }
            }
            else {
                //If the speech will not be played directly and the current synthesizer supports byte stream.
                //If result.AudioData is null, then current synthesizer doesn't support AsByteStream and the audio should be played directly, so we don't create the audio clip.
                if (currentSynthesizer.OutputForm == AudioDataOutputForm.AsByteStream && result.AudioData != null && result.AudioData.Length > 0) {
                    float[] clipData = BytesToFloat(result.AudioData);
                    audioSource.clip = AudioClip.Create("SynthesizedSpeech", 16000 * 10, 1, 16000, false);
                    audioSource.clip.SetData(clipData, 0);
                    audioSource.Play();
                }
            }
            return result;
        }

        /// <summary>
        /// Update all recognizers and try to find one applicable, return if any.
        /// Can be called when an error occurs.
        /// </summary>
        /// <returns>If there is an applicable recognizer</returns>
        public bool SwitchToApplicableRecognizer() {
            Debug.Log("Current recognizer is not applicable, trying to switch to another applicable recognizer.");
            foreach (ISpeechRecognizer recognizer in recognizers) {
                if (recognizer != currentRecognizer && recognizer.IsApplicable) {
                    currentRecognizer = recognizer;
                    Debug.Log("Applicable alternative recognizer found. Please start the recording again.");
                    return true;
                }
            }
            Debug.LogWarning("No applicable recognizer found.");
            return false;
        }

        public bool SwitchToApplicableSynthesizer() {
            Debug.Log("Current synthesizer is not applicable, trying to switch to another appicable synthesizer.");
            foreach (ISpeechSynthesizer synthesizer in synthesizers) {
                if (synthesizer != currentSynthesizer && synthesizer.IsApplicable) {
                    currentSynthesizer = synthesizer;
                    Debug.Log("Applicable alternative synthesizer found. Please try again.");                   
                    return true;
                }
            }
            Debug.LogWarning("No applicable synthesizer found.");
            return false;
        }

        #region Private Methods

        //Initialize all recognizers and synthesizers and choose the applicables with highest priority.
        private void InitializeRecognizerAndSynthesizer() {
            recognizers = GetComponents<ISpeechRecognizer>();
            foreach (ISpeechRecognizer speechRecognizer in recognizers) {
                speechRecognizer.Language = Language;
                if (speechRecognizer.IsApplicable) {
                    currentRecognizer = speechRecognizer;
                    break;
                }
            }
            synthesizers = GetComponents<ISpeechSynthesizer>();
            foreach (ISpeechSynthesizer speechSynthesizer in synthesizers) {
                speechSynthesizer.Language = Language;
                speechSynthesizer.OutputForm = primaryAudioOutputForm;
                if (speechSynthesizer.IsApplicable) {
                    currentSynthesizer = speechSynthesizer;
                    break;
                }
            }
        }

        // convert two bytes to one float in the range -1 to 1
        private float BytesToFloat(byte firstByte, byte secondByte) {
            // convert two bytes to one short (little endian)
            short s = (short)((secondByte << 8) | firstByte);
            // convert to range from -1 to (just below) 1
            return s / 32768.0F;
        }

        private float[] BytesToFloat(byte[] byteStream) {
            float[] soundData = new float[byteStream.Length / 2];
            for (int i = 0; i < soundData.Length; i++) {
                soundData[i] = BytesToFloat(byteStream[i * 2], byteStream[i * 2 + 1]);
            }
            return soundData;
        }

        #endregion
    }
    
}
