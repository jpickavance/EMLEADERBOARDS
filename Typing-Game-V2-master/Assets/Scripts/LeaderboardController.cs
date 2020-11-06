using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using System.Linq;
using TMPro;

public class LeaderboardController : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void ReadLeaderboardTop10(string tableName);
    [DllImport("__Internal")]
    private static extern void GetLeaderboardSize(string tableName);
    public string myToken;
    public int myIndex;
    public Text rank1;
    public Text rank2;
    public Text rank3;
    public Text rank4;
    public Text rank5;
    public Text rank6;
    public Text rank7;
    public Text rank8;
    public Text rank9;
    public Text rank10;
    public Text Token1;
    public Text Token2;
    public Text Token3;
    public Text Token4;
    public Text Token5;
    public Text Token6;
    public Text Token7;
    public Text Token8;
    public Text Token9;
    public Text Token10;
    public Text aveWPM1;
    public Text aveWPM2;
    public Text aveWPM3;
    public Text aveWPM4;
    public Text aveWPM5;
    public Text aveWPM6;
    public Text aveWPM7;
    public Text aveWPM8;
    public Text aveWPM9;
    public Text aveWPM10;
    public Text age1;
    public Text age2;
    public Text age3;
    public Text age4;
    public Text age5;
    public Text age6;
    public Text age7;
    public Text age8;
    public Text age9;
    public Text age10;
    public Text location1;
    public Text location2;
    public Text location3;
    public Text location4;
    public Text location5;
    public Text location6;
    public Text location7;
    public Text location8;
    public Text location9;
    public Text location10;

    public TextMeshProUGUI buttonText;
    public int n_rows;
    public int n_aaa;
    public int currentIndex;
    public int n_imported;
    public bool top10;
    public GameObject ScrollButtons;
    public Color pink;
    public Color green;

    [System.Serializable]
    public class ProfileClass // takes all the properties as strings (stringified from JSON)
    {
        public string tokenId;
        public string aveWPM;
        public string age;
        public string county1;
    }
    [System.Serializable]
    public class ProfileClass2
    {
        public string tokenId;
        public int aveWPM;
        public string age;
        public string county1;
    }
    
    public List<ProfileClass2> leaderboardData = new List<ProfileClass2>();
    public List<Text> rankCol = new List<Text>();
    public List<Text> tokenCol = new List<Text>();
    public List<Text> aveWPMCol = new List<Text>();
    public List<Text> ageCol = new List<Text>();
    public List<Text> locationCol = new List<Text>();


    void Awake()
    {
        n_imported = 0;
        currentIndex = 0;
        GetLeaderboardSize("EMLeaderboardData");
        ReadLeaderboardTop10("EMLeaderboardData");
        InitFields();
        myToken = UserInfo.Instance.username;
        top10 = true;
        AddCurrentUser();
        if (n_rows < 10)
        {
            n_aaa = 10 - n_rows + 1;
            for(int i = 0; i < n_aaa; i++)
            {
                leaderboardData.Add(new ProfileClass2()
                                {
                                    tokenId = "AAA",
                                    aveWPM = Convert.ToInt32(i),
                                    age = "-",
                                    county1 = "-"
                                });
            }
        }
    }

    public void setLeaderboardSize(int tableLength)
    {
        n_rows = tableLength;
    }

    public void AddCurrentUser()
    {
        leaderboardData.Add(new ProfileClass2()
                                {
                                    tokenId = UserInfo.Instance.username,
                                    aveWPM = Convert.ToInt32(UserInfo.Instance.averageWPM),
                                    age = UserInfo.Instance.age,
                                    county1 = UserInfo.Instance.location
                                });
    }
    ///This currently works by scanning the whole table of scores. Needs to be updated with a global secondary index (costs money per month)
    public void appendResult(string receivedData)
    {   
        ProfileClass profileItem = JsonUtility.FromJson<ProfileClass>(receivedData);
        int profileAveWPMInt = Convert.ToInt32(profileItem.aveWPM);                                         
        leaderboardData.Add(new ProfileClass2(){ tokenId = profileItem.tokenId.ToString(), 
                                                 aveWPM = profileAveWPMInt, 
                                                 age = profileItem.age.ToString(),
                                                 county1 = profileItem.county1.ToString()});
        n_imported ++;

        if(n_imported == n_rows)
        {
            leaderboardData.Sort((x, y) => y.aveWPM.CompareTo(x.aveWPM));
            myIndex = leaderboardData.FindIndex(p => p.tokenId == myToken);
            ShowLeaderBoard();
        }                      
    }

    public void ShowLeaderBoard()
    {
                for(int i = 0; i < 10; i++)
                {
                    rankCol[i].GetComponent<Text>().text = (currentIndex + i + 1).ToString();
                    tokenCol[i].GetComponent<Text>().text = leaderboardData[currentIndex + i].tokenId;
                    aveWPMCol[i].GetComponent<Text>().text = leaderboardData[currentIndex + i].aveWPM.ToString();
                    ageCol[i].GetComponent<Text>().text = leaderboardData[currentIndex + i].age;
                    locationCol[i].GetComponent<Text>().text = leaderboardData[currentIndex + i].county1;
                    
                    if(leaderboardData[currentIndex + i].tokenId == myToken)
                    {
                        rankCol[i].GetComponent<Text>().color = pink;
                        tokenCol[i].GetComponent<Text>().color = pink;
                        aveWPMCol[i].GetComponent<Text>().color = pink;
                        ageCol[i].GetComponent<Text>().color = pink;
                        locationCol[i].GetComponent<Text>().color = pink;
                    }
                    else
                    {
                        rankCol[i].GetComponent<Text>().color = green;
                        tokenCol[i].GetComponent<Text>().color = green;
                        aveWPMCol[i].GetComponent<Text>().color = green;
                        locationCol[i].GetComponent<Text>().color = green;
                    }
                }
    }

    public void errorCallback(string err)
    {
        Debug.LogError(err);
    }

    public void toggleTop10()
    {
        top10 = !top10;
        if(!top10)
        {
            if(myIndex > 3)
            {
                currentIndex = myIndex - 4; // shows 4 people above you in the leaderboard
            }
            else
            {
                currentIndex = 0;
            }
            ScrollButtons.SetActive(true);
            buttonText.text = "TOP 10";
        }
        else
        {
            currentIndex = 0; 
            ScrollButtons.SetActive(false);
            buttonText.text = "MY SCORE";
        }
        ShowLeaderBoard();
    }

    
    public void ScrollUp()
    {
        if(currentIndex > 0)
        {
            currentIndex -= 1; // subtract 1 from current index
            ShowLeaderBoard();
        }
    }
    public void ScrollDown()
    {
        if(currentIndex < (n_rows - 10))
        {
            currentIndex += 1;
            ShowLeaderBoard();
        }
    }
    

    public void InitFields()
    {
        rankCol.Add(rank1);
        rankCol.Add(rank2);
        rankCol.Add(rank3);
        rankCol.Add(rank4);
        rankCol.Add(rank5);
        rankCol.Add(rank6);
        rankCol.Add(rank7);
        rankCol.Add(rank8);
        rankCol.Add(rank9);
        rankCol.Add(rank10);
        tokenCol.Add(Token1);
        tokenCol.Add(Token2);
        tokenCol.Add(Token3);
        tokenCol.Add(Token4);
        tokenCol.Add(Token5);
        tokenCol.Add(Token6);
        tokenCol.Add(Token7);
        tokenCol.Add(Token8);
        tokenCol.Add(Token9);
        tokenCol.Add(Token10);
        aveWPMCol.Add(aveWPM1);
        aveWPMCol.Add(aveWPM2);
        aveWPMCol.Add(aveWPM3);
        aveWPMCol.Add(aveWPM4);
        aveWPMCol.Add(aveWPM5);
        aveWPMCol.Add(aveWPM6);
        aveWPMCol.Add(aveWPM7);
        aveWPMCol.Add(aveWPM8);
        aveWPMCol.Add(aveWPM9);
        aveWPMCol.Add(aveWPM10);
        ageCol.Add(age1);
        ageCol.Add(age2);
        ageCol.Add(age3);
        ageCol.Add(age4);
        ageCol.Add(age5);
        ageCol.Add(age6);
        ageCol.Add(age7);
        ageCol.Add(age8);
        ageCol.Add(age9);
        ageCol.Add(age10);
        locationCol.Add(location1);
        locationCol.Add(location2);
        locationCol.Add(location3);
        locationCol.Add(location4);
        locationCol.Add(location5);
        locationCol.Add(location6);
        locationCol.Add(location7);
        locationCol.Add(location8);
        locationCol.Add(location9);
        locationCol.Add(location10);
    }
}


