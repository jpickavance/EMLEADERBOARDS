using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class EndScreenController : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void OpenWindow(string link);
    public GameObject Leaderboard;
    public void SetLeaderboardActive()
    {
        Leaderboard.SetActive(true);
    }
    public void ExitLeaderboard()
    {
        Leaderboard.SetActive(false);
    }

    public void GoTwitter()
    {
        OpenWindow("https://twitter.com/ICON_UoL");
    }
}

