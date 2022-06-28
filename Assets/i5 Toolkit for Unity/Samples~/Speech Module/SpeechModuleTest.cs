using i5.Toolkit.Core.SpeechModule;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeechModuleTest : MonoBehaviour
{
    [SerializeField] private GameObject speechModule;
    [SerializeField] private TMP_InputField textInputField;
    [SerializeField] private TextMeshProUGUI recognitionResultText;
    [SerializeField] private GameObject startRecordingButton;
    [SerializeField] private GameObject stopRecordingButton;

    private SpeechProvider provider;
    private AzureSpeechSynthesizer azureSynthesizer;
    private NativeSpeechSynthesizer nativeSynthesizer;
    private AzureSpeechRecognizer azureRecognizer;
    private NativeSpeechRecognizer nativeRecognizer;
    // Start is called before the first frame update
    void Start()
    {
        provider = speechModule.GetComponent<SpeechProvider>();
        provider.OnRecognitionResultReceived += WriteRecognitionResult;

        azureSynthesizer = speechModule.GetComponent<AzureSpeechSynthesizer>();
        nativeSynthesizer = speechModule.GetComponent<NativeSpeechSynthesizer>();
        azureRecognizer = speechModule.GetComponent<AzureSpeechRecognizer>();
        nativeRecognizer = speechModule.GetComponent<NativeSpeechRecognizer>();
    }

    public void ChangeSynthesizer(int number) {
        //Azure
        if (number == 1) {
            Debug.LogWarning("You switched to the azure synthesizer, make sure you added your keys on its inspector view on the \"Speech Module\" object");
            azureSynthesizer.enabled = true;
            nativeSynthesizer.enabled = false;
            provider.SwitchToApplicableSynthesizer();
        }
        //Native
        else {
            azureSynthesizer.enabled = false;
            nativeSynthesizer.enabled = true;
            provider.SwitchToApplicableSynthesizer();
        }
    }

    public void ChangeRecognizer(int number) {
        //Azure
        if (number == 1) {
            Debug.LogWarning("You switched to the azure recognizer, make sure you added your keys on its inspector view on the \"Speech Module\" object.");
            azureRecognizer.enabled = true;
            nativeRecognizer.enabled = false;
            provider.SwitchToApplicableRecognizer();
        }
        //Native
        else {
            azureRecognizer.enabled = false;
            nativeRecognizer.enabled = true;
            provider.SwitchToApplicableSynthesizer();
        }
    }

    public async void Speak() {
        await provider.StartSynthesizingAndSpeakingAsync(textInputField.text);
    }

    public async void StartRecording() {
        recognitionResultText.text = "Recording...";
        startRecordingButton.SetActive(false);
        stopRecordingButton.SetActive(true);
        await provider.StartRecordingAsync();
    }

    public async void StopRecording() {
        startRecordingButton.SetActive(true);
        stopRecordingButton.SetActive(false);
        await provider.StopRecordingAsync();
    }

    public void WriteRecognitionResult(RecognitionResult result) {
        if(result.State == ResultState.Succeeded) {
            recognitionResultText.text = result.Text;
        }
        else {
            recognitionResultText.text = result.Message;
        }

    }
}
