using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class DatabaseController : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void PostTrialData(string tableName, string userName, string trialNum, string wpm);

    [DllImport("__Internal")]
    private static extern void PostUserData(string tableName, string username, string age, string location, 
                                            string widthPx, string heightPx, string pxRatio, string browserVersion, 
                                            string clickedInfo, string consentProvided, string consentTime, string startTime);

    [DllImport("__Internal")]
    private static extern void InsertLeaderboardUser(string tableName, string username, string wpm, string age, string location);

    public void CallPostTrialData(string tableName, string userName, string trialNum, string wpm)
    {
        if(UserInfo.Instance.GameMode != "debug" && UserInfo.Instance.consent)
        {
            PostTrialData(tableName, userName, trialNum, wpm);
        }

    }

    public void CallPostUserData(string tableName, string username, string age, string location, 
                                 string widthPx, string heightPx, string pxRatio, string browserVersion, 
                                 string clickedInfo, string consentProvided, string consentTime, string startTime)
    {
        if(UserInfo.Instance.GameMode != "debug" && UserInfo.Instance.consent)
        {
            PostUserData(tableName, username, age, location, widthPx, heightPx, pxRatio, browserVersion, clickedInfo, consentProvided, consentTime, startTime);
        }
    }

    public void CallInsertLeaderboard(string tableName, string username, string wpm, string age, string location)
    {
        if(UserInfo.Instance.GameMode != "debug" && UserInfo.Instance.consent)
        {
            InsertLeaderboardUser(tableName, username, wpm, age, location);
        }
    }
}
