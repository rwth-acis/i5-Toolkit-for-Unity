using Microsoft.CognitiveServices.Speech;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using UnityEngine;

namespace i5.Toolkit.Core.SpeechModule
{
    public interface ISpeechSynthesizer
    {
        /// <summary>
        /// Preffered Language, one recognizer may not supports some of the selectable languages.
        /// </summary>
        Language Language { get; set; }

        /// <summary>
        /// If it is set to ToSpeaker, the synthesizer should speak the audio out directly when it gets the result.
        /// If it is set to AsByteStream, the synthesizer should pass the raw audio data as Byte[] to the SynthesisResult for downstream modules, e.g. converting to an audio clip.
        /// Except for compatibility consideration, as a byte stream allows the audio to be played by an Audio Source and especially as a 3D audio clip.
        /// </summary>
        AudioDataOutputForm OutputForm { get; set; }

        /// <summary>
        /// Check if the synthesizer is currently applicable. For example, cloud service are not applicable without internet connection. 
        /// It is encouraged to have a native solution which works without internet connection.
        /// </summary>
        bool IsApplicable { get; }
        
        /// <summary>
        /// Synthesizing the given text and speaking the audio out.
        /// </summary>
        /// <param name="text"> The text to synthesize</param>
        Task<SynthesisResult> StartSynthesizingAndSpeakingAsync(string text);

        /// <summary>
        /// Fires when the synthesis is complete.
        /// </summary>
        event Action<SynthesisResult> OnSynthesisResultReceived;
    }
}
