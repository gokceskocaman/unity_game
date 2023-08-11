using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class LevelControl : MonoBehaviour
{
    // Start is called before the first frame update
    public int level;
    public Text highScoreText;
    void Start()
    {
        int highestScore = PlayerPrefs.GetInt("HighScore" + level, 0);
        if(highestScore == 0){
            highScoreText.text = "No Score";
        }
        else{
            highScoreText.text = "High Score: " + highestScore;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
