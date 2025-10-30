using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class PhoneScamManager : MonoBehaviour
{
    [System.Serializable]
    public class PhoneScamScenario
    {
        public string scenarioText;
        public AudioClip scenarioAudio;
        public bool isLegitimate;
    }

    [Header("Scenarios")]
    public List<PhoneScamScenario> scenarios = new List<PhoneScamScenario>()
    {
        new PhoneScamScenario 
        {
            scenarioText = "Hi, this is Marie from Revenue. We need your PPSN immediately to process your tax refund.",
            isLegitimate = false
        },
        new PhoneScamScenario 
        {
            scenarioText = "Good morning! John from Eir here. We need remote access to fix your broadband.",
            isLegitimate = false
        },
        new PhoneScamScenario 
        {
            scenarioText = "This is HSE Contact Tracing. Your COVID test results are ready on the portal.",
            isLegitimate = true
        }
    };

    [Header("UI Elements")]
    public TMP_Text scenarioText;
    public TMP_Text resultText;
    public Button trustButton;
    public Button rejectButton;

    [Header("Audio")]
    public AudioClip correctSound;
    public AudioClip wrongSound;
    private AudioSource audioSource;

    private int currentScenario = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        trustButton.onClick.AddListener(() => HandleResponse(true));
        rejectButton.onClick.AddListener(() => HandleResponse(false));
        LoadScenario();
    }

    public void ResetScenarios()
    {
        currentScenario = 0;
        LoadScenario();
    }

    void LoadScenario()
    {
        resultText.text = "";
        scenarioText.text = scenarios[currentScenario].scenarioText;
        
        trustButton.interactable = true;
        rejectButton.interactable = true;

        if(scenarios[currentScenario].scenarioAudio != null)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(scenarios[currentScenario].scenarioAudio);
        }
    }

    public void HandleResponse(bool trustedCall)
    {
        trustButton.interactable = false;
        rejectButton.interactable = false;

        bool correctChoice = (trustedCall == scenarios[currentScenario].isLegitimate);
        
        resultText.text = correctChoice ? 
            "<color=green>Correct! " + GetFeedback() + "</color>" :
            "<color=red>Danger! " + GetFeedback() + "</color>";

        AudioSource.PlayClipAtPoint(correctChoice ? correctSound : wrongSound, Camera.main.transform.position);
        Invoke(nameof(NextScenario), 2f);
    }

    string GetFeedback()
    {
        return scenarios[currentScenario].isLegitimate ? 
            "This was a legitimate call from an official service." :
            "This was a scam! " + (scenarios[currentScenario].scenarioText.Contains("PPSN") ? 
            "Never share your PPSN over the phone!" : 
            "Never grant remote access to unsolicited callers!");
    }

    void NextScenario()
    {
        currentScenario = (currentScenario + 1) % scenarios.Count;
        LoadScenario();
    }
}