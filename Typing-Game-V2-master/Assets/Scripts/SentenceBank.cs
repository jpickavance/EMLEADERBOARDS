using System.Linq; // Using this to cleanly access the last object in a list
using System.Collections.Generic;
using UnityEngine;

public class SentenceBank : MonoBehaviour
{

    private List<string> originalSentences = new List<string>()
    // Original list of sentences to complete
    // Once our game actually starts, we want to copy everything in this to another list (workingSentences). 
    // Then once those sentences are completed, we're gonna be removing it from that secondary list (workingSentences)
    // Hard-coding a list right here to keep it simple, but could change it to read from a text file.
    { 
        "Back off man, I'm a scientist.",
        "We're going to need a bigger boat.",
        "Do, or do not. There is no try.",
        "Where we're going, we don't need roads!",
        "Come with me if you want to live."

       //"a", "a", "a", "a", "a"
    };

    
    private List<string> workingSentences = new List<string>();

    private void Awake()
    // Awake is used to initialize any variables or game state before the game starts.
    // Awake is called only once during the lifetime of the script instance.
    // Grab everything in our originalSentences list and add it to our workingSentences list
    {
        // Using AddRange instead of equal to because all that's gonna do is pass it by a reference - 
        // so any time we change anything on this working Sentences list, it's gonna affect the originalSentences list,
        // which we don't want.
        // This effectively copies all of those strings over to a new list.
        workingSentences.AddRange(originalSentences);
        Shuffle(workingSentences);
        //ConvertToLower(workingSentences);
    
    }

    // 
    private void Shuffle(List<string> list)
    // Randomises the Sentences
    {
        for (int i = 0; i < list.Count; i++)
        {
            int random = Random.Range(i, list.Count);
            string temporary = list[i];

            list[i] = list[random];
            list[random] = temporary;
        }
    }

/*     private void ConvertToLower(List<string> list)
    // Convert all input into lower case. (((I probably don't want this if I'm doing sentences)))
    {
        for(int i = 0; i < list.Count; i++)
        list[i] = list[i].ToLower();
    } */

    public string GetWord()
    // Grabs the next word/sentence for use (not technically next - grabs the last)
    {
        string newWord = string.Empty;

        // if we have Sentences left
        if(workingSentences.Count != 0)
        {
            // let's get the last word/sentence on the list
            // getting the last rather than the first so when we're getting things from it, 
            // we're not making all the other strings shift down - instead we're just pulling off the end.

            newWord = workingSentences.Last();
            // remove the new word/sentence from the list
            workingSentences.Remove(newWord);
        }
        return newWord;
    }
}
