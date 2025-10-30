using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Security.Cryptography;
using System.Text;

public class PasswordChecker : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField passwordInput;
    public Button checkButton;
    public TMP_Text resultText;

    private string pwnedApiUrl = "https://api.pwnedpasswords.com/range/";
    private bool isChecking;

    void Start()
    {
        checkButton.onClick.AddListener(() => StartCoroutine(CheckPasswordBreach()));
        StartCoroutine(SelectInputField());
    }

    void Update()
    {
        // Handle Enter key submission
        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && !isChecking)
        {
            StartCoroutine(CheckPasswordBreach());
        }

        // Maintain input field focus
        if (passwordInput.gameObject.activeInHierarchy && !passwordInput.isFocused)
        {
            StartCoroutine(SelectInputField());
        }
    }

    IEnumerator SelectInputField()
    {
        yield return null; // Wait one frame for UI initialization
        passwordInput.Select();
        passwordInput.ActivateInputField();
    }

    IEnumerator CheckPasswordBreach()
    {
        isChecking = true;
        StartCoroutine(SelectInputField());
        
        string password = passwordInput.text.Trim();
        if (string.IsNullOrEmpty(password))
        {
            resultText.text = "<color=red>Enter a password!</color>";
            isChecking = false;
            yield break;
        }

        resultText.text = "Checking...";
        
        // Hash the password
        string hash = ComputeSHA1Hash(password);
        string prefix = hash.Substring(0, 5);
        string suffix = hash.Substring(5).ToUpper();

        string url = $"{pwnedApiUrl}{prefix}";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                int breachCount = CheckForSuffix(request.downloadHandler.text, suffix);
                resultText.text = breachCount > 0 
                    ? $"<color=red>Compromised in {breachCount} breaches!</color>" 
                    : "<color=green>Secure password!</color>";
            }
            else
            {
                resultText.text = $"<color=red>Error: {request.error}</color>";
            }
        }

        isChecking = false;
        StartCoroutine(SelectInputField()); // Re-focus after check
    }

    string ComputeSHA1Hash(string input)
    {
        using (SHA1 sha1 = SHA1.Create())
        {
            byte[] bytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
            var builder = new StringBuilder();
            foreach (byte b in bytes) builder.Append(b.ToString("X2"));
            return builder.ToString();
        }
    }

    int CheckForSuffix(string response, string suffix)
    {
        foreach (string line in response.Split('\n'))
        {
            string[] parts = line.Trim().Split(':');
            if (parts.Length == 2 && parts[0].Equals(suffix, System.StringComparison.OrdinalIgnoreCase))
            {
                return int.Parse(parts[1]);
            }
        }
        return 0;
    }
}