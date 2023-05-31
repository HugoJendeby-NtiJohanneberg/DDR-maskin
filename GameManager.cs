using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public AudioSource theMusic; // Reference to the audio source for the music

    public bool startPlaying; // Flag to indicate if the music and game have started playing

    public BeatScroller theBS; // Reference to the BeatScroller script

    public static GameManager instance; // Static instance of the GameManager (Static means there can only be on variable/instance for GameManager)

    // Different scores added to currentScore depending on note hit timing
    public int currentScore; 
    public int scorePerNote = 100; 
    public int scorePerGoodNote = 125; 
    public int scorePerPerfectNote = 150; 

    
    public int currentMultiplier; // Current multiplier value
    public int multiplierTracker; // Counter to track the number of notes hit within a multiplier range
    public int[] multiplierThresholds; // Thresholds for increasing the multiplier

    // Text elements displaying current score and multiplier
    public Text scoreText; 
    public Text multiText;

    // Notes hit, displayed in result screen
    public float totalNotes; 
    public float normalHits; 
    public float goodHits; 
    public float perfectHits; 
    public float missedHits; 

    // Text elements to display information about performance on resultsScreen object
    public GameObject resultsScreen;
    public Text percentHitText, normalsText, goodsText, perfectsText, missesText, rankText, finalScoreText;


    void Start()
    {
        instance = this; // Set the static instance to this GameManager

        // Initialize score text and multiplier
        scoreText.text = "Score: 0"; 
        currentMultiplier = 1; 

        totalNotes = FindObjectsOfType<NoteObject>().Length; // Find the number of NoteObject instances in the scene
    }

    void Update()
    {
        // If statement starts game, BeatScroller and music upon any key pressed if game has not started.
        if (!startPlaying) 
        {
            if (Input.anyKeyDown) 
            {
                startPlaying = true;
                theBS.hasStarted = true;

                theMusic.Play(); 
            }
        }
        else
        {
            if (!theMusic.isPlaying && !resultsScreen.activeInHierarchy) // If the music has stopped and the results screen is not active 
            {
                resultsScreen.SetActive(true); // Activate the results screen

                // Display notes hit and missed
                normalsText.text = "" + normalHits; 
                goodsText.text = goodHits.ToString(); 
                perfectsText.text = perfectHits.ToString(); 
                missesText.text = "" + missedHits;

                float totalHit = normalHits + goodHits + perfectHits; 
                float percentHit = (totalHit / totalNotes) * 100f; 

                percentHitText.text = percentHit.ToString("f1") + "%"; // Display the percentage of notes hit with one decimal place ("f1" = show this as a float value with one decimal place)

                // If statements determining rank shown at resultsScreen depending on percentage of notes hit (Initialized as "F")
                string rankVal = "F"; 
  
                if (percentHit > 40) 
                {
                    rankVal = "D"; 
                    if (percentHit > 55) 
                    {
                        rankVal = "C"; 
                        if (percentHit > 70) 
                        {
                            rankVal = "B"; 
                            if (percentHit > 85) 
                            {
                                rankVal = "A";
                                if (percentHit > 95) 
                                {
                                    rankVal = "S"; 
                                }
                            }
                        }
                    }
                }

                rankText.text = rankVal; // Display the player's rank

                finalScoreText.text = currentScore.ToString(); // Display the final score
            }
        }
    }

    // Called when a note is hit
    public void NoteHit()
    {
        Debug.Log("Hit On Time");

        if (currentMultiplier - 1 < multiplierThresholds.Length) // Check if the currentMultiplier is within the thresholds array
        {
            multiplierTracker++; // Increment the multiplier tracker

            if (multiplierThresholds[currentMultiplier - 1] <= multiplierTracker) // Check if the tracker has reached the threshold for the current multiplier
            {
                multiplierTracker = 0; // Reset the tracker
                currentMultiplier++; // Increase the multiplier
            }
        }

        multiText.text = "Multiplier: x" + currentMultiplier; // Update the multiplier text

        scoreText.text = "Score: " + currentScore; // Update the score text
    }

    // Functions called depending on note timing, increase the score by respective score and calls NoteHit(), increment number of respective hits
    public void NormalHit()
    {
        currentScore += scorePerNote + currentMultiplier; 
        NoteHit(); // Handles the multiplier and text updates

        normalHits++; 
    }

    public void GoodHit()
    {
        currentScore += scorePerGoodNote + currentMultiplier; 
        NoteHit(); 

        goodHits++; 
    }

    public void PerfectHit()
    {
        currentScore += scorePerPerfectNote + currentMultiplier;
        NoteHit(); 

        perfectHits++; 
    }

    // Called when a note is missed
    public void NoteMissed()
    {
        Debug.Log("Missed Note");

        currentMultiplier = 1; // Reset the multiplier to 1
        multiplierTracker = 0; // Reset the multiplier tracker

        multiText.text = "Multiplier: x" + currentMultiplier; // Update the multiplier text

        missedHits++; // Increment the number of missed hits
    }
}