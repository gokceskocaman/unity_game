using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class mainSceneChange : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void MoveToScene(int sceneID) {

       SceneManager.LoadScene(sceneID);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
