using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Image selectedImage;
    public Image compImage;
    public List<Sprite> images;

    private Image defaultSelectionImage;


    // TextMeshPro elements
    public TMP_Text playerScoreText;
    public TMP_Text computerScoreText;
    public TMP_Text gameAnnounceText;

    // Scores
    private int playerScore = 0;
    private int computerScore = 0;

    private bool matchOver = false;

    public GameObject gameOverPanel;      // The panel you created
    public TMP_Text gameOverText;         // Text showing who won
    public Button resetButton;            // Reset button
    public Button backHomeButton;         // Back Home button

    void Start()
    {
        // Hide panel at start
        gameOverPanel.SetActive(false);

        // Hook up buttons
        resetButton.onClick.AddListener(ResetMatch);
        backHomeButton.onClick.AddListener(BackToHome);
    }


    void Awake()
    {
        defaultSelectionImage = selectedImage;
    }
    public void OnButtonClick(GameObject buttonObject)
    {
        if (matchOver)
            return; // Stop input if match is over

        Debug.Log("Click: " + buttonObject.name);

        if (selectedImage == null || compImage == null || images == null || images.Count == 0)
        {
            Debug.LogError("Images not properly assigned in Inspector!");
            return;
        }

        Image img = buttonObject.GetComponent<Image>();
        if (img == null)
        {
            Debug.LogError("Clicked object has no Image component: " + buttonObject.name);
            return;
        }

        selectedImage.sprite = img.sprite;

        compImage.sprite = images[Random.Range(0, images.Count)];

        DetermineWinner(img.sprite, compImage.sprite);
    }

    private void DetermineWinner(Sprite playerSprite, Sprite computerSprite)
    {
        if (playerSprite == computerSprite)
        {
            gameAnnounceText.text = "It's a Draw!";
        }
        else if ((playerSprite == images[0] && computerSprite == images[2]) ||
                (playerSprite == images[2] && computerSprite == images[1]) ||
                (playerSprite == images[1] && computerSprite == images[0]))
        {
            playerScore++;
            gameAnnounceText.text = "You Win this round! ";
        }
        else
        {
            computerScore++;
            gameAnnounceText.text = "Computer Wins this round!";
        }

        UpdateScoreUI();
        CheckMatchWinner();
    }

    private void UpdateScoreUI()
    {
        playerScoreText.text = "Player Score: " + playerScore;
        computerScoreText.text = "Computer Score: " + computerScore;
    }

    private void CheckMatchWinner()
    {
        if (playerScore >= 3)
        {
            gameAnnounceText.text = "You won the match!";
            ShowGameOverPanel("You won the match!");
        }
        else if (computerScore >= 3)
        {
            gameAnnounceText.text = "You lost the match!";
            ShowGameOverPanel("You lost the match!");
        }
    }

    public void ResetMatch()
    {
        playerScore = 0;
        computerScore = 0;
        matchOver = false;
        gameAnnounceText.text = "Match Reset. Play!";
        UpdateScoreUI();
        selectedImage.sprite = defaultSelectionImage.sprite;
        compImage.sprite = defaultSelectionImage.sprite;
        gameOverPanel.SetActive(false);

    }

    private void ShowGameOverPanel(string resultText)
    {
        matchOver = true;
        gameOverText.text = resultText;
        gameOverPanel.SetActive(true);
    }
    public void BackToHome()
    {
        SceneManager.LoadScene("MainMenu");
    }

}

