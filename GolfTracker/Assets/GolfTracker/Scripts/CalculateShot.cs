using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CalculateShot : MonoBehaviour
{
    private float heldDownTime = 0.0f;
    private bool isHeldDown = false;
    private float maxTime;
    private float minTime;
    private float maxShotLength;
    private float minShotLength;
    public float currentShotLength;
    public float[] club;

    public Slider shotTimeSlider;
    //private float perfectTime = 2.2f;
    public TMP_Text guiShotLength;
    public TMP_Text guiMaxShotLength;
    public Gradient badShotGradient;
    public Color goodShotColor;
    public Image sliderFillArea;

    public TMP_Dropdown clubList;

    public ttteeessstttiiinnnggg testing;

    // Start is called before the first frame update
    void Start()
    {
        fillBag();
        guiShotLength.text = "";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isHeldDown = true;
            shotTimeSlider.value = 0;
            club = listOfClubs[clubList.value];

            maxShotLength = club[0]; //wrong way around?? but it works anyway, so don't panic
            minShotLength = club[1];

            maxTime = 2.2f;
            minTime = (maxShotLength / minShotLength) * maxTime;

            StartCoroutine(KeyDownTime());
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isHeldDown = false;
            StopCoroutine(KeyDownTime());
            Debug.Log(heldDownTime);

            testing.ShootBall(currentShotLength, club[2]);

            heldDownTime = 0;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            testing.ResetBall();
        }
        // display the maximum length achieved by the selected golf club
        guiMaxShotLength.text = $"max: {listOfClubs[clubList.value][1].ToString("F2")}";
    }

    private IEnumerator KeyDownTime()
    {
        while (isHeldDown)
        {
            heldDownTime += 0.025f;
            shotTimeSlider.value = heldDownTime;
            currentShotLength = minShotLength * (heldDownTime / maxTime);
            guiShotLength.text = "~" + currentShotLength.ToString("F1") + "m";

            SetTimeSliderColor();

            yield return new WaitForSeconds(.025f); // This determines how smooth the slider moves
            //KeyDownTime();
        }
    }

    private void SetTimeSliderColor() // also make a gradient for the background fill to make shots easier :) - nope
    {
        // gradient on the slider goes from red to green, depending on how good the shot is going to be (how far its gonna go)
        if (heldDownTime > minTime && heldDownTime < maxTime)
        {
            sliderFillArea.color = goodShotColor;
        }
        else if (minTime > heldDownTime)
        {
            sliderFillArea.color = badShotGradient.Evaluate(currentShotLength / minShotLength);
        }
        else if (maxTime < heldDownTime)
        {
            sliderFillArea.color = badShotGradient.Evaluate(maxShotLength / currentShotLength);
        }
    }

    // stats for all the golf clubs that can be selected. First two numbers are the "perfect shot range". last number is angle of approach
    private List<float[]> listOfClubs = new List<float[]>();
    private void fillBag()
    {
        // Adding floats to arrays
        float[] Driver1 = { 175, 225, 10.9f };
        float[] Driver2 = { 150, 200, 11.4f };
        float[] Hybrid1 = { 125, 175, 10.2f };
        float[] Hybrid2 = { 100, 150, 10.7f };
        float[] Wood1 = { 140, 180, 9.0f };
        float[] Wood2 = { 135, 175, 9.1f };
        float[] Wood3 = { 130, 170, 9.2f };
        float[] Wood4 = { 125, 165, 9.4f };
        float[] Wood5 = { 120, 160, 9.6f };
        float[] Wood6 = { 115, 155, 9.9f };
        float[] Wood7 = { 110, 150, 10.4f };
        float[] Wood8 = { 105, 145, 11.2f };
        float[] Wood9 = { 100, 140, 12.8f };
        float[] Iron1 = { 120, 160, 9.7f };
        float[] Iron2 = { 115, 155, 9.9f };
        float[] Iron3 = { 110, 150, 10.4f };
        float[] Iron4 = { 105, 145, 11.0f };
        float[] Iron5 = { 100, 140, 12.1f };
        float[] Iron6 = { 95, 135, 14.1f };
        float[] Iron7 = { 90, 130, 16.3f };
        float[] Iron8 = { 85, 125, 18.2f };
        float[] Iron9 = { 80, 120, 20.4f };
        float[] PitchWedge = { 30, 90, 29.2f };
        float[] SandWedge = { 15, 30, 47.5f };
        float[] LobWedge = { 10, 20, 66.6f };
        float[] Putter = { 0.01f, 20, 1f }; //if angle is 0, ball won't shoot because division by 0

        // Adding arrays to list
        listOfClubs.Add(Driver1);
        listOfClubs.Add(Driver2);
        listOfClubs.Add(Hybrid1);
        listOfClubs.Add(Hybrid2);
        listOfClubs.Add(Wood1);
        listOfClubs.Add(Wood2);
        listOfClubs.Add(Wood3);
        listOfClubs.Add(Wood4);
        listOfClubs.Add(Wood5);
        listOfClubs.Add(Wood6);
        listOfClubs.Add(Wood7);
        listOfClubs.Add(Wood8);
        listOfClubs.Add(Wood9);
        listOfClubs.Add(Iron1);
        listOfClubs.Add(Iron2);
        listOfClubs.Add(Iron3);
        listOfClubs.Add(Iron4);
        listOfClubs.Add(Iron5);
        listOfClubs.Add(Iron6);
        listOfClubs.Add(Iron7);
        listOfClubs.Add(Iron8);
        listOfClubs.Add(Iron9);
        listOfClubs.Add(PitchWedge);
        listOfClubs.Add(SandWedge);
        listOfClubs.Add(LobWedge);
        listOfClubs.Add(Putter);
    }
}

