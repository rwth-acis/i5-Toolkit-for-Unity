using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.SpeechModule
{
    public enum Language 
    {
        en_US,
        de_DE
    }
    public enum ResultState
    {
        Succeeded,
        NoMatch,
        Failed
    }

    public enum AudioDataOutputForm {
        ToSpeaker,
        AsByteStream
    }

    public class RecognitionResult {
        public ResultState State;
        /// <summary>
        /// The recognized text.
        /// </summary>
        public string Text;
        /// <summary>
        /// The success message or error message.
        /// </summary>
        public string Message;
    }

    public class SynthesisResult {
        public ResultState State;
        /// <summary>
        /// The synthesized audio in bytes, only assigned when the output form is "AsByteStream".
        /// </summary>
        public byte[] AudioData;
        /// <summary>
        /// The success message or error message.
        /// </summary>
        public string Message;
    }
}
