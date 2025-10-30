using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PhishingQuiz : MonoBehaviour
{
    [System.Serializable]
    public class EmailQuestion
    {
        public Sprite emailImage;
        public bool isPhishing;
        public string explanation;
    }

    [Header("Quiz Content")]
    public EmailQuestion[] emailQuestions;
    
    [Header("UI References")]
    public Image emailDisplay;
    public Button realButton;
    public Button phishingButton;
    public TMP_Text feedbackText;
    public Button nextButton;
    
    private int currentEmailIndex = 0;
    private int correctAnswers = 0;
    private bool hasAnswered = false;
    
    void OnEnable()
    {
        // Called when UI is activated - CRITICAL for reuse
        ResetQuiz();
    }
    
    void Start()
    {
        // Set up button listeners
        realButton.onClick.AddListener(() => SubmitAnswer(false));
        phishingButton.onClick.AddListener(() => SubmitAnswer(true));
        nextButton.onClick.AddListener(LoadNextEmail);
    }
    
    void ResetQuiz()
    {
        // Reset quiz state when UI is shown
        currentEmailIndex = 0;
        correctAnswers = 0;
        hasAnswered = false;
        
        // Initialize the UI
        feedbackText.text = "";
        nextButton.gameObject.SetActive(false);
        LoadCurrentEmail();
    }
    
    void LoadCurrentEmail()
    {
        if (currentEmailIndex < emailQuestions.Length)
        {
            // Display the current email
            emailDisplay.sprite = emailQuestions[currentEmailIndex].emailImage;
            
            // Reset UI state
            hasAnswered = false;
            feedbackText.text = "";
            nextButton.gameObject.SetActive(false);
            
            // Enable answer buttons
            realButton.interactable = true;
            phishingButton.interactable = true;
        }
        else
        {
            // Quiz completed
            emailDisplay.sprite = null;
            feedbackText.text = $"Quiz completed! Final score: {correctAnswers}/{emailQuestions.Length}";
            realButton.interactable = false;
            phishingButton.interactable = false;
            nextButton.gameObject.SetActive(false);
        }
    }
    
    void SubmitAnswer(bool selectedPhishing)
    {
        if (hasAnswered) return;
        
        EmailQuestion currentQuestion = emailQuestions[currentEmailIndex];
        bool isCorrect = (selectedPhishing == currentQuestion.isPhishing);
        
        // Update score
        if (isCorrect)
            correctAnswers++;
        
        // Show feedback
        string correctnessText = isCorrect ? "Correct!" : "Incorrect!";
        feedbackText.text = $"{correctnessText}\n{currentQuestion.explanation}";
        feedbackText.color = isCorrect ? Color.green : Color.red;
        
        // Update UI state
        hasAnswered = true;
        realButton.interactable = false;
        phishingButton.interactable = false;
        nextButton.gameObject.SetActive(true);
    }
    
    void LoadNextEmail()
    {
        currentEmailIndex++;
        LoadCurrentEmail();
    }
    
    public void ExitQuiz()
    {
        // Just deactivate the UI
        gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}