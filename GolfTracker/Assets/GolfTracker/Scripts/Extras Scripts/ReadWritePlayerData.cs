using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ReadWritePlayerData : MonoBehaviour
{
    private string filePath;

    public List<string> playerList = new List<string>();
    
    //Course parameters
    private List<string> courseParameters = new List<string> { "date", "dayNight", "rainySunny", "wetDry", "wind" };
    private List<Vector3> teePositions = new List<Vector3>();
    private List<Vector3> flagPositions = new List<Vector3>();

    void Start()
    {
        filePath = Application.dataPath + "/GolfTracker/UserData/PlayerData.txt";

        if (File.Exists(filePath))
        {
            // Reads all lines from PlayerText file
            string[] lines = File.ReadAllLines(filePath);

            // Process each line (each line should represent a player's data)
            foreach (string line in lines)
            {
                // Split the line into parts (assuming a simple format, e.g., "Name,Handicap")
                string[] parts = line.Split(',');

                if (parts.Length >= 2)
                {
                    string playerName = parts[0];
                    float playerHandicap;

                    if (parts[1] != null)
                    {
                        if (!float.TryParse(parts[1], out playerHandicap))
                        {
                            parts[1] = "54";
                        }
                        float.TryParse(parts[1], out playerHandicap);


                        Debug.Log($"Player: {playerName}, Handicap: {playerHandicap}");
                    }
                    else
                    {
                        Debug.LogError("No Players in Database.");
                    }
                }

                playerList.Add(line);
                Debug.Log("Players in database: " + playerList.Count);
            }
        }
        else
        {
            Debug.LogError("File does not exist.");
        }
    }

    public string ReadPlayerData(int player) //move stuff from Start() to here
    {
        return playerList[player]; //should find information from personal textfile instead
    }

    public void ReadPlayerRounds(int player) //string?? 
    {
        // find the players previous rounds
    }

    public void ReadPlayerRound(int round)
    {
        // return the list of vector3(or 2), for that specific date/round
    }

    public void ChangePlayerData(string playerName, float playerHandicap, int tee)
    {
        // Create or open the file for writing (appending)
        using (StreamWriter writer = new StreamWriter(filePath, true))
        {
            // Format the data as a CSV line and write it to the file
            string playerData = $"{playerName}|{playerHandicap}";
            writer.WriteLine(playerData);
        }
    }

    public void ChangePlayerApperance(string shoes, string pants, string shirt, string hat, string glasses)
    {
        //save new player apperance to file
    }

    public void WritePlayerRound(int player, List<Vector3> shotCoordinates)
    {
        // Find that players folder/textfile and append those coordinates in the file
    }

    public void ReadCourseInfo()
    {
        string CourseParametersFile = Application.dataPath + "/GolfTracker/UserData/LatestCourseData.txt"; // move to Start()
        int parameterIndex = 0;

        if (File.Exists(CourseParametersFile))
        {
            // Reads all lines from PlayerText file
            string[] lines = File.ReadAllLines(CourseParametersFile);

            // Process each line (each line should represent a player's data)
            foreach (string line in lines)
            {
                // Split the line into parts (assuming a simple format, e.g., "Wind:5")
                string[] parts = line.Split(':');

                courseParameters[parameterIndex] = parts[1];

                parameterIndex++;

                // Something for tee and flag positions
            }
        }
        else
        {
            Debug.LogError("File does not exist.");
        }
    }

    public void WriteCourseInfo() //other scripts shold change the necessary varables, and then we just write them to the file here
    {
        string CourseParametersFile = Application.dataPath + "/GolfTracker/UserData"; // move to Start()

        string fileContentCourseParameters = $"Date:{courseParameters[0]}\nDay:{courseParameters[1]}\nSunny:{courseParameters[2]}\nDry:{courseParameters[3]}\nWind:{courseParameters[4]}";


        File.WriteAllText(CourseParametersFile, fileContentCourseParameters); // + tee and flag positions
    }


    public void CreateNewPlayer() //string name, float hcp, int tee
    {
        string nameInput = "test"; //put the name of player here
        float hcpInput = 64f; //put hcp here
        int teeInput = 47; //put tee spot here

        // Specify the directory and file names
        string userDataPath = Application.dataPath + "/GolfTracker/UserData";
        string fileName = nameInput;
        string folderPath = Path.Combine(userDataPath, fileName);
        string filePathPersonal = Path.Combine(folderPath, fileName + "Personal.txt"); //do this for each textfile
        string filePathRounds = Path.Combine(folderPath, fileName + "Rounds.txt");

        // Create the directory if it doesn't exist (if it does we are just overwriting its data)
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // Person specific information to textfile
        string fileContentPersonalPerson = $"Name:{nameInput}, HCP:{hcpInput}, Tee:{teeInput}\n\nClothes:default,default,default,default,default\n\n";
        // Default club shot ranges (basically just mine :>)
        string fileContentPersonalClubs = $"Driver1:175,225,10.9\nDriver2:150,200,11.4\nHybrid1:125,175,10.2\nHybrid2:100,150,10.7\n" +
            $"Wood1:140,180,9.0\nWood2:135,175,9.1\nWood3:130,170,9.2\nWood4:125,165,9.4\nWood5:120,160,9.6\nWood6:115,155,9.9\nWood7:110,150,10.4\nWood8:105,145,11.2\nWood9:100,140,12.8\n" +
            $"Iron1:120,160,9.7\nIron2:115,155,9.9\nIron3:110,150,10.4\nIron4:105,145,11.0\nIron5:100,140,12.1\nIron6:95,135,14.1\nIron7:90,130,16.3\nIron8:85,125,18.2\nIron9:80,120,20.4\n" +
            $"PitchWedge:15,80,24.2\nSandWedge:2,15,30.5\nLobWedge:2,5,36.8\nPutter:0,999,0";

        // Writing the content to the specified filepaths
        File.WriteAllText(filePathPersonal, fileContentPersonalPerson + fileContentPersonalClubs);
        File.WriteAllText(filePathRounds, ""); //empty string, because else the file isn't actually created, as it would be null

        Debug.Log("Text file created at: " + folderPath);
    }
}

