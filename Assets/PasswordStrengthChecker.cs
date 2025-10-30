using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class PasswordStrengthCheckerzzz : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField passwordInput;
    public Button checkButton;
    public TMP_Text strengthText;
    public TMP_Text timeText;

    private bool isChecking;

    void Start()
    {
        if (checkButton != null)
            checkButton.onClick.AddListener(CheckStrength);
        
        StartCoroutine(SelectInputField());
    }

    void Update()
    {
        if (passwordInput == null) return;

        // Handle Enter key
        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && !isChecking)
        {
            CheckStrength();
        }

        // Maintain focus
        if (passwordInput.gameObject.activeInHierarchy && !passwordInput.isFocused)
        {
            StartCoroutine(SelectInputField());
        }
    }

    IEnumerator SelectInputField()
    {
        yield return null; // Wait one frame
        if (passwordInput != null)
        {
            passwordInput.Select();
            passwordInput.ActivateInputField();
        }
    }

    public void CheckStrength()
    {
        if (passwordInput == null || strengthText == null || timeText == null) return;

        isChecking = true;
        string password = passwordInput.text.Trim();

        if (string.IsNullOrEmpty(password))
        {
            strengthText.text = "<color=red>Enter a password!</color>";
            timeText.text = "";
            isChecking = false;
            return;
        }

        // Calculate metrics
        int charVariety = CalculateCharacterVariety(password);
        double combinations = Mathf.Pow(charVariety, password.Length);
        double seconds = combinations / 1e12f; // 1 trillion guesses/sec

        strengthText.text = GetStrengthRating(password.Length, charVariety);
        timeText.text = FormatTime(seconds);
        isChecking = false;
    }

    int CalculateCharacterVariety(string password)
    {
        int variety = 0;
        if (password.Any(char.IsLower)) variety += 26;
        if (password.Any(char.IsUpper)) variety += 26;
        if (password.Any(char.IsDigit)) variety += 10;
        if (password.Any(c => !char.IsLetterOrDigit(c))) variety += 32;
        return variety;
    }

    string GetStrengthRating(int length, int variety)
    {
        if (length < 8) return "<color=red>Very Weak</color>";
        if (variety < 30) return "<color=orange>Weak</color>";
        if (variety < 60) return "<color=yellow>Moderate</color>";
        return "<color=green>Strong!</color>";
    }

    string FormatTime(double seconds)
    {
        string[] units = { "seconds", "minutes", "hours", "days", "years", "centuries" };
        double[] divisors = { 1, 60, 3600, 86400, 31536000, 3153600000 };

        for(int i = divisors.Length - 1; i >= 0; i--)
        {
            if (seconds >= divisors[i])
            {
                double time = seconds / divisors[i];
                return $"{time:0.##} {units[i]}";
            }
        }
        return "Instantly";
    }
}