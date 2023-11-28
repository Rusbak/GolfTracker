using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolfShots : MonoBehaviour
{
    //calculate distance between 1-2, 2-3, etc. DONE
    //make list of parameters DONE?
    //Distance, direction(aim), gravity, wind, club(angle of shot), good/bad shot, spin, (forest gnomes --> add waypoint)
    //set good/bad range for each club

    //Probably makes this into one big dictionary
    //Woods
    private List<int> driver, hybrid, oneWood, twoWood, threeWood, fourWood, fiveWood, sixWood, sevenWood, eightWood, nineWood = new List<int>();
    //Irons
    private List<int> oneIron, twoIron, threeIron, fourIron, fiveIron, sixIron, sevenIron, eightIron, nineIron = new List<int>();
    //Diverse
    private List<int> pitchWedge, sandWedge, lobWedge, putter = new List<int>();

    public List<GameObject> shotList;
    public GameObject golfBall;
    private float shotLength;

    // Start is called before the first frame update
    void Start()
    {
        //golfBall.transform.position = shotList[0].transform.position;
        //move to a person prefab
        //AddClubRangesToList();
        //CalculateShotLenght(0);
        CalculateShot(0);
    }

    private void AddClubRangesToList() //obsolete
    {
        driver.AddRange(new int[] { 150, 200 });
        hybrid.AddRange(new int[] { 125, 175 });

        //no clue about these. I don't use them personally
        oneWood.AddRange(new int[] { 140, 180 });
        twoWood.AddRange(new int[] { 135, 175 });
        threeWood.AddRange(new int[] { 130, 170 });
        fourWood.AddRange(new int[] { 125, 165 });
        fiveWood.AddRange(new int[] { 120, 160 });
        sixWood.AddRange(new int[] { 115, 155 });
        sevenWood.AddRange(new int[] { 110, 150 });
        eightWood.AddRange(new int[] { 105, 145 });
        nineWood.AddRange(new int[] { 95, 135 });

        oneIron.AddRange(new int[] { 120, 160 });
        twoIron.AddRange(new int[] { 115, 155 });
        threeIron.AddRange(new int[] { 110, 150 });
        fourIron.AddRange(new int[] { 105, 145 });
        fiveIron.AddRange(new int[] { 100, 140 });
        sixIron.AddRange(new int[] { 95, 135 });
        sevenIron.AddRange(new int[] { 90, 130 });
        eightIron.AddRange(new int[] { 85, 125 });
        nineIron.AddRange(new int[] { 80, 120 });

        pitchWedge.AddRange(new int[] { 15, 80 });
        sandWedge.AddRange(new int[] { 2, 15 });
        lobWedge.AddRange(new int[] { 2, 5 });
        putter.AddRange(new int[] { 0, 999 });
    }

    private void CalculateShotLenght(int shotNumber) //use when balls are stationary
    {
        shotLength = Vector3.Distance(golfBall.transform.position, shotList[shotNumber + 1].transform.position); //just use the vector coord instead
    }

    //Put on script on golf ball//////////////////////////////////////
    public Vector3 initialVelocity;
    private float gravity = Physics.gravity.y;
    private float timeToReachTarget = .4f;
    private float stopThreshold = 0.01f;
    private bool isSimulationComplete = false; //should be true, when not testing

    private void CalculateShot(int shotNumber)
    {
        initialVelocity = (shotList[shotNumber + 1].transform.position - shotList[shotNumber].transform.position) / timeToReachTarget;
        initialVelocity.y -= gravity * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSimulationComplete)
        {
            Vector3 newPosition = golfBall.transform.position + initialVelocity * Time.deltaTime;
            golfBall.transform.position = newPosition;

            CalculateShotLenght(0); //change dynamically
            Debug.Log(shotLength);
        }
        if (shotLength < stopThreshold) //GolfShots.shotLength
        {
            initialVelocity = Vector3.zero;
            isSimulationComplete = true;
        }
    }
}