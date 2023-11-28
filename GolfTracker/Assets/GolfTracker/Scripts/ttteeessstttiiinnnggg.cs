using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ttteeessstttiiinnnggg : MonoBehaviour
{
    public Rigidbody ballRB;
    public LayerMask groundLayer;

    private float kmhSpeed;
    private float degAngle;

    private Vector3 trajectory;

    private bool isFlying = true;
    private bool isPutting = false;
    private bool checkPutLength = false;
    private Vector3 shotStart;
    private float distanceTravelled;
    private float distanceToTravel;

    public CalculateShot calculateShot; // not used here?
    public PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (checkPutLength) // since a shot from the putter never flies. the "FreezeBall()" function is never called, and the ball would just keep going
        {
            distanceTravelled = Mathf.Sqrt(Mathf.Pow(ballRB.position.x - shotStart.x, 2) + Mathf.Pow(ballRB.position.z - shotStart.z, 2));
            Debug.Log($"Distance Travelled: {distanceTravelled} m");

            if (distanceTravelled >= distanceToTravel)
            {
                isPutting = false;
                checkPutLength = false;
                FreezeBall();
            }
        }
    }

    public void ShootBall(float distance, float angle)
    {
        isFlying = true;
        ballRB.useGravity = true;
        if (angle <= 1) { isPutting = true; }
        else { isPutting = false; }

        distanceToTravel = distance; // to use this value elsewhere

        float radAngle = angle * (Mathf.PI / 180f); // degrees to radians

        kmhSpeed = Mathf.Sqrt((distance * 9.8f) / Mathf.Sin(2 * radAngle)); // shot calculation is true to the real world, except no bounces. (~95-98% accurate)

        // determining the trajectory of the golf ball
        float velocityX = kmhSpeed * Mathf.Cos(radAngle);
        float velocityY = kmhSpeed * Mathf.Sin(radAngle);

        if (!isPutting) // here we get a curvy trajectory line
        {
            trajectory = new Vector3(0, velocityY, velocityX);
        }
        else // here we get a straight trajectory line
        {
            trajectory = new Vector3(0, 0, velocityX);
            shotStart = ballRB.position;
            checkPutLength = true;
        }
        
        StartCoroutine(AnimateShot(angle, trajectory));
    }

    // animate the swing of the shot and the shot of the ball
    private IEnumerator AnimateShot(float angle, Vector3 trajectory)
    {
        // the time it takes to pull the selected golf club back
        float waitTime1;
        // the time it takes to swing the golf club towards teh golf ball
        float waitTime2;
        if (angle <= 1.0f) { waitTime1 = 2.4f; waitTime2 = 0.4f; }
        else if (angle <= 11.0f) { waitTime1 = 2.1f; waitTime2 = 0.9f; }
        else { waitTime1 = 2.4f; waitTime2 = 0.4f; }

        playerMovement.AlignPlayer("idleToShoot");

        yield return new WaitForSeconds(1.01f);

        playerMovement.ShootAnimation(angle);

        yield return new WaitForSeconds(waitTime1);

        // shooting the ball with ForceMode.Impulse. a lot of velocity is added once!
        ballRB.AddRelativeForce(trajectory, ForceMode.Impulse);

        yield return new WaitForSeconds(waitTime2);

        playerMovement.AlignPlayer("shootToIdle");

        yield return new WaitForSeconds(1f);

        playerMovement.RotatePlayer();
    }

    private void RollBall() //just for testing purposes
    {
        Debug.Log("rolling...");

        Vector3 rollVector = new Vector3(5f, 0f, 5f);

        ballRB.AddForce(rollVector, ForceMode.Impulse);
    }

    // when the ball has been shot, find the point a which it hit the ground and freeze it there
    public void FreezeBall()
    {
        isFlying = false;
        ballRB.useGravity = false;

        ballRB.velocity = Vector3.zero;
        ballRB.angularVelocity = Vector3.zero;
        ballRB.transform.rotation = Quaternion.identity;

        ballRB.transform.position += new Vector3(0, 10, 0);

        RaycastHit hit;

        // find hit point on the ground
        Physics.Raycast(ballRB.transform.position, Vector3.down, out hit, Mathf.Infinity, groundLayer);
        Debug.Log("hitpoint" + hit.point);

        // place the ball on top of the ground, rather than halfway into the ground
        ballRB.transform.position = hit.point + new Vector3(0, ballRB.transform.localScale.y/2, 0);
        Debug.Log("ball position" + ballRB.transform.position);

        //ballRB.transform.position = new Vector3(0, .2f, 0);
    }

    public void ResetBall() // again for test purposes. press r to get a new ball
    {
        isFlying = true;
        ballRB.useGravity = true;
        //isPutting = false;

        ballRB.transform.position = new Vector3(0f, 2.5f, 0f);
    }

    public void OnCollisionEnter(Collision other) // check ground collision
    {
        Debug.Log("Freeze! This is a stickup");

        if (other.gameObject.CompareTag("Respawn")/* && !isPutting*/) //rename tag to ground om something (grass, rough, green, fairway)
        {
            FreezeBall();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        // checks if the golf ball touches the green disc in front of the player
        if (other.gameObject.CompareTag("BigBoost")) //"BigBoost" = bad name, but I have too many tags in Unity
        {
            Debug.Log("Entering shootmode...");
            playerMovement.EnterShootMode();
        }
        if (other.gameObject.CompareTag("Finish")) //"Finish" = hole under the flag
        {
            Debug.Log("Going to next hole");
            playerMovement.NextHole();
        }
    }
}
