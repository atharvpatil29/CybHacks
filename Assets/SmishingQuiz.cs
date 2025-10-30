using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SmishingQuiz : MonoBehaviour
{
    [System.Serializable]
    public class TextQuestion
    {
        public Sprite textMessageImage;
        public bool isSmishing;
        public string explanation;
    }

    [Header("Quiz Content")]
    public TextQuestion[] textQuestions;
    
    [Header("UI References")]
    public Image messageDisplay;
    public Button genuineButton;
    public Button smishingButton;
    public TMP_Text feedbackText;
    public Button nextButton;
    
    private int currentMessageIndex = 0;
    private int correctAnswers = 0;
    private bool hasAnswered = false;
    
    void OnEnable()
    {
        ResetQuiz();
    }
    
    void Start()
    {
        genuineButton.onClick.AddListener(() => SubmitAnswer(false));
        smishingButton.onClick.AddListener(() => SubmitAnswer(true));
        nextButton.onClick.AddListener(LoadNextMessage);
    }
    
    void ResetQuiz()
    {
        currentMessageIndex = 0;
        correctAnswers = 0;
        hasAnswered = false;
        
        feedbackText.text = "";
        nextButton.gameObject.SetActive(false);
        LoadCurrentMessage();
    }
    
    void LoadCurrentMessage()
    {
        if (currentMessageIndex < textQuestions.Length)
        {
            messageDisplay.sprite = textQuestions[currentMessageIndex].textMessageImage;
            hasAnswered = false;
            feedbackText.text = "";
            nextButton.gameObject.SetActive(false);
            
            genuineButton.interactable = true;
            smishingButton.interactable = true;
        }
        else
        {
            messageDisplay.sprite = null;
            feedbackText.text = $"Quiz Complete! Score: {correctAnswers}/{textQuestions.Length}";
            genuineButton.interactable = false;
            smishingButton.interactable = false;
            nextButton.gameObject.SetActive(false);
        }
    }
    
    void SubmitAnswer(bool selectedSmishing)
    {
        if (hasAnswered) return;
        
        TextQuestion currentQuestion = textQuestions[currentMessageIndex];
        bool isCorrect = (selectedSmishing == currentQuestion.isSmishing);
        
        if (isCorrect)
            correctAnswers++;
        
        string correctnessText = isCorrect ? "Correct!" : "Incorrect!";
        feedbackText.text = $"{correctnessText}\n{currentQuestion.explanation}";
        feedbackText.color = isCorrect ? Color.green : Color.red;
        
        hasAnswered = true;
        genuineButton.interactable = false;
        smishingButton.interactable = false;
        nextButton.gameObject.SetActive(true);
    }
    
    void LoadNextMessage()
    {
        currentMessageIndex++;
        LoadCurrentMessage();
    }
    
    public void ExitQuiz()
    {
        gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}