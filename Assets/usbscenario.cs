// USBScenario.cs
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class USBScenario : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text questionText;
    public TMP_Text correctText;
    public TMP_Text wrongText;
    public Button reportButton;
    public Button plugButton;

    void Start()
    {
        reportButton.onClick.AddListener(ShowCorrect);
        plugButton.onClick.AddListener(ShowWrong);
        InitializeUI();
    }

    void InitializeUI()
    {
        correctText.gameObject.SetActive(false);
        wrongText.gameObject.SetActive(false);
        reportButton.gameObject.SetActive(true);
        plugButton.gameObject.SetActive(true);
    }

    void ShowCorrect()
    {
        correctText.gameObject.SetActive(true);
        wrongText.gameObject.SetActive(false);
        reportButton.gameObject.SetActive(false);
        plugButton.gameObject.SetActive(false);
    }

    void ShowWrong()
    {
        wrongText.gameObject.SetActive(true);
        correctText.gameObject.SetActive(false);
        reportButton.gameObject.SetActive(false);
        plugButton.gameObject.SetActive(false);
    }
}