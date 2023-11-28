using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateBallPlacement : MonoBehaviour
{
    private List<Vector3> shotPlacementThreeD = new List<Vector3>();

    public LayerMask groundLayer;

    public float maxRayLength = 3; //dont know if needed, but here it is i guess

    public GameObject golfBall; //get data from the golfBall

    private Vector3 ballHeight; //used to store the height of the golfBall

    private float yMaxDistance = Mathf.Infinity; //used for RayCast

    private bool findingBallPosY = false; 

    public GolfShots golfShots; //call this script something else

    // Start is called before the first frame update
    void Start()
    {
        ballHeight = new Vector3(0, golfBall.transform.localScale.y / 2, 0);
        CalculateShotHeight(golfBall.transform.position.x, golfBall.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // used by external script, not found in miniproject
    public void PositionsToThreeD(float x, float y)
    {
        //float z = CalculateShotHeight(x, y); //move it around to calculate y-axis

        //shotPlacementThreeD.Add(new Vector3(x, y, z));
    }

    public void CalculateShotHeight(float x, float z) //this needs to accept the 2D-coordinates as argument
    {
        // create a sphere gameobject, or just adjust the shot gameobject. probably the latter
        findingBallPosY = true;

        while (findingBallPosY)
        {
            RaycastHit hit; //could be moved to Start()

            if (Physics.Raycast(golfBall.transform.position, Vector3.down, out hit, yMaxDistance, groundLayer))
            {
                //places the ball directly on top of the ground
                golfBall.transform.position = hit.point + ballHeight;
                Debug.Log("Moving ball down!");
                findingBallPosY = false;
            }
            else if (Physics.Raycast(golfBall.transform.position, Vector3.up, out hit, yMaxDistance, groundLayer))
            {
                //moves the golfBall up, if there is ground above the ball
                golfBall.transform.position = hit.point + ballHeight * 5; //this should be change when golf course is more on the finished side
                Debug.Log("Moving ball up!");
            }
            Debug.Log($"golfBall's distance to groundLayer: {hit.distance}");
            Debug.Log($"golfBall's heightPoint in groundLayer: {hit.point}");
        }
    }

    // again, used by external script, not found in miniproject
    public void WriteThreeDListToFile()
    {
        //call read/write script to write the shotPlacementThreeD-list to the selected players text file
        //golfShots.writeShotPositionsToFile(?shotPlacementThreeD?)
    }

}
