using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class LevelDataParser : MonoBehaviour
{
    public TextAsset levelDataFile; 
    public TextAsset[] levelFiles; // Attach your text file in the Inspector

    public int levelNumber;
    public int gridWidth;
    public int gridHeight;
    public int moveCount;
    public string gridData;
    public int selectedLevel;

    //downloaded data
    public int[] grid_width = new int[15];
    public int[] grid_height = new int[15];
    public int[] move_count = new int[15];
    public string[] grid = new string[15];
    

    private void Start() {
        for(int i = 0; i < 15; i++){
            grid_width[i] = 0;
            grid_height[i] = 0;
            move_count[i] = 0;
            grid[i] = "";
        }
    }
    

    private void Awake()
    {
        
        selectedLevel = PlayerPrefs.GetInt("SelectedLevel", 0);
       

        if(selectedLevel <= 10){
            levelDataFile = levelFiles[selectedLevel - 1];
            string[] lines = levelDataFile.text.Split('\n');

            foreach (string line in lines)
            {
                string[] parts = line.Split(':');
                if (parts.Length == 2)
                {
                    string key = parts[0].Trim();
                    string value = parts[1].Trim();

                    switch (key)
                    {
                        case "level_number":
                            levelNumber = int.Parse(value);
                            break;
                        case "grid_width":
                            gridWidth = int.Parse(value);
                            break;
                        case "grid_height":
                            gridHeight = int.Parse(value);
                            break;
                        case "move_count":
                            moveCount = int.Parse(value);
                            break;
                        case "grid":
                            gridData = value;
                            break;
                    }
                }
            }

        }
        else{

            if(grid_width[selectedLevel - 11] == 0){
                // if level is not downloaded before
                string url;
                if(selectedLevel > 10 && selectedLevel <= 15){
                    url = "https://row-match.s3.amazonaws.com/levels/RM_A" + selectedLevel;
                }
                else{
                    url = "https://row-match.s3.amazonaws.com/levels/RM_B" + (selectedLevel - 15);
                }
                StartCoroutine(GetText(url));
            }
            else{
                gridWidth = grid_width[selectedLevel - 11];
                gridHeight = grid_height[selectedLevel - 11];
                moveCount = move_count[selectedLevel - 11];
                gridData = grid[selectedLevel - 11];
            }
        }
    }

    IEnumerator GetText(string url) {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();
 
        if (www.result != UnityWebRequest.Result.Success) {
            Debug.Log(www.error);
            Debug.Log("error");
        }
        else {
            // Show results as text
      
            Debug.Log(www.downloadHandler.text);
            string data = www.downloadHandler.text;
            string[] lines = data.Split('\n');
            foreach (string line in lines){
                if (line.StartsWith("level_number:"))
                {
                    levelNumber = int.Parse(line.Split(':')[1].Trim());

                }
                else if (line.StartsWith("grid_width:"))
                {
                    gridWidth = int.Parse(line.Split(':')[1].Trim());
                    grid_width[levelNumber - 11] = gridWidth;
                }
                else if (line.StartsWith("grid_height:"))
                {
                    gridHeight = int.Parse(line.Split(':')[1].Trim());
                    grid_height[levelNumber - 11] = gridHeight;
                }
                else if (line.StartsWith("move_count:"))
                {
                    moveCount = int.Parse(line.Split(':')[1].Trim());
                    move_count[levelNumber - 11] = moveCount;
                }
                else if (line.StartsWith("grid:"))
                {
                    gridData = line.Split(':')[1].Trim();
                    grid[levelNumber - 11] = gridData;
                }
            }
            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }
    }

}
