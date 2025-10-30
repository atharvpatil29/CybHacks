using UnityEngine;
using TMPro;

public class ComputerInteract : MonoBehaviour
{
    [SerializeField] private GameObject infoPanel; // Your UI panel
    [SerializeField] private TMP_Text infoText; // Your text component

    private string phishingInfo = @"PHISHING AWARENESS TIP:

Be careful of fake emails that try to steal your information!

Example of a suspicious email:
'Your account will be suspended! Click here to verify...'

Red Flags to Watch For:
- Urgent or threatening language
- Strange email addresses
- Bad spelling and grammar
- Requests for personal information

Remember: Real companies never ask for your password by email!

[Press ESC to close]";

    void Start()
    {
        // Hide panel when game starts
        infoPanel.SetActive(false);
    }

    void OnMouseDown()
    {
        // Show panel and set text when computer is clicked
        infoPanel.SetActive(true);  // Removed the accidental 'g' at line start
        infoText.text = phishingInfo;
    }

    void Update()
    {
        // Hide panel when ESC is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            infoPanel.SetActive(false);
        }
    }
}