using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentController : MonoBehaviour
{

    //public string gameState;
    public GameObject sentenceOutputObject;
    public GameObject sentenceFeedbackObject;
    public GameObject endScreenObject;
    public GameObject arrow;
    public GameObject distribution;
    public bool endOfGame = false;

    //public DatabaseController databaseController;

    public void TypingState()
    {
        if(!endOfGame)
            {
            sentenceOutputObject.SetActive(true);
            sentenceFeedbackObject.SetActive(false);
            endScreenObject.SetActive(false);
            arrow.SetActive(false);
            distribution.SetActive(false);
            }
    }

    public void FeedbackState()
    {
        if(!endOfGame)
            {
            sentenceOutputObject.SetActive(false);
            sentenceFeedbackObject.SetActive(true);
            endScreenObject.SetActive(false);
            arrow.SetActive(false);
            distribution.SetActive(false);
            }
    }

    public void EndScreenState()
    {
        endOfGame = true;
        sentenceOutputObject.SetActive(false);
        sentenceFeedbackObject.SetActive(false);
        endScreenObject.SetActive(true);
        arrow.SetActive(true);
        distribution.SetActive(true);
    }
    
}
