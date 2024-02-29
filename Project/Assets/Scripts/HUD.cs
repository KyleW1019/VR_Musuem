using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class HUD : MonoBehaviour
{
    // Initialize HUD GameObject
    [SerializeField] private GameObject HUDGO;
    
    // Initialize player GO and Object
    [SerializeField] private GameObject player;
    private PlayerScript Player;
    
    // Initialize Graph GO and Object
    [SerializeField] private GameObject graphGO;
    private Graph graphScript;
    
    // Initialize Period Menu Game Objects and TMPs
    [SerializeField] private GameObject period_GO;
    [SerializeField] private GameObject period_neg2_GO;
    [SerializeField] private GameObject period_neg1_GO;
    [SerializeField] private GameObject period_selected_GO;
    [SerializeField] private GameObject period_plus1_GO;
    [SerializeField] private GameObject period_plus2_GO;
    private TextMeshPro periodNeg2TMP;
    private TextMeshPro periodNeg1TMP;
    private TextMeshPro periodSelectedTMP;
    private TextMeshPro periodPlus1TMP;
    private TextMeshPro periodPlus2TMP;

    // Initialize Artist Menu Game Objects and TMPs
    [SerializeField] private GameObject artist_GO;
    [SerializeField] private GameObject artist_neg2_GO;
    [SerializeField] private GameObject artist_neg1_GO;
    [SerializeField] private GameObject artist_selected_GO;
    [SerializeField] private GameObject artist_plus1_GO;
    [SerializeField] private GameObject artist_briefs_GO;
    [SerializeField] private GameObject artist_brief1_GO;
    [SerializeField] private GameObject artist_brief2_GO;
    [SerializeField] private GameObject artist_brief3_GO;
    [SerializeField] private GameObject artist_brief4_GO;
    private TextMeshPro artistNeg2TMP;
    private TextMeshPro artistNeg1TMP;
    private TextMeshPro artistSelectedTMP;
    private TextMeshPro artistPlus1TMP;
    private TextMeshPro artistBrief1TMP;
    private TextMeshPro artistBrief2TMP;
    private TextMeshPro artistBrief3TMP;
    private TextMeshPro artistBrief4TMP;
    private List<TextMeshPro> briefTMPs;
    private List<float> artistPlus1Pos = new List<float> { -0.7f, -0.14f, -0.20f, -0.26f, -0.31f };
    private float artistPlus1x = 8.65f;
    private float artistPlus1z = 1.88f;
    
    // Initialize Exhibit Menu Game Objects and TMPs
    [SerializeField] private GameObject exhibit_GO;
    [SerializeField] private GameObject exhibit_neg2_GO;
    [SerializeField] private GameObject exhibit_neg1_GO;
    [SerializeField] private GameObject exhibit_selected_GO;
    [SerializeField] private GameObject exhibit_plus1_GO;
    [SerializeField] private GameObject exhibit_plus2_GO;
    private TextMeshPro exhibitNeg2TMP;
    private TextMeshPro exhibitNeg1TMP;
    private TextMeshPro exhibitSelectedTMP;
    private TextMeshPro exhibitPlus1TMP;
    private TextMeshPro exhibitPlus2TMP;

    // Selected Exhibit Game Objects and TMPs
    [SerializeField] private GameObject selected_exhibit_GO;    
    [SerializeField] private GameObject exhibit_Name_GO;
    [SerializeField] private GameObject exhibit_Desc_GO;
    [SerializeField] private GameObject exhibit_NavTo_GO;
    private string NavToString = "Right Click to Navigate to ";
    private TextMeshPro exhibitNameTMP;
    private TextMeshPro exhibitDescTMP;
    private TextMeshPro exhibitNavToTMP;

    // Initialize Biography Game Objects and TMPs
    [SerializeField] private GameObject Bio_Pane;
    [SerializeField] private GameObject Bio_Artist;
    [SerializeField] private GameObject BIO_Content;
    private TextMeshPro bioArtistTMP;
    private TextMeshPro bioContentTMP;
    
    // Navigation Prompts
    [SerializeField] private GameObject navPrompts;
    private TextMeshPro navigatingToTMP;
    
    // Currently Viewing Prompt
    [SerializeField] private GameObject currViewGO;
    [SerializeField] private GameObject currViewDescGO;
    private TextMeshPro currViewTMP;
    private TextMeshPro currViewDescTMP;
    private GameObject nearestArtist;
    private string nearestArtistName;
    
    // Loading Prompt
    [SerializeField] private GameObject loadingPrompt;

    // Initialize Empty Game Object in which Exhibit Game Objects are nested
    [SerializeField] private GameObject exhibitCollectionGO;

    // Initialize Lists to display Periods, Artists, and Exhibits
    private List<string> displayPeriods;
    private List<Artist> displayArtists;
    private List<Exhibit> displayExhibits;
    private int selectedPeriodIndex;
    private int selectedArtistIndex;
    private int selectedExhibitIndex;

    // Initialize Dictionaries used to parse Artist Document
    private Dictionary<string, Period> periodDict;
    private Dictionary<string, Artist> artistDict;
    
    // Navigation Variables
    private bool navigatingToExhibit = false;
    public bool NavigatingToExhibit => navigatingToExhibit;
    private Node prevNearestNode;
    private Node destExhibitNode;
    private string destExhibitName;
    private Vector3 destNodePos;
    

    // Enumerator for Selected Menu
    private enum LRCursor
    {
        Period,
        Artist,
        ExhibitSelection,
        Bio,
        Exhibit
    }
    private LRCursor currentPane;

    private bool hudActive = false;
    public bool HUDActive => hudActive;
    
    // Start is called before the first frame update
    void Start()
    {
        // Display Loading Prompt
        loadingPrompt.SetActive(true);
        
        // Initialize Player Script
        Player = player.GetComponent<PlayerScript>();
        
        // Initialize Graph Script
        graphScript = graphGO.GetComponent<Graph>();
        
        // Pull TMPs from Game Objects
        periodNeg2TMP = period_neg2_GO.GetComponent<TextMeshPro>();
        periodNeg1TMP = period_neg1_GO.GetComponent<TextMeshPro>();
        periodSelectedTMP = period_selected_GO.GetComponent<TextMeshPro>();
        periodPlus1TMP = period_plus1_GO.GetComponent<TextMeshPro>();
        periodPlus2TMP = period_plus2_GO.GetComponent<TextMeshPro>();

        artistNeg2TMP = artist_neg2_GO.GetComponent<TextMeshPro>();
        artistNeg1TMP = artist_neg1_GO.GetComponent<TextMeshPro>();
        artistSelectedTMP = artist_selected_GO.GetComponent<TextMeshPro>();
        artistPlus1TMP = artist_plus1_GO.GetComponent<TextMeshPro>();
        artistBrief1TMP = artist_brief1_GO.GetComponent<TextMeshPro>();
        artistBrief2TMP = artist_brief2_GO.GetComponent<TextMeshPro>();
        artistBrief3TMP = artist_brief3_GO.GetComponent<TextMeshPro>();
        artistBrief4TMP = artist_brief4_GO.GetComponent<TextMeshPro>();

        exhibitNeg2TMP = exhibit_neg2_GO.GetComponent<TextMeshPro>();
        exhibitNeg1TMP = exhibit_neg1_GO.GetComponent<TextMeshPro>();
        exhibitSelectedTMP = exhibit_selected_GO.GetComponent<TextMeshPro>();
        exhibitPlus1TMP = exhibit_plus1_GO.GetComponent<TextMeshPro>();
        exhibitPlus2TMP = exhibit_plus2_GO.GetComponent<TextMeshPro>();

        exhibitNameTMP = exhibit_Name_GO.GetComponent<TextMeshPro>();
        exhibitDescTMP = exhibit_Desc_GO.GetComponent<TextMeshPro>();
        exhibitNavToTMP = exhibit_NavTo_GO.GetComponent<TextMeshPro>();
        
        bioContentTMP = BIO_Content.GetComponent<TextMeshPro>();
        bioArtistTMP = Bio_Artist.GetComponent<TextMeshPro>();

        navigatingToTMP = navPrompts.transform.GetChild(0).GetComponent<TextMeshPro>();

        currViewTMP = currViewGO.GetComponent<TextMeshPro>();
        currViewDescTMP = currViewDescGO.GetComponent<TextMeshPro>();
        nearestArtist = null;
        nearestArtistName = "";
        currViewDescTMP.SetText("");
        
        briefTMPs = new List<TextMeshPro>
        {
            artistBrief1TMP,
            artistBrief2TMP,
            artistBrief3TMP,
            artistBrief4TMP
        };

        displayPeriods = new List<string>();    // Initialize displayPeriods
        
        DeActivateAllHUDObjects();
        
        Invoke("InitializeHUD", 3f);
    }

    private void InitializeHUD()
    {
        ParseExhibitDoc();  // Read in Artist Document
        
        

        destExhibitNode = null;
        prevNearestNode = null;
        destNodePos = new Vector3();
        destExhibitName = "";
        
        
        DeActivateHUD();

        loadingPrompt.GetComponent<TextMeshPro>().text = "Museum Loaded.  Enjoy your Experience!";
        Invoke("EndLoadingPrompt", 2f);
        
        // Deactivate HUD After testing
        // ActivateHUD();
    }

    private void EndLoadingPrompt()
    {
        loadingPrompt.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown("t")) SwipeUp(1);
        if (Input.GetKeyDown("g")) SwipeDown(1);
        
        if (Input.GetKeyDown("j")) SwipeRight();
        if (Input.GetKeyDown("h")) SwipeLeft();

        if (navigatingToExhibit)
        {
            ActivateNavPrompts(destExhibitName);
            var nearestNode = Player.NearestNode;
            var playerPos = Player.transform.position;
            var distToDest = Mathf.Sqrt(Mathf.Pow(playerPos.x - destNodePos.x, 2f) +
                                        Mathf.Pow(playerPos.z - destNodePos.z, 2f));
            // reached destination node - end navigation
            if (nearestNode == destExhibitNode || distToDest <= 3)
                EndNavigation();
            else
            {
                // continue navigation
                if (prevNearestNode != nearestNode)
                    graphScript.DisplayPath(nearestNode, destExhibitNode);
                prevNearestNode = nearestNode;
            }
        }

        if (!hudActive)
        {
            currViewGO.SetActive(true);
            currViewDescGO.SetActive(true);
            RaycastHit[] artistPlanes = Physics.RaycastAll(transform.position, 
                transform.forward, 22f);

            float nearestHit = Mathf.Infinity;
            nearestArtist = null;
            foreach (var hit in artistPlanes)
            {
                if ((hit.distance < nearestHit) && (hit.collider.gameObject != nearestArtist) &&
                    hit.transform.gameObject.layer == LayerMask.NameToLayer("Artist"))
                {
                    nearestHit = hit.distance;
                    nearestArtist = hit.transform.gameObject;
                }
            }

            if (nearestArtist == null) nearestArtistName = "";
            else
            {
                if (nearestArtist.name != "Blank")
                    nearestArtistName = nearestArtist.name;
                
                var nearestArtistExhibit = nearestArtist.GetComponent<Exhibit>();
                if ( nearestArtistExhibit != null) 
                    currViewDescTMP.SetText(nearestArtistExhibit.ExhibitDesc);
                else 
                    currViewDescTMP.SetText("");
            }
            
            currViewTMP.SetText(nearestArtistName);
        }
        else
        {
            currViewGO.SetActive(false);
            currViewDescGO.SetActive(false);
        }
        
    }
    
    public void ActivateHUD()
    {
        DeActivateAllHUDObjects();
        HUDGO.SetActive(true);
        currentPane = LRCursor.Period;
        selectedPeriodIndex = displayPeriods.Count / 2;
        ActivatePeriod();
        hudActive = true;
    }
    
    public void DeActivateHUD()
    {
        currentPane = LRCursor.Period;
        selectedPeriodIndex = displayPeriods.Count / 2;
        DeActivateAllHUDObjects();
        hudActive = false;
        // HUDGO.SetActive(false);
    }

    private void DeActivateAllHUDObjects()
    {
        // Deactivate all Menus
        DeActivatePeriod();
        DeActivateArtist();
        DeActivateExhibitSelection();
        DeActivateSelectedExhibit();
        DeActivateBio();
        DeActivateNavPrompts();
    }

    private void ActivatePeriod()
    {
        period_GO.SetActive(true);
        UpdatePeriod(0);
    }

    private void DeActivatePeriod()
    {
        period_GO.SetActive(false);
    }

    private void ActivateArtist(bool leftSwipe)
    {
        artist_GO.SetActive(true);
        artist_briefs_GO.SetActive(true);
        displayArtists = periodDict[displayPeriods[selectedPeriodIndex]].getArtists;
        selectedArtistIndex =  !leftSwipe ? displayArtists.Count / 2 : selectedArtistIndex;
        UpdateArtist(0);
    }

    private void DeActivateArtist()
    {
        artist_GO.SetActive(false);
    }
    private void ActivateExhibitSelection(bool leftSwipe)
    {
        exhibit_GO.SetActive(true);
        displayExhibits = new List<Exhibit>(displayArtists[selectedArtistIndex].Exhibits);
        displayExhibits.Insert(0,null);
        selectedExhibitIndex = !leftSwipe ? displayExhibits.Count / 2 : selectedExhibitIndex;
        UpdateExhibit(0);
    }

    private void DeActivateExhibitSelection()
    {
        exhibit_GO.SetActive(false);
    }

    private void ActivateSelectedExhibit()
    {
        selected_exhibit_GO.SetActive(true);
        exhibitNameTMP.text = displayExhibits[selectedExhibitIndex].ExhibitName;
        exhibitDescTMP.text = displayExhibits[selectedExhibitIndex].ExhibitDesc;
        exhibitNavToTMP.text = NavToString + displayExhibits[selectedExhibitIndex].ExhibitName;
        
    }

    public void DeActivateSelectedExhibit()
    {
        selected_exhibit_GO.SetActive(false);
        exhibitNavToTMP.text = NavToString;
    }
    
    
    private void ActivateBio()
    {
        Bio_Pane.SetActive(true);
        bioArtistTMP.text = displayArtists[selectedArtistIndex].ArtistName;
        bioContentTMP.text = displayArtists[selectedArtistIndex].Bio;
    }

    private void DeActivateBio()
    {
        Bio_Pane.SetActive(false);
    }
    
    /// <summary>
    /// Updates the Period Menu in direction, dir
    /// </summary>
    /// <param name="dir">Integer direction to increment or decrement Period Menu</param>
    public void UpdatePeriod(int dir)
    {
        // Ensure index is not outside of the List
        selectedPeriodIndex += -dir;
        if (selectedPeriodIndex >= displayPeriods.Count) 
            selectedPeriodIndex = displayPeriods.Count - 1;
        if (selectedPeriodIndex < 0) 
            selectedPeriodIndex = 0;
        
        // Set selected period in the middle of the menu
        periodSelectedTMP.text = displayPeriods[selectedPeriodIndex];
        
        // Set +/- Menu choices based on size of List
        if (selectedPeriodIndex >= 1)
            periodNeg1TMP.text = displayPeriods[selectedPeriodIndex - 1];
        else
            periodNeg1TMP.text = "";
                
        if (selectedPeriodIndex >= 2)
            periodNeg2TMP.text = displayPeriods[selectedPeriodIndex - 2];
        else
            periodNeg2TMP.text = "";
        
        if (selectedPeriodIndex <= (displayPeriods.Count - 2))
            periodPlus1TMP.text = displayPeriods[selectedPeriodIndex + 1];
        else
            periodPlus1TMP.text = "";
        
        if (selectedPeriodIndex <= (displayPeriods.Count - 3))
            periodPlus2TMP.text = displayPeriods[selectedPeriodIndex + 2];
        else
            periodPlus2TMP.text = "";
        
    }
    /// <summary>
    /// Updates the Artist Menu in direction, dir
    /// </summary>
    /// <param name="dir">Integer direction to increment or decrement Period Menu</param>
    public void UpdateArtist(int dir)
    {
        // Ensure index is not outside of the List
        selectedArtistIndex += -dir;
        if (selectedArtistIndex >= displayArtists.Count) 
            selectedArtistIndex = displayArtists.Count - 1;
        if (selectedArtistIndex < 0) 
            selectedArtistIndex = 0;

        // Set selected artist in the middle of the menu
        artistSelectedTMP.text = displayArtists[selectedArtistIndex].ArtistName;
        
        // Build brief points and display
        int i = 0;
        while (i < briefTMPs.Count && i < displayArtists[selectedArtistIndex].Brief.Count)
        {
            briefTMPs[i].text = "-" + displayArtists[selectedArtistIndex].Brief[i];
            i++;
        }
        while (i < briefTMPs.Count)
        {
            briefTMPs[i].text = "";
            i++;
        }
        
        artist_plus1_GO.transform.localPosition = new Vector3(artistPlus1x, 
            artistPlus1Pos[displayArtists[selectedArtistIndex].Brief.Count], artistPlus1z);
        
        // Set +/- Menu choices based on size of List
        if (selectedArtistIndex >= 1)
            artistNeg1TMP.text = displayArtists[selectedArtistIndex - 1].ArtistName;
        else
            artistNeg1TMP.text = "";
                
        if (selectedArtistIndex >= 2)
            artistNeg2TMP.text = displayArtists[selectedArtistIndex - 2].ArtistName;
        else
            artistNeg2TMP.text = "";
        
        if (selectedArtistIndex + 1 < displayArtists.Count)
            artistPlus1TMP.text = displayArtists[selectedArtistIndex + 1].ArtistName;
        else
            artistPlus1TMP.text = "";

    }

    /// <summary>
    /// Updates the Exhibit Menu in direction, dir
    /// </summary>
    /// <param name="dir">Integer direction to increment or decrement Period Menu</param>
    private void UpdateExhibit(int dir)
    {
        // Ensure index is not outside of the List
        selectedExhibitIndex += -dir;
        if (selectedExhibitIndex >= displayExhibits.Count) 
            selectedExhibitIndex = displayExhibits.Count - 1;
        if (selectedExhibitIndex < 0) 
            selectedExhibitIndex = 0;

        // NullExhibitCheck is called for every menu assignment to determine if index is 
        //  on the biography
        // Set selected exhibit in the middle of the menu
        exhibitSelectedTMP.text = NullExhibitCheck(displayExhibits[selectedExhibitIndex]);
        
        // Set +/- Menu choices based on size of List
        if (selectedExhibitIndex >= 1)
            exhibitNeg1TMP.text = NullExhibitCheck(displayExhibits[selectedExhibitIndex - 1]);
        else
            exhibitNeg1TMP.text = "";
                
        if (selectedExhibitIndex >= 2)
            exhibitNeg2TMP.text = NullExhibitCheck(displayExhibits[selectedExhibitIndex - 2]);
        else
            exhibitNeg2TMP.text = "";
        
        if (selectedExhibitIndex <= (displayExhibits.Count - 2))
            exhibitPlus1TMP.text = NullExhibitCheck(displayExhibits[selectedExhibitIndex + 1]);
        else
            exhibitPlus1TMP.text = "";
        
        if (selectedExhibitIndex <= (displayExhibits.Count - 3))
            exhibitPlus2TMP.text = NullExhibitCheck(displayExhibits[selectedExhibitIndex + 2]);
        else
            exhibitPlus2TMP.text = "";
        
    }
    
    /// <summary>
    /// Determines if selected Exhibit, e is null
    /// </summary>
    /// <param name="e">Exhibit to be checked for null value</param>
    /// <returns>"Biography" if the exhibit is null, else the name of the exhibit</returns>
    private string NullExhibitCheck(Exhibit e)
    {
        if (e == null) return "Biography";
        return e.ExhibitName;
    }

    public void SwipeLeft()
    {
        switch (currentPane)
        {
            case LRCursor.Period:
                break;
            case LRCursor.Artist:
                currentPane = LRCursor.Period;
                DeActivateArtist();
                ActivatePeriod();
                break;
            case LRCursor.ExhibitSelection:
                DeActivateExhibitSelection();
                currentPane = LRCursor.Artist;
                ActivateArtist(true);
                break;
            case LRCursor.Bio:
                ActivateExhibitSelection(true);
                DeActivateBio();
                currentPane = LRCursor.ExhibitSelection;
                break;
            case LRCursor.Exhibit:
                ActivateExhibitSelection(true);
                DeActivateSelectedExhibit();
                currentPane = LRCursor.ExhibitSelection;
                break;
            default:
                break;
        }
    }

    public void SwipeRight()
    {
        switch (currentPane)
        {
            case LRCursor.Period:
                periodNeg2TMP.text = periodNeg1TMP.text = periodPlus1TMP.text = periodPlus2TMP.text = "";
                currentPane = LRCursor.Artist;
                ActivateArtist(false);
                break;
            case LRCursor.Artist:
                artistNeg2TMP.text = artistNeg1TMP.text = artistPlus1TMP.text = "";
                currentPane = LRCursor.ExhibitSelection;
                artist_briefs_GO.SetActive(false);
                ActivateExhibitSelection(false);
                break;
            case LRCursor.ExhibitSelection:
                if (NullExhibitCheck(displayExhibits[selectedExhibitIndex]).Equals("Biography"))
                {
                    currentPane = LRCursor.Bio;
                    ActivateBio();
                }
                else
                {
                    currentPane = LRCursor.Exhibit;
                    ActivateSelectedExhibit();
                }
                DeActivateExhibitSelection();
                
                break;
            case LRCursor.Bio:
                break;
            case LRCursor.Exhibit:
                NavigateToExhibit(displayExhibits[selectedExhibitIndex]);
                break;
            default:
                break;
        }
    }

    public void SwipeUp(int dir)
    {
        switch (currentPane)
        {
            case LRCursor.Period:
                UpdatePeriod(dir);
                break;
            case LRCursor.Artist:
                UpdateArtist(dir);
                break;
            case LRCursor.ExhibitSelection:
                UpdateExhibit(dir);
                break;
            case LRCursor.Bio:
                // need to add scroll for bio pane
                break;
            case LRCursor.Exhibit:
                break;
            default:
                break;
        }
    }

    public void SwipeDown(int dir)
    {
        dir *= -1;
        switch (currentPane)
        {
            case LRCursor.Period:
                UpdatePeriod(dir);
                break;
            case LRCursor.Artist:
                UpdateArtist(dir);
                break;
            case LRCursor.ExhibitSelection:
                UpdateExhibit(dir);
                break;
            case LRCursor.Bio:
                // need to add scroll for bio pane
                break;
            case LRCursor.Exhibit:
                break;
            default:
                break;
        }
    }

    private void NavigateToExhibit(Exhibit destNode)
    {
        navigatingToExhibit = true;
        destExhibitNode = destNode.NearestNode;
        destNodePos = destExhibitNode.transform.position;
        destExhibitName = destNode.ExhibitName;
        DeActivateHUD();
        prevNearestNode = null;

    }

    private void ActivateNavPrompts(String destNode)
    {
        navPrompts.SetActive(true);
        navigatingToTMP.SetText($"Navigating to {destNode}");
    }

    private void DeActivateNavPrompts()
    {
        navPrompts.SetActive(false);
    }
    
    

    public void EndNavigation()
    {
        DeActivateHUD();
        DeActivateNavPrompts();
        navigatingToExhibit = false;
        destExhibitNode = null;
        destExhibitName = "";
        graphScript.DeActivatePath();
    }
    

    
    /// <summary>
    /// Reads in Artist file and assigns artist data (brief and biography) to each artist.
    /// Artists are then added to their respective period.
    /// </summary>
    private void ParseExhibitDoc()
    {
        periodDict = new Dictionary<string, Period>();  // Initialize Dictionary to store Periods
        
        artistDict = new Dictionary<string, Artist>();  // Initialize Dictionary to store Artists
        // Iterate through exhibit collection and add Artist objects to artistDict
        foreach (Transform a in exhibitCollectionGO.transform)
        {
            artistDict.Add(a.GameObject().name, a.GameObject().GetComponent<Artist>());
        }
        
        // Initialize queues for parsing document
        Queue<Period> periodQ = new Queue<Period>();    // stack index 0
        // time                                         // stack index 1
        Queue<Artist> artistQ = new Queue<Artist>();    // stack index 2
        Queue<string> briefQ = new Queue<string>();     // stack index 3
        Queue<string> bioQ = new Queue<string>();       // stack index 4
        
        // Initialize indices 
        Artist currArtist = null;
        int stackIndex = -1;
        
        try
        {
            using (StreamReader sr = new StreamReader("Assets/Scripts/exhibit_data.txt"))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    switch (line)
                    {
                        case "<time>":
                            stackIndex = 1;
                            break;
                        case "<brief>":
                            stackIndex = 3;
                            break;
                        case "</brief>":
                            if (currArtist != null)
                                while(briefQ.TryDequeue(out var brief))
                                    currArtist.AddToBrief(brief);

                            stackIndex = -1;
                            break;
                        case "<bio>":
                            stackIndex = 4;
                            break;
                        case "</bio>":
                            if (currArtist != null)
                                while (bioQ.TryDequeue(out var bio))
                                    currArtist.AddToBio(bio);

                            stackIndex = -1;
                            break;
                            
                        case "<artist>":
                            stackIndex = 2;
                            break;
                        case "</artist>":
                            if (currArtist != null) artistQ.Enqueue(currArtist);
                            currArtist = null;
                            stackIndex = -1;
                            break;
                        case "</period>":
                            var p = periodQ.Dequeue();
                            while(artistQ.TryDequeue(out var a))
                                p.AddArtist(a);

                            periodDict.Add(p.PeriodName, p);
                            stackIndex = -1;
                            break;
                        case "</time>":
                            stackIndex = -1;
                            break;
                        
                        default:
                            switch (stackIndex)
                            {
                                case 1:
                                    periodQ.Enqueue(new Period(line));
                                    stackIndex = -1;
                                    break;
                                case 2:
                                    if (!artistDict.ContainsKey(line)) Debug.Log($"{line} Not in Scene");
                                    else currArtist = artistDict[line];
                                    stackIndex = -1;
                                    break;
                                case 3:
                                    briefQ.Enqueue(line);
                                    break;
                                case 4:
                                    bioQ.Enqueue(line);
                                    break;
                                default:
                                    break;
                            }
                            break;
                    }
                }
            }   // end of using StreamReader

            
        }
        catch (Exception e)
        {
            // Let the user know what went wrong.
            Debug.Log("The file could not be read:");
            Debug.Log(e.StackTrace);
        }
        foreach (var p in periodDict)
        {
            displayPeriods.Add(p.Key);
        }
        // sort Periods, then remove "Pre-War" from end and place at front
        displayPeriods.Sort();
        displayPeriods.Insert(0,displayPeriods[^1]);
        displayPeriods.RemoveAt(displayPeriods.Count - 1);
    }   // end of ParseExhibitDoc()
}
