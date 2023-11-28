using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; //move to central script

public class RoundLayout : MonoBehaviour
{
    private bool isMappingPoints = true;
    //private bool isMovingFlag = false;
    //private bool isMovingTee = false;

    public List<GameObject> holeList = new List<GameObject>();
    private int currentHoleIndex = 0; // use the hole number instead

    public LayerMask groundLayer;
    public GameObject shotMarker; //shotMarker prefab

    private List<Vector3> shotListMap = new List<Vector3>();
    private List<GameObject> shotMarkerList = new List<GameObject>(); //could maybe combine these
    private List<List<Vector3>> shotListRound = new List<List<Vector3>>();
    private List<Vector4> markerColorList = new List<Vector4>();
    private int markerColorIndex = 0;

    //GUI elements
    public TMP_Text guiName;
    public TMP_Text guiHCP;
    public TMP_Text guiShots;
    public TMP_Dropdown guiClub;
    public TMP_Text guiHoleNumber;
    public Transform usedClubsView;
    //other parameters like weather and that


    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject hole in holeList)
        { hole.SetActive(false); }
        holeList[0].SetActive(true);

        guiName.text = ""; //find the player name
        guiHCP.text = "HCP: ";
        guiShots.text = "Shots: 0";
        guiHoleNumber.text = "Hole: 1";

        markerColorList.Add(new Vector4 ( 230 / 255f, 25 / 255f, 75 / 255f ));  // red
        markerColorList.Add(new Vector4 ( 245 / 255f, 130 / 255f, 48 / 255f )); // orange
        markerColorList.Add(new Vector4 ( 255 / 255f, 255 / 255f, 25 / 255f )); // yellow
        markerColorList.Add(new Vector4 ( 145 / 255f, 30 / 255f, 180 / 255f )); // purple
        markerColorList.Add(new Vector4 ( 240 / 255f, 50 / 255f, 230 / 255f )); // magenta
    }

    // Update is called once per frame
    void Update()
    {
        if (isMappingPoints) // also make possible to move and zoom on image
        {
            if (Input.GetKeyDown(KeyCode.Mouse0)) //Primary/left mouse button
            {
                // Places a shotMarker prefab at the mouse position
                Vector2 mousePos = Input.mousePosition;
                PlaceShotMarker(mousePos);

                guiShots.text = $"Shots: {shotListMap.Count}";
            }
            if (Input.GetKey(KeyCode.LeftControl) | Input.GetKey(KeyCode.RightControl))
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    // Remove latest put shot
                    Debug.Log("delete!");
                    RemoveLatestShotMarkerPos();

                    guiShots.text = $"Shots: {shotListMap.Count}";
                }
            }
        }
    }

    public void PlaceShotMarker(Vector2 mousePos)
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        RaycastHit hit = new RaycastHit();

        //checks the point clicked on the screen
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            SpawnShotMarker(hit.point);
        }
    }

    public void SpawnShotMarker(Vector3 placement) //put the used club in the scroll view also (use dropdown instead if possible)
    {
        GameObject newShotMarker = Instantiate(shotMarker, placement/* + ballHeight*/, Quaternion.identity);

        shotMarkerList.Add(newShotMarker);
        shotListMap.Add(newShotMarker.transform.position);

        newShotMarker.GetComponent<Renderer>().material.SetColor("_Color", markerColorList[markerColorIndex]);
        if (markerColorIndex < (markerColorList.Count - 1))
        { markerColorIndex++; }
        else
        { markerColorIndex = 0; }

        Debug.Log($"shot {shotListMap.Count}:{newShotMarker.transform.position}");

        AddUsedClub();
    }

    public void RemoveLatestShotMarkerPos() //called through some buttom press
    {
        //removes the latest entry of the list
        Destroy(shotMarkerList[shotMarkerList.Count - 1]); //removes the GameObject itself
        shotMarkerList.RemoveAt(shotMarkerList.Count - 1); //removes the GameObject from the list
        shotListMap.RemoveAt(shotListMap.Count - 1); //removes the position of the GameObject

        markerColorIndex--; //dont need this, but sure

        RemoveUsedClub();
    }

    public TMP_Text textPrefab;
    public void AddUsedClub()
    {
        TMP_Text usedClub = Instantiate(textPrefab, usedClubsView);
        usedClub.text = $"{shotListMap.Count}: {guiClub.options[guiClub.value].text}";
    }
    public void RemoveUsedClub()
    {
        // Check if there are any children in the contentContainer
        if (usedClubsView.childCount > 0)
        {
            // Get the last child and destroy it
            Destroy(usedClubsView.GetChild(usedClubsView.childCount - 1).gameObject);
        }
        else
        {
            Debug.LogWarning("No items to remove.");
        }
    }

    //public void MoveFlag()
    //{
    //    isMappingPoints = false;
    //    isMovingFlag = true;
    //    isMovingTee = false;
    //}
    //public void MoveTee() 
    //{
    //    isMappingPoints = false;
    //    isMovingFlag = false;
    //    isMovingTee = true;
    //}

    public void NextHole()
    {
        //have a counter for each hole marked, to know how many times to print "DNF"
        //print positions from list to textfile
        Debug.Log($"That only took {shotListMap.Count} shots. Way to go!!");

        shotListRound.Add(shotListMap);

        holeList[currentHoleIndex].SetActive(false);
        currentHoleIndex++;
        holeList[currentHoleIndex].SetActive(true);

        foreach (GameObject shotMarker in shotMarkerList)
        {
            Destroy(shotMarker);
        }
        shotListMap.Clear();

        guiHoleNumber.text = $"Hole: {currentHoleIndex + 1}";
        guiShots.text = $"Shots: {shotListMap.Count}";
    }

    public void EndRound()
    {
        isMappingPoints = false;
        Debug.Log("Good Game!");

        Debug.Log($"number of holes played: {shotListRound.Count}"); //store in text file

        SceneManager.LoadScene("GolfTracker");
    }

    //public void adddimensiontopostwod() // move to calculateballplacement script
    //{
    //    for (int i = 0; i <= shotplacementTwod.count; i++)
    //    {
    //        float x = shotPlacementTwoD[i].x;
    //        float y = shotPlacementTwoD[i].y;

    //        calculateBallPlacement.positionsToThreeD(x, y);
    //    }

    //    calculateBallPlacement.writeThreeDListToFile();
    //}
}
