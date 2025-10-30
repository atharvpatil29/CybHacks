using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SimpleUIController1 : MonoBehaviour
{
    [Header("UI References")]
    public GameObject mainPanel; // Drag MainPanel here
    public TMP_InputField passwordInput;
    public Button checkButton;
    public TMP_Text resultText;

    void Start()
    {
        // Ensure UI is hidden on start
        if(mainPanel != null)
            mainPanel.SetActive(false);
    }

    public void ShowUI()
    {
        if(mainPanel != null)
        {
            mainPanel.SetActive(true);
            passwordInput.text = "";
            resultText.text = "Enter password:";
            passwordInput.Select();
        }
    }

    public void HideUI()
    {
        if(mainPanel != null)
            mainPanel.SetActive(false);
    }

    public void CheckPassword()
    {
        // Your password check logic here
        string password = passwordInput.text;
        resultText.text = $"Checked: {password}";
    }
}