using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Adding a new name space because we're gonna need a reference to the Unity text class
using UnityEngine.UI;

public class Typer : MonoBehaviour
{
    public SentenceBank sentenceBank;
    public Metrics metrics;
    public ExperimentController experimentController;
    public DatabaseController databaseController;
    public LeaderboardController leaderboardController;
    public Text sentenceOutput;
    public Text sentenceFeedback;
    public Text endScreen;
    public Text instructions;
    public DateTime startTime;
    public DateTime endTime;
    public GameObject arrow;
    public double numWords;
    public double numChars;
    public double trialWPM;
    public int currSentNum = -1; // 

    public bool oneTime = false;

    private string remainingSentence;
    private string currentSentence;


    // Removed the default comments below and made the default functions private

    private void Start()
    {
        experimentController.TypingState();
        SetCurrentSentence();
    }

    private void SetCurrentSentence()
    {
        // Increment current sentence number
        currSentNum ++;

        if (currSentNum > 0) // only do this at the end of a sentence (currSent starts at 0) 
        {
            instructions.text = "";

            if(UserInfo.Instance.GameMode != "debug")
            {
            databaseController.CallPostTrialData("EMTrialData", 
                                                UserInfo.Instance.username,
                                                currSentNum.ToString(),
                                                trialWPM.ToString());
            }
            
        }
        
            
        if(currSentNum < 5) // change 5 to num sentences
        { 
            currentSentence = sentenceBank.GetWord();
            numWords = currentSentence.Split(' ').Length;
            numChars = currentSentence.Length; // spaces included, which is necessary
            SetRemainingSentence(currentSentence);
        }
        else // all words are completed
        { 

            // I'd rather not put the end screen state on first because there might be slight
            // lag for when the things are ready to display.
            // But I need to have the arrow active to find its pos. (Could set arrow active separately)
            experimentController.EndScreenState();
            UserInfo.Instance.averageWPM = metrics.CalcAveWPM(UserInfo.Instance.wpmArray);
            UserInfo.Instance.percentile = metrics.CalcAveWPMPercentile(UserInfo.Instance.averageWPM);
            endScreen.text = "Your average WPM: " + UserInfo.Instance.averageWPM.ToString() + "\n" +
            "Your percentile is " + Math.Round(UserInfo.Instance.percentile,3).ToString();
            float arrowXPos = Convert.ToSingle(metrics.CalcArrowXPos(UserInfo.Instance.averageWPM));
            Vector3 currentArrowPos = GameObject.Find("Arrow").transform.position;
            // position 1 in the vector is the second position
            arrow.GetComponent<Transform>().position = new Vector3(arrowXPos, currentArrowPos[1], 0);

                        //*JP ADDED CALL TO LEADERBOARD INSERT USER HERE
            databaseController.CallInsertLeaderboard("EMLeaderboardData",
                                                     UserInfo.Instance.username,
                                                     UserInfo.Instance.averageWPM.ToString(),
                                                     UserInfo.Instance.age,
                                                     UserInfo.Instance.location);

           }
        
        }

    private void SetRemainingSentence(string newString)
    {
        // set the current word and update it on screen
        remainingSentence = newString;
        sentenceOutput.text = remainingSentence;
    }

    private void Update() // every frame
    {
        CheckInput();
    }

    private void CheckInput()
    {
        if(Input.anyKeyDown)
        {
            string keysPressed = Input.inputString;

            // If multiple keys are pressed in the exact same frame, ignore that

            if(keysPressed.Length == 1)
            {
                EnterLetter(keysPressed);
            }

        }

    }

    private void EnterLetter(string typedLetter)
    {
        // Uses a string rather than a character because that apparently simplifies a few things for us
        // if the letter is correct,
        //   if it's the first letter ( grab the start time )
        //    remove that letter from remainingSentence and update this on screen
        //    if it's the last letter ( calculate speeds, display a new word )

        // If letter is correct
        if(IsCorrectLetter(typedLetter))
        {
            // If it's the first letter
            if(IsFirstLetter())
            {
                // Get the start time
                startTime = DateTime.Now;
                
            }
            
            //Remove the first letter from the displayed string
            RemoveLetter();

            // If that was the last letter
            if(IsSentenceComplete())
            {
                // grab the end time
                endTime = DateTime.Now;
                //calc trialWPM
                trialWPM = Math.Round(metrics.CalcWPM(startTime, endTime, numChars),0);
                
                //provide feedback
                FeedbackWPM();
                // put trialWPM in array
                AddToArrayWPM();
                // get a new word/sentence
                SetCurrentSentence();
            }
          
        }

    }

    private bool IsCorrectLetter(string letter)
    {
        // Check whether the inputted letter is the first letter of the remaining word
        return remainingSentence.IndexOf(letter) == 0;
    }

    private void RemoveLetter()
    {
        // Remove the first character from the string
        string newString = remainingSentence.Remove(0,1);
        SetRemainingSentence(newString);
    }

    private bool IsSentenceComplete()
    {
        // Return TRUE if the remaining word has zero letters
        return remainingSentence.Length == 0;
    }

        private bool IsFirstLetter()
    {
        // Return TRUE if this is the first letter entered
        return currentSentence.Length == remainingSentence.Length;
    }

    private void FeedbackWPM()
    {
        // show Feedback text
        sentenceFeedback.text = "(WPM = " + trialWPM + ")";
        experimentController.FeedbackState();
        
        // wait for 1 secs
        StartCoroutine(feedbackWait());
        

    }      

    private IEnumerator feedbackWait()
    {
        yield return new WaitForSeconds(1);

        // hide feedback text
        experimentController.TypingState();

    }

    private void AddToArrayWPM()
    {
        UserInfo.Instance.wpmArray[currSentNum] = trialWPM;

    }
    
}