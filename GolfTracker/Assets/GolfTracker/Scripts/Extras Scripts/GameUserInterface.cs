using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameUserInterface : MonoBehaviour
{
    //info about course/hole
    private int holeNumber;
    private string weatherConditions;
    private float distanceToFlag;

    //info about current player
    private List<string> playerList = new List<string>();
    private string currentPlayer;
    private float currentPlayerHCP;
    private int currentPlayerShots;
    private string selectedClub;
    private float currentShotLength;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GetDistanceToFlag() { } //from current ball

    void GetCurrentPlayer() { } //name, HCP, shotAmount

    void NextPlayer() 
    {

    }

    //PAUSE MENU
    private int MasterVolume;
    private int VolumeSoundFX;
    private int VolumeMusic;
    private bool SkipWalking;
    private bool SkipNPCShots;

    public void ChangeMasterVolume(Slider slider)
    {
        MasterVolume = (int) slider.value;
        Debug.Log($"Changing Overall Volume: {MasterVolume}");
    }
    public void ChangeVolumeSoundFX(Slider slider)
    {
        VolumeSoundFX = (int) slider.value; //* MasterVolume
        Debug.Log($"Changing Sound FX: {VolumeSoundFX}");
    }
    public void ChangeVolumeMusic(Slider slider)
    {
        VolumeMusic = (int) slider.value; //* MasterVolume
        Debug.Log($"Changing Music Volume: {VolumeMusic}");
    }
    public void ToggleSkipWalking(Toggle toggle)
    {
        SkipWalking = toggle.isOn;
        Debug.Log($"Toggling Skip Walking: {SkipWalking}");
    }
    public void ToggleSkipNPCShots(Toggle toggle)
    {
        SkipNPCShots = toggle.isOn;
        Debug.Log($"Toggling Skip NPC Shots: {SkipNPCShots}");
    }
    public void Resume()
    {
        //Resume (unfreeze gameplay)
    }
    public void QuitToMainMenu()
    {
        //Quit to main menu (store what needs to be stored, if anything)
    }
}
