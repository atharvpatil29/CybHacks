using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class PasswordStrengthChecker : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField passwordInput;
    public Button checkButton;
    public TMP_Text strengthText;
    public TMP_Text timeText;

    private bool isChecking;
    private const float GUESSES_PER_SECOND = 1000000000000f;

    void Start()
    {
        checkButton.onClick.AddListener(CheckStrength);
        StartCoroutine(SelectInputField());
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && !isChecking)
        {
            CheckStrength();
        }

        if (!passwordInput.isFocused)
        {
            StartCoroutine(SelectInputField());
        }
    }

    IEnumerator SelectInputField()
    {
        yield return null;
        passwordInput.Select();
        passwordInput.ActivateInputField();
    }

    void CheckStrength()
    {
        isChecking = true;
        string password = passwordInput.text.Trim();

        if (string.IsNullOrEmpty(password))
        {
            strengthText.text = "<color=red>Enter a password!</color>";
            timeText.text = "";
            isChecking = false;
            return;
        }

        int charVariety = CalculateCharacterVariety(password);
        double combinations = Mathf.Pow(charVariety, password.Length);
        double seconds = combinations / GUESSES_PER_SECOND;

        strengthText.text = GetStrengthRating(password.Length, charVariety);
        timeText.text = FormatTime(seconds);
        isChecking = false;
        StartCoroutine(SelectInputField());
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
        if (length < 8) return "<color=red>Very Weak (Too Short)</color>";
        if (variety < 30) return "<color=orange>Weak (Limited Chars)</color>";
        if (variety < 60) return "<color=yellow>Moderate</color>";
        return "<color=green>Strong!</color>";
    }

    string FormatTime(double seconds)
    {
        string[] units = { "seconds", "minutes", "hours", "days", "years", "centuries" };
        double[] divisors = { 1, 60, 3600, 86400, 31536000, 3153600000 };

        for (int i = divisors.Length - 1; i >= 0; i--)
        {
            if (seconds >= divisors[i])
            {
                double time = seconds / divisors[i];
                return $"Estimated crack time: {time:0.##} {units[i]}";
            }
        }
        return "Instantly crackable";
    }
}