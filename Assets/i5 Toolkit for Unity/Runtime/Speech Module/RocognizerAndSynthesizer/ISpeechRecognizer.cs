using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using UnityEngine;

namespace i5.Toolkit.Core.SpeechModule
{
    public interface ISpeechRecognizer
    {

        /// <summary>
        /// Preffered Language, one recognizer may not supports some of the selectable languages.
        /// </summary>
        Language Language { get; set; }

        /// <summary>
        /// Check if the recognizer is currently applicable. For example, cloud service are not applicable without internet connection.
        /// </summary>
        bool IsApplicable { get;}

        /// <summary>
        /// Start recording and return the result.
        /// Please subscribe the OnRecognitionResultReceived event and avoid using the return value of StartRecordingAsync(), 
        /// because the result is empty for successful continuous recognition for some (Azure) recognizers.
        /// </summary>
        /// <returns>The recognition result, avoid using it.</returns>
        Task<RecognitionResult> StartRecordingAsync();

        /// <summary>
        /// Only use to stop recording but return nothing.
        /// For some use cases that the recording stops automatically (e.g. when silence is detected), you can leave it empty.
        /// </summary>
        Task StopRecordingAsync();

        

        /// <summary>
        /// Fires when the recognizer receives the result.
        /// Please subscribe the OnRecognitionResultReceived event and avoid using the return value of StartRecordingAsync(), 
        /// because the result is empty for successful continuous recognition for some (Azure) recognizers.
        /// </summary>
        event Action<RecognitionResult> OnRecognitionResultReceived;
    }
}
