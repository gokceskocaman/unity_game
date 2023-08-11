using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{
    public int column;
    public int row;
    public int targetX;
    public int targetY;
    private Board board;
    private GameObject otherDot;
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    private Vector2 tempPosition;
    public float swipeAngle;


    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        targetX = (int)transform.position.x;
        targetY = (int)transform.position.y;
        row = targetY;
        column = targetX;
     
    }

    // Update is called once per frame
    void Update()
    {

    
        targetX = column;
        targetY = row;
        if (Mathf.Abs(targetX - transform.position.x) > .1){
            //Move Towards the target
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .6f);
            if(board.allDots[column, row] != this.gameObject){
                board.allDots[column, row] = this.gameObject;
            }
        }else{
            //Directly set the position
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;
            board.allDots[column, row] = this.gameObject;
        }
        if (Mathf.Abs(targetY - transform.position.y) > .1){
            //Move Towards the target
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .6f);
            if (board.allDots[column, row] != this.gameObject)
            {
                board.allDots[column, row] = this.gameObject;
            }
        }
        else{
            //Directly set the position
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;
            board.allDots[column, row] = this.gameObject;
        }
    }

    private void OnMouseDown() {
        firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log(firstTouchPosition);
    }

    private void OnMouseUp() {
        finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log(finalTouchPosition);
        CalculateAngle();
    }

    void CalculateAngle(){
        swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
        Debug.Log(swipeAngle);
        if(board.move_count > 0){
            MovePieces();
        }
    }

    void MovePieces(){
        if(swipeAngle > -30 && swipeAngle <= 30 && column < board.width-1 && !board.matchedRows.Contains(row)){
            //Right Swipe
            board.updateMoveCount();
            otherDot = board.allDots[column + 1, row];
            otherDot.GetComponent<Dot>().column -=1;
            column += 1;

        } else if(swipeAngle > 60 && swipeAngle <= 120 && row < board.height-1 && !board.matchedRows.Contains(row) && !board.matchedRows.Contains(row + 1)){
            //Up Swipe
            board.updateMoveCount();
            otherDot = board.allDots[column, row + 1];
            otherDot.GetComponent<Dot>().row -=1;
            row += 1;
           
        } else if((swipeAngle > 150 || swipeAngle <= -150) && column > 0 && !board.matchedRows.Contains(row)){
            //Left Swipe
            board.updateMoveCount();
            otherDot = board.allDots[column - 1, row];
            otherDot.GetComponent<Dot>().column +=1;
            column -= 1;
        } else if(swipeAngle < -60 && swipeAngle >= -120 && row > 0 && !board.matchedRows.Contains(row) && !board.matchedRows.Contains(row - 1)){
            //Down Swipe
            board.updateMoveCount();
            otherDot = board.allDots[column, row - 1];
            otherDot.GetComponent<Dot>().row +=1;
            row -= 1;
        }

        
        
    }


}
