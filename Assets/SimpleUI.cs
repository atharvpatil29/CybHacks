using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SimpleUIController : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField inputField;
    public Button submitButton;
    public TMP_Text resultText;
    public GameObject uiPanel;

    void Start()
    {
        uiPanel.SetActive(false);
        submitButton.onClick.AddListener(HandleSubmit);
    }

    void Update()
    {
        // Submit on Enter/Return
        if (Input.GetKeyDown(KeyCode.Return) && uiPanel.activeSelf)
        {
            HandleSubmit();
        }

        // Force focus to input field
        if (uiPanel.activeSelf && !inputField.isFocused)
        {
            inputField.Select();
            inputField.ActivateInputField();
        }
    }

    public void ShowUI()
    {
        uiPanel.SetActive(true);
        inputField.text = "";
        resultText.text = "";
        inputField.Select(); // Focus immediately
    }

    public void HideUI()
    {
        uiPanel.SetActive(false);
    }

    void HandleSubmit()
    {
        string input = inputField.text.Trim();
        resultText.text = $"You entered: {input}";
        // Add custom logic (e.g., password validation)
    }
}