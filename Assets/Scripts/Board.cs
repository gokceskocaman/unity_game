using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class Board : MonoBehaviour
{
    public int width;
    public int height;
    public GameObject tilePrefab;
    private BackgroundTile[,] allTiles;
    public GameObject[,] allDots;
    public GameObject[] circles;
    private string gridString;
    private string[] gridList;
    public int move_count;
    public int score = 0;
    public int highestScore;
    private int scoreToAdd = 0;
    public List<int> matchedRows = new List<int>();
    public Text scoreText;
    public Text remainingMoves;
    public Text highScoreText;
    public Text newHighScoreText;
    public int MainSceneID;
    public int level;
    public GameObject celebrationPopup;

    
   


    // Start is called before the first frame update
    void Start()
    {
        level = PlayerPrefs.GetInt("SelectedLevel" , 0);
        if(level <= 10){
            LevelDataParser parser = FindObjectOfType<LevelDataParser>();
            level = parser.levelNumber;
            gridString = parser.gridData;
            gridList = gridString.Split(',');
            allTiles = new BackgroundTile[width, height];  
            width = parser.gridWidth;
            height = parser.gridHeight;
            allDots = new GameObject[width, height];
            setUp();
            move_count = parser.moveCount;
            scoreText.text = "Score: " + score;
            highestScore = PlayerPrefs.GetInt("HighScore" + level, 0);
            highScoreText.text = "Highest Score: " + highestScore;
            remainingMoves.text = "Remaining Moves: " + move_count;
            StartCoroutine(StartLevel());
        }else{
             StartCoroutine(StartLevel());         
        }
        
        
    }

    IEnumerator StartLevel(){
        LevelDataParser parser = FindObjectOfType<LevelDataParser>();
         yield return new WaitForSeconds(1.0f);
        level = parser.levelNumber;
        gridString = parser.gridData;
        gridList = gridString.Split(',');
        allTiles = new BackgroundTile[width, height];  
        width = parser.gridWidth;
        height = parser.gridHeight;
        allDots = new GameObject[width, height];
        setUp();
        move_count = parser.moveCount;
        scoreText.text = "Score: " + score;
        highestScore = PlayerPrefs.GetInt("HighScore" + level, 0);
        highScoreText.text = "Highest Score: " + highestScore;
        remainingMoves.text = "Remaining Moves: " + move_count;
    }

   


    private void Update() {
        if(move_count == 0 && width > 0){  
            //if grid is setup then move_count become zero  
            if(!celebrationPopup.activeSelf){
                finishLevel();
            }    
        }else if (width == 0){
            //if grid is not setup yet  
        }
        else{
            //check row match
            for (int row = 0; row < height; row++)
            {
                if (CheckForRowMatch(row) && !matchedRows.Contains(row))
                {
                    matchedRows.Add(row);
                    // Handle row match, update score, etc.
                    score += scoreToAdd;
                    scoreText.text = "Score: " + score;
                    for(int i = 0; i < width; i++){
                        Vector2 temp = new Vector2(i, row);
                        GameObject circle = Instantiate(circles[4], temp, Quaternion.identity);
                        circle.transform.parent = this.transform;
                        allDots[i, row] = circle;
                    }
                }
            }
        }
        
    }

    public void finishLevel(){
        if(highestScore < score){
            //new highest score
            highestScore = score;
            highScoreText.text = "High Score: " + highestScore;
            newHighScoreText.text = "New High Score: " + highestScore;
            celebrationPopup.SetActive(true);
            StartCoroutine(HideCelebrationAndLoadLevel());
        }
        else{
            //without new highest score
            SceneManager.LoadScene(MainSceneID);
        }
        PlayerPrefs.SetInt("HighScore" + level, highestScore);
    }


    private IEnumerator HideCelebrationAndLoadLevel()
    {
        yield return new WaitForSeconds(5f);

        celebrationPopup.SetActive(false);

        SceneManager.LoadScene(MainSceneID); // Replace with the actual scene name
    }

    public bool CheckForRowMatch(int row)
    {
        bool isMatch = true;
        GameObject temp = allDots[0, 0]; ;

        for (int col = 0; col < width - 2; col++)
        {
            GameObject dotA = allDots[col, row];
            GameObject dotB = allDots[col + 1, row];
            GameObject dotC = allDots[col + 2, row];
            

            if (dotA != null && dotB != null && dotC != null)
            {
                if (dotA.tag != dotB.tag || dotB.tag != dotC.tag)
                {
                    isMatch = false; // Row match found
                    break;
                }

            }

            temp = dotA;
        }

        if(isMatch){

            switch(temp.tag){
                case "red":
                    scoreToAdd = 100;
                    break;
                case "green":
                    scoreToAdd = 150;
                    break;
                case "blue":
                    scoreToAdd = 200;
                    break;
                case "yellow":
                    scoreToAdd = 250;
                    break;
                default:
                    scoreToAdd = 0;
                    break;

            }

           
        }

        return isMatch; // No row match
    }

    public void updateMoveCount(){
        move_count--;
        remainingMoves.text = "Remaining Moves: " + move_count;
    }

    private void setUp(){
        for(int i = 0; i < width; i++){
            for(int j = 0; j < height; j++){
                Vector2 temp = new Vector2(i, j);
                GameObject backgroundTile = Instantiate(tilePrefab, temp , Quaternion.identity) as GameObject;
                backgroundTile.transform.parent = this.transform;
                backgroundTile.name = "(" + i + "," + j + ")";

                int gridListIndex = height * i + j;
                string color = gridList[gridListIndex];
                int colorIndex;
                switch(color){
                    case "r":
                        colorIndex = 0;
                        break;
                    case "g":
                        colorIndex = 1;
                        break;
                    case "b":
                        colorIndex = 2;
                        break;
                    case "y":
                        colorIndex = 3;
                        break;
                    default:
                        colorIndex = 0;
                        break;

                }

                GameObject circle = Instantiate(circles[colorIndex], temp, Quaternion.identity);
                circle.transform.parent = this.transform;
                circle.name = "(" + i + "," + j + ")";
                 allDots[i, j] = circle;

            }
        }
    }
}
