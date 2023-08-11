using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScalar : MonoBehaviour
{
    private Board board;
    public float cameraOffset;
    public float padding = 2;
    public float aspectRatio = 0.625f;
    private Vector3 tempPosition;

    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        if(board != null){
            if(board.height >= board.width){
                tempPosition = new Vector3(Mathf.Round(board.width/2), Mathf.Round(board.height/2) + 1, cameraOffset);
            }
            else{
                tempPosition = new Vector3((board.width - 1)/2 , (board.height - 1)/2);
            }
            RepositionCamera(tempPosition);
        }
    }

    void RepositionCamera(Vector3 position){
        transform.position = tempPosition;
        if(board.width >= board.height){
            Camera.main.orthographicSize = (board.width / 2 + padding) / aspectRatio;
        }
        else{
            Camera.main.orthographicSize = board.height / 2 + padding;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(board != null){
            if(board.height >= board.width){
                tempPosition = new Vector3(Mathf.Round(board.width/2), Mathf.Round(board.height/2) + 1, cameraOffset);
            }
            else{
                tempPosition = new Vector3((board.width - 1)/2 , (board.height - 1)/2, cameraOffset);
            }
            RepositionCamera(tempPosition);
        }
        
    }
}
