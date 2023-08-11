using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class changeScene : MonoBehaviour
{
    // Start is called before the first frame update
    public int levelIndex;
    public Text playedOrLocked;
    public Button button;

    private void Start() {
        //to reset highest scores use below for loop
        /*
        for(int i = 0; i < 25 ; i++){
            PlayerPrefs.SetInt("HighScore" + (levelIndex - 1), 0);
        }
        */
        playedOrLocked.text = "PLAY";
        button.image.color = Color.green;
        if(levelIndex > 1){
            int highestScore = PlayerPrefs.GetInt("HighScore" + (levelIndex - 1), 0);
            if(highestScore == 0){
                playedOrLocked.text = "LOCKED";
                button.image.color = Color.gray;
            }
        }
       
    }
    public void MoveToScene(int sceneID) {

       if(levelIndex > 1){
            int prevHighestScore = PlayerPrefs.GetInt("HighScore" + (levelIndex - 1), 0);
            if(prevHighestScore != 0){
                SceneManager.LoadScene(sceneID);
            }
        }else{
            SceneManager.LoadScene(sceneID);
        }
        
    }

    public void SetSelectedLevel(){
        PlayerPrefs.SetInt("SelectedLevel", levelIndex);
    }
}
