using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Menu : MonoBehaviour
{
    /*
    //js headers  
    [DllImport("__Internal")]
    private static extern string ReadData (string tableName, string token);
    [DllImport("__Internal")]
    private static extern string UpdateToken (string tableName, string token); 
    */

    [DllImport("__Internal")]
    private static extern string getScreenWidth();
    [DllImport("__Internal")]
    private static extern string getScreenHeight();
    [DllImport("__Internal")]
    private static extern string getPixelRatio();
    [DllImport("__Internal")]
    private static extern string getBrowserVersion();
    
    

    // game objects and elements

    DatabaseController databaseController;
    public GameObject infoPopUp;
    public GameObject consentPopUp;
    public Text errorMessage;
    public Text consentMessage;
    public Text consentInfo;
    public TMP_InputField ageField;
    public TMP_InputField usernameField;
    public TMP_Dropdown ukCountyField;
    public Button consentButton;
    public Button consentAcceptButton;
    public Toggle consentToggle1;
    public Toggle consentToggle2;
    public Toggle consentToggle3;
    public Toggle consentToggle4;
    public Toggle consentToggle5;
    public Toggle consentToggle6;



    // variables

    public string enteredAge;
    public int age = 0;
    public bool ageProvided;
    public bool genderProvided;
    public bool ukCountyProvided;
    public bool consentDecided;
    public bool clickedInfo;
    public string consentTime;
    public string startTime;


    private List<string> counties = new List<string>()
    { 
        "Please select...",
        "Not in the UK",
        "Aberdeenshire",
        "Angus",
        "Antrim",
        "Argyll & Bute",
        "Armagh",
        "Ayrshire",
        "Banffshire",
        "Bath and North East Somerset",
        "Bedfordshire",
        "Berkshire",
        "Berwickshire",
        "Blaenau Gwent",
        "Borders",
        "Bridgend",
        "Bristol",
        "Buckinghamshire",
        "Caerphilly",
        "Caithness",
        "Cambridgeshire",
        "Cardiff",
        "Carmarthenshire",
        "Ceredigion",
        "Cheshire",
        "Clackmannanshire",
        "Conwy",
        "Cornwall",
        "County Durham",
        "Cumbria",
        "Denbighshire",
        "Derbyshire",
        "Devon",
        "Down",
        "Dumfries & Galloway",
        "Dunbartonshire",
        "East Ayrshire",
        "East Dunbartonshire",
        "East Lothian",
        "East Renfrewshire",
        "East Riding of Yorkshire",
        "East Sussex",
        "Essex",
        "Fermanagh",
        "Fife",
        "Flintshire",
        "Gloucestershire",
        "Greater London",
        "Greater Manchester",
        "Gwynedd",
        "Hampshire",
        "Herefordshire",
        "Hertfordshire",
        "Highland",
        "Inverclyde",
        "Isle of Anglesey",
        "Isle of Wight",
        "Isles of Scilly",
        "Kent",
        "Kincardineshire",
        "Lancashire",
        "Leicestershire",
        "Lincolnshire",
        "Londonderry",
        "Merseyside",
        "Merthyr Tydfil",
        "Midlothian",
        "Monmouthshire",
        "Moray",
        "Neath Port Talbot",
        "Newport",
        "Norfolk",
        "North Ayrshire",
        "North Lanarkshire",
        "North Somerset",
        "North Yorkshire",
        "Northamptonshire",
        "Northumberland",
        "Nottinghamshire",
        "Orkney",
        "Oxfordshire",
        "Pembrokeshire",
        "Perth & KInross",
        "Powy",
        "Renfrewshire",
        "Rhondda Cynon Taff",
        "Rutland",
        "Shetland",
        "Shropshire",
        "Somerset",
        "South Ayrshire",
        "South Gloucestershire",
        "South Lanarkshire",
        "South Yorkshire",
        "Staffordshire",
        "Stirlingshire",
        "Suffolk",
        "Surrey",
        "Swansea",
        "Torfaen",
        "Tyne & Wear",
        "Tyrone",
        "Vale of Glamorgan",
        "Warwickshire",
        "West Dunbartonshire",
        "West Lothian",
        "West Midlands",
        "West Sussex",
        "West Yorkshire",
        "Western Isles",
        "WIltshire",
        "Worcestershire",
        "Wrexham"

    };
     private List<string> adjectives = new List<string>()
     {
        "Active",
        "Adventurous",
        "Agile",
        "Alert",
        "Amusing",
        "Angry",
        "Annoyed",
        "Athletic",
        "Aware",
        "Bashful",
        "Beaming",
        "Beautiful",
        "Big",
        "Bitter",
        "Blissful",
        "Brave",
        "Brilliant",
        "Busy",
        "Calm",
        "Capable",
        "Cautious",
        "Challenging",
        "Charming",
        "Cheerful",
        "Chilly",
        "Chocolatey",
        "Clever",
        "Cloudy",
        "Compassionate",
        "Considerate",
        "Cozy",
        "Cranky",
        "Creative",
        "Crispy",
        "Crunchy",
        "Dangerous",
        "Daring",
        "Dark",
        "Delicate",
        "Delightful",
        "Ecstatic",
        "Elated",
        "Empty",
        "Endless",
        "Enormous",
        "Entertaining",
        "Equal",
        "Exhausted",
        "Fantastic",
        "Flexible",
        "Fluffy",
        "Freezing",
        "Frenetic",
        "Funny",
        "Furious",
        "Fussy",
        "Generous",
        "Gentle",
        "Gigantic",
        "Glad",
        "Gleeful",
        "Gorgeous",
        "Graceful",
        "Harmonious",
        "Icky",
        "Icy",
        "Infinite",
        "Intelligent",
        "Jaded",
        "Jolly",
        "Jovial",
        "Joyful",
        "Joyous",
        "Jumpy",
        "Kind",
        "Kindly",
        "Knowledgeable",
        "Large",
        "Lazy",
        "Left",
        "Light",
        "Likely",
        "Lousy",
        "Loyal",
        "Lucky",
        "Lumpy",
        "Marvellous",
        "Mean",
        "Minty",
        "Mysterious",
        "Naive",
        "Nervous",
        "New",
        "Nice",
        "Nimble",
        "Optimistic",
        "Oval",
        "Peaceful",
        "Petite",
        "Pleasant",
        "Pleased",
        "Polite",
        "Precise",
        "Pretty",
        "Proud",
        "Quick",
        "Quiet",
        "Rainy",
        "Relaxing",
        "Restful",
        "Right",
        "Serene",
        "Shocking",
        "Short",
        "Simple",
        "Skilful",
        "Slow",
        "Small",
        "Soothing",
        "Sour",
        "Sparkling",
        "Speedy",
        "Spiky",
        "Still",
        "Straight",
        "Strong",
        "Stubborn",
        "Stunning",
        "Sunny",
        "Swift",
        "Tall",
        "Terrified",
        "Thrilled",
        "Timid",
        "Tiny",
        "Tranquil",
        "Tricky",
        "Truthful",
        "Whimsical",
        "Wise",
        "Young"

     };
    private List<string> colours = new List<string>()
    {
        "Black",
        "Blue",
        "Bronze",
        "Brown",
        "Burgundy",
        "Copper",
        "Coral",
        "Crimson",
        "Cyan",
        "Emerald",
        "Fuchsia",
        "Gold",
        "Gray",
        "Green",
        "Indigo",
        "Ivory",
        "Khaki",
        "Lavendar",
        "Lilac",
        "Lime",
        "Magenta",
        "Maroon",
        "Navy",
        "Orange",
        "Peach",
        "Red",
        "Silver",
        "Teal",
        "Turquoise",
        "Violet",
        "White",
        "Yellow"

    };
    private List<string> animals = new List<string>()
    {
        "Alligator",
        "Alpaca",
        "Antelope",
        "Ape",
        "Armadillo",
        "Baboon",
        "Badger",
        "Bat",
        "Bear",
        "Bee",
        "Bison",
        "Boar",
        "Buffalo",
        "Butterfly",
        "Cat",
        "Cattle",
        "Cheetah",
        "Chicken",
        "Cod",
        "Coyote",
        "Crow",
        "Deer",
        "Dinosaur",
        "Dog",
        "Dolphin",
        "Donkey",
        "Dove",
        "Duck",
        "Eagle",
        "Eel",
        "Elephant",
        "Elk",
        "Emu",
        "Falcon",
        "Ferret",
        "Finch",
        "Fish",
        "Flamingo",
        "Fly",
        "Fox",
        "Frog",
        "Gerbil",
        "Giraffe",
        "Goat",
        "Goose",
        "Gorilla",
        "Grasshopper",
        "Grouse",
        "Guineapig",
        "Gull",
        "Hamster",
        "Hawk",
        "Hedgehog",
        "Heron",
        "Hippopotamus",
        "Hog",
        "Hornet",
        "Horse",
        "Hound",
        "Hummingbird",
        "Hyena",
        "Jellyfish",
        "Kangaroo",
        "Koala",
        "Lark",
        "Leopard",
        "Lion",
        "Llama",
        "Magpie",
        "Mallard",
        "Mole",
        "Monkey",
        "Moose",
        "Mosquito",
        "Mouse",
        "Mule",
        "Nightingale",
        "Ostrich",
        "Otter",
        "Owl",
        "Ox",
        "Oyster",
        "Panda",
        "Parrot",
        "Penguin",
        "Pheasant",
        "Pig",
        "Pigeon",
        "Platypus",
        "Porpoise",
        "Possum",
        "Rabbit",
        "Raccoon",
        "Rat",
        "Raven",
        "Reindeer",
        "Rhinoceros",
        "Seal",
        "Shark",
        "Sheep",
        "Snake",
        "Sparrow",
        "Spider",
        "Squid",
        "Squirrel",
        "Swan",
        "Tiger",
        "Toad",
        "Trout",
        "Turkey",
        "Turtle",
        "Walrus",
        "Wasp",
        "Weasel",
        "Whale",
        "Wolf",
        "Wombat",
        "Woodpecker",
        "Wren",
        "Yak",
        "Zebra"
    };
    // Start is called before the first frame update
    void Start()
    {
        // add counties to dropdown list
        PopulateCounties();

        // generate username
        GenerateUsername();

        UserInfo.Instance.consent = false;
        if(UserInfo.Instance.GameMode != "debug")
        {
            UserInfo.Instance.widthPx = getScreenWidth();
            UserInfo.Instance.heightPx = getScreenHeight();
            UserInfo.Instance.pxRatio = getPixelRatio();
            UserInfo.Instance.browserVersion = getBrowserVersion();
            //fullscreenMenuListener();
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateUsername()
    {
        // generate a random number between 1 and the max number of username combos
        System.Random rnd = new System.Random();
        int usernameNum = rnd.Next(1, adjectives.Count * colours.Count * animals.Count);
        
        string name = GetNthUsername(usernameNum, adjectives, colours, animals);
        // get the username for this number and assign it to the username textbox
        usernameField.text = name;

        UserInfo.Instance.username = name;

    }
      public void ShowInfo()
    {
        // make a note that they clicked on the info - will save this in the user DB
        clickedInfo = true;

        //Screen.fullScreen = true;
        infoPopUp.SetActive(true);
    }

     public void HideInfo()
    {
        // make a note that they clicked on the info - will save this in the user DB

        //Screen.fullScreen = true;
        infoPopUp.SetActive(false);
    }

    public void CheckIfCanPartipate()
    {
    if(!ageProvided)
        {
            // if they need an error message
            errorMessage.text = "Please provide your age.";
        }
        else if(!ukCountyProvided)
        {
            errorMessage.text = "Please provide your UK county.";
        }
        else
        {
            //can participate
            errorMessage.text = "";
            ShowConsent();
            
        }
    }

     public void ShowConsent()
    {
        //Debug.Log("clicked");
        //Screen.fullScreen = true;
        consentPopUp.SetActive(true);
    }

    public void CheckValidConsent()
    {
        if(consentToggle1.isOn && consentToggle2.isOn && consentToggle3.isOn && consentToggle4.isOn
        && consentToggle5.isOn && consentToggle6.isOn)
        {
            //consentAcceptButton.interactable = true;
            consentInfo.text = "";
            consentTime = DateTime.Now.ToString();
            GiveConsent();
        }
        else
        {
            //consentAcceptButton.interactable = false;
            consentInfo.text = "You must confirm that you meet the criteria by ticking the boxes in order to participate.";
        }
    }
    public void GiveConsent()
    {
        UserInfo.Instance.consent = true;
        UserInfo.Instance.consentTime = DateTime.Now;
        //errMessage.text = "";
        consentPopUp.SetActive(false);
        consentDecided = true;
    }

    public void RefuseConsent()
    {
        //UserInfo.Instance.consent = false;
        consentPopUp.SetActive(false);
        consentDecided = true;
    }

    public void ValidateAge()
    {
        // take entered age
        enteredAge = ageField.text.ToString();

        // convert to an integer
        bool isNumeric = int.TryParse(enteredAge, out age);
        
        if ((isNumeric == true) && (age >= 1 && age <= 120))
        {
            errorMessage.text = "";
            ageProvided = true;
        }
        else
        {
            errorMessage.text = "Please enter your age as a full number between 1 and 120.";
            ageProvided = false;
        }

        DenyConsentAccessIfYoung();
    
    }

    public void AgePreferNTS()
    {
        // disable age

        // disable consent
    }
    public void DenyConsentAccessIfYoung()
    {
        // if < 18
        if ((age >= 1 && age < 18 )|| age > 120)
        {
            consentButton.interactable = false;
            consentMessage.text = "All ages can play, but only over 18s\ncan participate in the research.";
        }
        else if (age >= 18 && age <= 120)
        {
            consentButton.interactable = true;
            consentMessage.text = "";
        }
    }

    public void CheckCountyProvided()
    {
        if (ukCountyField.options[ukCountyField.value].text != "Please select...")
        {
            ukCountyProvided = true;
        }
        else
        {
            ukCountyProvided = false;
        }
    }
    
    public void HideConsentExplanIfTicks()
    {
        if(consentToggle1.isOn && consentToggle2.isOn && consentToggle3.isOn && consentToggle4.isOn
        && consentToggle5.isOn && consentToggle6.isOn)
        {
            //consentAcceptButton.interactable = true;
            consentInfo.text = "";
        }
        else
        {
            //consentAcceptButton.interactable = false;
            //consentInfo.text = "Everyone can play Turbo Typing, but only those who fit the criteria above can participate in the\nresearch. If you cannot tick all the boxes, please click 'I will not participate in the research'.";
        }
    }

   
   public void CheckIfCanPlay() 
    { 
        /*
        can play if
        - entered age
        - entered county
        - if over 18: decided consent
        */
        
        if(!ageProvided)
        {
            // if they need an error message
            errorMessage.text = "Please provide your age.";
        }
        else if(!ukCountyProvided)
        {
            errorMessage.text = "Please provide your UK county.";
        }
        else if(age >= 18 && age <= 120 && !consentDecided)
        {
            errorMessage.text = "Please decide if you would like to participate in the research.";
        }
        else
        {
            UserInfo.Instance.location = ukCountyField.options[ukCountyField.value].text;
            UserInfo.Instance.age = age.ToString();
            //can play
            databaseController.CallPostUserData("EMUserData",
                                                UserInfo.Instance.username,
                                                UserInfo.Instance.age,
                                                UserInfo.Instance.location,
                                                UserInfo.Instance.widthPx,
                                                UserInfo.Instance.heightPx,
                                                UserInfo.Instance.pxRatio,
                                                UserInfo.Instance.browserVersion,
                                                clickedInfo.ToString(),
                                                UserInfo.Instance.consent.ToString(),
                                                consentTime,
                                                DateTime.Now.ToString());

            SceneManager.LoadScene("TypingGame");
        }
        
        

        /* JP below:
        if((UserInfo.Instance.GameMode != "debug" && UserInfo.Instance.tokenId != "notryan") && (UserInfo.Instance.consent == false || !handTicked || !pointerTicked))
        {
            if(!handTicked)
            {
                errMessage.text = "You must select your preferred hand before participating in the experiment";
            }
            else if(!pointerTicked)
            {
                errMessage.text = "You must select your pointer device before participating in the experiment";
            }
            else if(UserInfo.Instance.consent == false)
            {
                errMessage.text = "You must provide your consent before participating in the experiment";
            }
        }
        else
        {
            if(UserInfo.Instance.GameMode != "debug" && UserInfo.Instance.tokenId != "notryan")
            {
                ReadData("tokenTable", UserInfo.Instance.tokenId.ToString());
            }
            else
            {
                SceneManager.LoadScene("SettingsMenu");
            }
        }
        */

       
    }

     public void PopulateCounties()
        {
            //Clear the old options of the Dropdown menu
            ukCountyField.ClearOptions();
            //Add the options created in the List above
            ukCountyField.AddOptions(counties);
            
        }


  /// Get the n-th unique user name constructed using one element from each of the given lists in order
  public static string GetNthUsername(int n, List<string> pt1, List<string> pt2, List<string> pt3)
  {
    // The maximum number of unique names possible
    int maxNames = pt1.Count * pt2.Count * pt3.Count;
    // If n is bigger than the maximum number of names (minus 1), add a counter to the end of the generated name
    int overflowLoops = n / maxNames;
    string overflowId = overflowLoops > 0 ? "-"+overflowLoops.ToString() : "";

    // Get the correct index for each list, given the current ID number n
    n = n % maxNames;
    int index1 = n / (pt2.Count * pt3.Count);
    int index2 = (n - index1 * pt2.Count * pt3.Count) / pt3.Count;
    int index3 = (n - (index1 * pt2.Count * pt3.Count) - (index2 * pt3.Count));

    return pt1[index1] + "-" + pt2[index2] + "-" + pt3[index3] + overflowId;
  } 
     
}
