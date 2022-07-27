using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;
using System.IO;
using i5.Toolkit.Core.Utilities.Async;
using System.Threading.Tasks;

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

        public static RecognitionResult RequiredModulesNotFoundResult
        {
            get
            {
                RecognitionResult result = new RecognitionResult();
                result.State = ResultState.Failed;
                result.Text = "Required modules for the current Recognizer cannot be found, or corresponding directive is not set on the current platform.";
                result.Message = "Required modules for the current Recognizer cannot be found, or corresponding directive is not set on the current platform.";
                return result;
            }
        }
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

        public static SynthesisResult RequiredModulesNotFoundResult
        {
            get
            {
                SynthesisResult result = new SynthesisResult();
                result.State = ResultState.Failed;
                result.Message = "Required modules for the current Recognizer cannot be found, or corresponding directive is not set on the current platform.";
                return result;
            }
        }
    }

#if UNITY_EDITOR
    public static class LibraryImporter {

        private const string AzureRecognizerDefine = "I5_TOOLKIT_USE_AZURE_SPEECH_RECOGNIZER";
        private const string AzureSynthesizerDefine = "I5_TOOLKIT_USE_AZURE_SPEECH_SYNTHESIZER";
        private const string NativeRecognizerDefine = "I5_TOOLKIT_USE_NATIVE_SPEECH_RECOGNIZER";
        private const string NativeSynthesizerDefine = "I5_TOOLKIT_USE_NATIVE_SPEECH_SYNTHESIZER";
        private const string ProgressBarTitle = "i5 Toolkit Speech Module Importer";
        private const string AzureSpeechSDKURL = "https://aka.ms/csspeech/unitypackage";
        private const string NativeRecognizerAssemblyName = "i5.Toolkit.Core.Runtime.SpeechModulePlugin.NativeRecognizer";
        private const string NativeRecognizerURL = "https://github.com/rwth-acis/i5-Toolkit-for-Unity-SpeechModulePlugin/releases/download/v1.0.0/NativeSpeechRecognizerPlugin.unitypackage";
        private const string VoskEnglishModelURL = "https://alphacephei.com/vosk/models/vosk-model-small-en-us-0.15.zip";
        private const string VoskEnglishModelName = "vosk-model-small-en-us-0.15.zip";
        private const string VoskEnglishModelNameWithoutExtension = "vosk-model-small-en-us-0.15";
        private const string VoskGermanModelURL = "https://alphacephei.com/vosk/models/vosk-model-small-de-0.15.zip";
        private const string VoskGermanModelName = "vosk-model-small-de-0.15.zip";
        private const string VoskGermanModelNameWithoutExtension = "vosk-model-small-de-0.15";
        private const string NativeSynthesizerAssemblyName = "i5.Toolkit.Core.Runtime.SpeechModulePlugin.NativeSynthesizer";
        private const string NativeSynthesizerURL = "https://github.com/rwth-acis/i5-Toolkit-for-Unity-SpeechModulePlugin/releases/download/v1.0.0/NativeSpeechSynthesizerPlugin.unitypackage";

        public static List<BuildTargetGroup> TargetPlatform = new List<BuildTargetGroup>{BuildTargetGroup.Standalone, BuildTargetGroup.WSA, BuildTargetGroup.Android};

        //We only need to check on one platform
        private static bool AzureSpeechRecognizerImported
        {
            get
            {
                string define = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
                if (define.Contains(AzureRecognizerDefine)) {
                    Debug.Log($"{AzureRecognizerDefine} already defined.");
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        private static bool AzureSpeechSynthesizerImported 
        {
            get
            {
                string define = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
                if (define.Contains(AzureSynthesizerDefine)) {
                    Debug.Log($"{AzureSynthesizerDefine} already defined.");
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        private static bool NativeSpeechRecognizerImported
        {
            get
            {
                string define = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
                if (define.Contains(NativeRecognizerDefine)) {
                    Debug.Log($"{NativeRecognizerDefine} already defined.");
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        private static bool NativeSpeechSynthesizerImported
        {
            get
            {
                string define = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
                if (define.Contains(NativeSynthesizerDefine)) {
                    Debug.Log($"{NativeSynthesizerDefine} already defined.");
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        [MenuItem("i5 Toolkit/Import Speech Module/Azure Recognizer")]
        public static async void ImportAzureSpeechRecognizerAsync() {
            if (!AzureSpeechRecognizerImported) {
                string[] assetGUIDs = AssetDatabase.FindAssets("SpeechSDK");
                if (assetGUIDs.Length > 0) {
                    Debug.Log($"Found Speech SDK in {AssetDatabase.GUIDToAssetPath(assetGUIDs[0])}, skip download.");
                }
                else { 
                    //Download the speech SDK and show the progress
                    Debug.Log("Starting to download SpeechSDK...");
                    if (AssetDatabase.FindAssets($"StreamingAssets").Length == 0) {
                        AssetDatabase.CreateFolder("Assets", "StreamingAssets");
                    }
                    string path = Application.streamingAssetsPath + "/SpeechSDK.unitypackage";
                    string progressBarMessage = "Downloading SpeechSDK...";
                    await DownloadResourceAsync(AzureSpeechSDKURL, path, progressBarMessage);
                    //Import the package
                    AssetDatabase.ImportPackage(path, false);
                    File.Delete(path);
                }      
                Dictionary<BuildTargetGroup, string> scriptingSymbols = GetScriptingSymbolsForTargetPlatforms();         
                foreach (BuildTargetGroup targetGroup in TargetPlatform) {
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, $"{scriptingSymbols[targetGroup]};{AzureRecognizerDefine}");
                }
                Debug.Log("Azure Speech Recognizer imported.");
                EditorUtility.ClearProgressBar();
            }
            else {
                Debug.Log("Azure Speech Recognizer already imported.");
            }
        }

        [MenuItem("i5 Toolkit/Import Speech Module/Azure Synthesizer")]
        public static async void ImportAzureSpeechSynthesizerAsync() {
            if (!AzureSpeechSynthesizerImported) {
                string[] assetGUIDs = AssetDatabase.FindAssets("SpeechSDK");
                if (assetGUIDs.Length > 0) {
                    Debug.Log($"Found Speech SDK in {AssetDatabase.GUIDToAssetPath(assetGUIDs[0])}, skip download.");
                } 
                else {
                    //Download the speech SDK and show the progress
                    Debug.Log("Starting to download SpeechSDK...");
                    if (AssetDatabase.FindAssets($"StreamingAssets").Length == 0) {
                        AssetDatabase.CreateFolder("Assets", "StreamingAssets");
                    }
                    string path = Application.streamingAssetsPath + "/SpeechSDK.unitypackage";
                    string progressBarMessage = "Downloading SpeechSDK...";
                    await DownloadResourceAsync(AzureSpeechSDKURL, path, progressBarMessage);
                    //Import the package
                    AssetDatabase.ImportPackage(path, false);
                    File.Delete(path);
                }              
                Dictionary<BuildTargetGroup, string> scriptingSymbols = GetScriptingSymbolsForTargetPlatforms();
                foreach (BuildTargetGroup targetGroup in TargetPlatform) {
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, $"{scriptingSymbols[targetGroup]};{AzureSynthesizerDefine}");
                }
                Debug.Log("Azure Speech Synthesizer imported.");
            }
            else {
                Debug.Log("Azure Speech Synthesizer already imported.");
            }
        }

        [MenuItem("i5 Toolkit/Import Speech Module/Native Recognizer")]
        public static async void ImportNativeSpeechRecognizerAsync() {
            if (!NativeSpeechRecognizerImported) {
                // Download the (latest) English model (small).
                if (AssetDatabase.FindAssets(VoskEnglishModelNameWithoutExtension, new string[1] { "Assets/StreamingAssets/" }).Length > 0) {
                    Debug.Log("Found Vosk English model (small) in the StreamingAssets folder, skip download.");
                }
                else {
                    Debug.Log("Starting to download Vosk English model (small)...");
                    string path = $"{Application.streamingAssetsPath}/{VoskEnglishModelName}";
                    string progressBarMessage = "Downloading Vosk English model (small)...";
                    // Check if the StreamingAssets folder exists
                    if (AssetDatabase.FindAssets($"StreamingAssets").Length == 0) {
                        AssetDatabase.CreateFolder("Assets", "StreamingAssets");
                    }
                    await DownloadResourceAsync(VoskEnglishModelURL, path, progressBarMessage);
                    Debug.Log("Vosk English model (small) downloaded in the StreamingAssets folder.");
                }

                // Download the (latest) German model (small).
                if (AssetDatabase.FindAssets(VoskGermanModelNameWithoutExtension, new string[1] { "Assets/StreamingAssets/" }).Length > 0) {
                    Debug.Log("Found Vosk German model (small) in StreamingAssets folder, skip download.");
                }
                else {
                    Debug.Log("Starting to download Vosk German model (small)...");
                    string path = $"{Application.streamingAssetsPath}/{VoskGermanModelName}";
                    string progressBarMessage = "Downloading Vosk German model (small)...";
                    // Check if the StreamingAssets folder exists
                    if (AssetDatabase.FindAssets($"StreamingAssets").Length == 0) {
                        AssetDatabase.CreateFolder("Assets", "StreamingAssets");
                    }
                    await DownloadResourceAsync(VoskGermanModelURL, path, progressBarMessage);
                    Debug.Log("Vosk German model (small) downloaded in the StreamingAssets folder.");
                }

                // Download the scripts as package and import.
                string[] folderGUIDs = AssetDatabase.FindAssets(NativeRecognizerAssemblyName);
                if (folderGUIDs.Length > 0) {
                    Debug.Log($"Found the native recognizer assembly definition in {AssetDatabase.GUIDToAssetPath(folderGUIDs[0])}, skip download.");
                }
                else {
                    string path = Application.streamingAssetsPath + "/NativeRecognizer.unitypackage";
                    string progressBarMessage = "Downloading native recognizer scripts...";
                    if (AssetDatabase.FindAssets($"StreamingAssets").Length == 0) {
                        AssetDatabase.CreateFolder("Assets", "StreamingAssets");
                    }
                    await DownloadResourceAsync(NativeRecognizerURL, path, progressBarMessage);
                    //Import the package
                    AssetDatabase.ImportPackage(path, false);
                    File.Delete(path);
                }
                Dictionary<BuildTargetGroup, string> scriptingSymbols = GetScriptingSymbolsForTargetPlatforms();
                foreach (BuildTargetGroup targetGroup in TargetPlatform) {
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, $"{scriptingSymbols[targetGroup]};{NativeRecognizerDefine}");
                }
                Debug.Log("Native Speech Recognizer imported in \"i5 Toolkit for Unity Speech Module Plugin\".");
            }
            else {
                Debug.Log("Native Speech Recognizer already imported.");
            }
        }

        [MenuItem("i5 Toolkit/Import Speech Module/Native Synthesizer")]
        public static async void ImportNativeSpeechSynthesizerAsync() {
            if (!NativeSpeechSynthesizerImported) {
                string[] folderGUIDs = AssetDatabase.FindAssets(NativeSynthesizerAssemblyName);
                if (folderGUIDs.Length > 0) {
                    Debug.Log($"Found the native synthesizer assembly definition in {AssetDatabase.GUIDToAssetPath(folderGUIDs[0])}, skip download.");
                }
                else {
                    string path = Application.streamingAssetsPath + "/NativeSynthesizer.unitypackage";
                    string progressBarMessage = "Downloading native synthesizer scripts...";
                    if (AssetDatabase.FindAssets($"StreamingAssets").Length == 0) {
                        AssetDatabase.CreateFolder("Assets", "StreamingAssets");
                    }
                    await DownloadResourceAsync(NativeSynthesizerURL, path, progressBarMessage);
                    //Import the package
                    AssetDatabase.ImportPackage(path, false);
                    File.Delete(path);
                }
                Dictionary<BuildTargetGroup, string> scriptingSymbols = GetScriptingSymbolsForTargetPlatforms();
                foreach (BuildTargetGroup targetGroup in TargetPlatform) {
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, $"{scriptingSymbols[targetGroup]};{NativeSynthesizerDefine}");
                }
                Debug.Log("Native Speech Synthesizer imported in \"i5 Toolkit for Unity Speech Module Plugin\".");
            }
            else {
                Debug.Log("Native Speech Synthesizer already imported.");
            }
        }

        private static Dictionary<BuildTargetGroup, string> GetScriptingSymbolsForTargetPlatforms() {
            Dictionary<BuildTargetGroup, string> scriptingSymbols = new Dictionary<BuildTargetGroup, string>();
            foreach(BuildTargetGroup targetGroup in TargetPlatform) {
                scriptingSymbols.Add(targetGroup, PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup));
            }
            return scriptingSymbols;
        }

        private static void ShowDownloadProgress(UnityWebRequest downloadRequest, string progressBarInfo) {
            if (downloadRequest != null) {
                EditorUtility.DisplayProgressBar(ProgressBarTitle, progressBarInfo, downloadRequest.downloadProgress);
            }
        }

        private static void ShowImportingPackageProgress() {
            EditorUtility.DisplayProgressBar(ProgressBarTitle, "Importing Package...", 1);
        }

        // Download the resources from url to path.
        private static async Task DownloadResourceAsync(string url, string path, string progressBarMessage) {
            UnityWebRequest downloadRequest = UnityWebRequest.Get(url);
            EditorApplication.update += () => ShowDownloadProgress(downloadRequest, progressBarMessage);
            await downloadRequest.SendWebRequest();
            EditorApplication.update -= () => ShowDownloadProgress(downloadRequest, progressBarMessage);
            byte[] data = downloadRequest.downloadHandler.data;
            using (FileStream fs = new FileStream(path, FileMode.Create)) {
                fs.Write(data, 0, data.Length);
                fs.Dispose();
            }
            EditorUtility.ClearProgressBar();
            downloadRequest.Dispose();
            downloadRequest = null;
        }
    }
#endif
}
