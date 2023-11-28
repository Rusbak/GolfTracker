using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public Camera firstPersonCamera; //change to cinemachine?
    public Camera thirdPersonCamera; //change to cinemachine?
    private bool isFirstPerson;
    public GameObject crosshair;
    public GameObject ballDetector;
    public GameObject golfBall;
    public Animation playerAnimation;
    public GameObject person;

    private int moveSpeed = 10; // Adjust this value to control the movement speed.
    private float horizontalInput;
    private float verticalInput;
    private int rotationSpeed = 1000; // Adjust this value to control the camera speed.
    private float mouseX;
    private float mouseY;
    private bool isShooting = false;
    private int holeNumber = 1; // use/replace from GUI
    public TMP_Text holeNumberText;
    private int shotsTaken = 1; // use/replace from GUI
    public TMP_Text shotsTakenText;
    private float distanceToHole;
    public TMP_Text distanceToHoleText;
    public GameObject[] holeList;
    public GameObject[] flagList;

    // Start is called before the first frame update
    void Start()
    {
        firstPersonCamera.transform.position = person.transform.position + new Vector3(0f, 1.5f, 0.5f); //new Vector3(0.25f, 0.75f, 0.1f)
        //firstPersonCamera.transform.eulerAngles = person.transform.eulerAngles + new Vector3(63, 0, 0); //if you wanna see the guys pants
        firstPersonCamera.transform.eulerAngles = person.transform.eulerAngles + new Vector3(15, 0, 0);

        thirdPersonCamera.transform.position = person.transform.position + new Vector3(2, 6-2.6f, -7.5f);
        thirdPersonCamera.transform.eulerAngles = person.transform.eulerAngles + new Vector3(15, 0, 0);

        SwitchCameraView(3);

        // set all holes inactive
        for (int i = 0; i < 18; i++)
        {
            holeList[i].gameObject.SetActive(false);
        }

        // set first hole active
        holeList[0].SetActive(true);

        // update "GUI"
        holeNumberText.text = $"Hole: {holeNumber}";
        shotsTakenText.text = $"Shot: {shotsTaken}";
        distanceToHole = Vector3.Distance(golfBall.transform.position, flagList[0].transform.position);
        distanceToHoleText.text = $"Distance to hole: {distanceToHole.ToString("F2")} m";
    }

    // Update is called once per frame
    void Update()
    {
        // display how to the current hole
        distanceToHole = Vector3.Distance(golfBall.transform.position, flagList[holeNumber - 1].transform.position);
        distanceToHoleText.text = $"Distance to hole: {distanceToHole.ToString("F2")} m";

        // change cameraView on mouse scroll
        if (Input.mouseScrollDelta.y > 0) { SwitchCameraView(1); }
        else if (Input.mouseScrollDelta.y < 0) { SwitchCameraView(3); }

        if (!isShooting)
        {
            // Player Movement
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");

            // I f'ed up animations so have to do this workaround... okai maybe not :O
            float movementX = horizontalInput;
            float movementZ = verticalInput;

            //if (shotsTaken % 4 == 0)
            //{
            //    movementX = horizontalInput;
            //    movementZ = verticalInput;
            //}
            //else if (shotsTaken % 4 == 1)
            //{
            //    movementX = -verticalInput;
            //    movementZ = horizontalInput;
            //}
            //else if (shotsTaken % 4 == 2)
            //{
            //    movementX = -horizontalInput;
            //    movementZ = -verticalInput;
            //}
            //else if (shotsTaken % 4 == 3)
            //{
            //    movementX = verticalInput;
            //    movementZ = -horizontalInput;
            //}

            Vector3 movement = new Vector3(movementX, 0, movementZ) * moveSpeed * Time.deltaTime;
            transform.Translate(movement);

            // CameraMovement
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");
            transform.RotateAround(golfBall.transform.position, Vector3.up, mouseX * rotationSpeed * Time.deltaTime);
            golfBall.transform.Rotate(Vector3.up, mouseX * rotationSpeed * Time.deltaTime);

            // play walk animation if player is moving and not shooting (look above)
            if (horizontalInput != 0 || verticalInput != 0) 
            {
                // walking animation
                playerAnimation.Play("Walk");
            }
            //else
            //{
            //    //idle animation is bugged
            //    playerAnimation.Play("idle");
            //}

            // move camera up and down when in first person
            if (isFirstPerson)
            {
                firstPersonCamera.transform.Rotate(Vector3.left, mouseY * rotationSpeed * Time.deltaTime);
            }
        }
        else if (isShooting) // can only aim when in shoot mode
        {
            // CameraMovement
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");
            transform.RotateAround(golfBall.transform.position, Vector3.up, mouseX * rotationSpeed * Time.deltaTime);
            golfBall.transform.Rotate(Vector3.up, mouseX * rotationSpeed * Time.deltaTime);
        }
    }

    public void SwitchCameraView(int POV)
    {
        if (POV == 1)
        {
            Debug.Log("Switching to first person");
            isFirstPerson = true;
            firstPersonCamera.enabled = true;
            thirdPersonCamera.enabled = false;
            //crosshair.SetActive(true);
        }
        else if (POV == 3)
        {
            Debug.Log("Switching to third person");
            isFirstPerson = false;
            firstPersonCamera.enabled = false;
            thirdPersonCamera.enabled = true;
            //crosshair.SetActive(false); //maybe just always active
        }
    }

    public void ShootAnimation(float usedClubAngle)
    {
        if (usedClubAngle <= 1.0f)
        {
            // putting animation
            playerAnimation.Play("Putting");
        }
        else if (usedClubAngle <= 11.0f)
        {
            // driving animation
            playerAnimation.Play("Driving");
        }
        else
        {
            // chipping animation
            playerAnimation.Play("Chipping");
        }
    }

    // player get ready to shoot or reverts to idle after shooting
    public void AlignPlayer(string state)
    {
        if (state == "idleToShoot")
        {
            playerAnimation.Play("idleToShoot");
        }
        else if (state == "shootToIdle")
        {
            playerAnimation.Play("shootToIdle");
        }
    }

    // after shooting rotate the player to look towards the direction of the ball
    public void RotatePlayer()
    {
        Vector3 currentCameraPos = thirdPersonCamera.transform.position;
        person.transform.eulerAngles += new Vector3(0, -90, 0);
        thirdPersonCamera.transform.position = currentCameraPos;
        thirdPersonCamera.transform.eulerAngles += new Vector3(0, 90, 0);

        isShooting = false;
        ballDetector.SetActive(true);

        shotsTaken++;
        shotsTakenText.text = $"Shot: {shotsTaken}";
    }

    // rotate and place the player next to the ball so it is ready to shoot
    public void EnterShootMode()
    {
        if (playerAnimation.isPlaying)
        {
            playerAnimation.Play("walkToIdle");
        }

        isShooting = true;
        ballDetector.SetActive(false);
        Vector3 newPosition = new Vector3(0.3f, -0.1f, -0.1f); 

        transform.position = golfBall.transform.position + newPosition;
        golfBall.transform.eulerAngles = transform.eulerAngles;

        Vector3 currentCameraPos = thirdPersonCamera.transform.position;
        person.transform.eulerAngles += new Vector3(0, 90, 0);
        thirdPersonCamera.transform.position = currentCameraPos;
        thirdPersonCamera.transform.eulerAngles += new Vector3(0, -90, 0);
    }

    public void NextHole() // go to the next hole, unless alreaydy at the last hole
    {
        if (holeNumber != 18)
        {
            holeList[holeNumber - 1].SetActive(false);
            holeList[holeNumber].SetActive(true);

            holeNumber++;
            holeNumberText.text = $"Hole: {holeNumber}";
            shotsTaken = 0;

            golfBall.transform.position = new Vector3(0, 0.1f, 0);
            transform.position = new Vector3(0, 0, -4);
            transform.eulerAngles = Vector3.zero;
        }
    }
}
