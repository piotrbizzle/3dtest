using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // screen boundaries
    private const float ScreenTopY = 4.2f;
    private const float ScreenBottomY = -4.2f;
    private const float ScreenRightX = 5.2f;
    private const float ScreenLeftX = -5.2f;
    private const float ScreenEdgeBuffer = 0.25f;

    private string currentTile = "0_0_democellmap";
    
    void Start() {
        
    }

    void Update() {
         this.Move();	 
    }

    private void Move() {
	// get input
	bool up = Input.GetKey("w");
	bool down = Input.GetKey("s");
	bool left = Input.GetKey("a");
	bool right = Input.GetKey("d");

	// modify speed
	bool vertical = up ^ down;
	bool horizontal = left ^ right;
	bool diagonal = vertical && horizontal;	
	float speedScale = diagonal ? 6.2f : 8.0f;

	// try moving
	Vector3 previousPosition = this.transform.position;	
	if (up && this.transform.position.y < ScreenTopY + ScreenEdgeBuffer) {
	    this.transform.Translate(Vector3.up * Time.deltaTime * speedScale);
	}
	if (down && this.transform.position.y > ScreenBottomY - ScreenEdgeBuffer) {
	    this.transform.Translate(Vector3.down * Time.deltaTime * speedScale);
	}
	if (left && this.transform.position.x > ScreenLeftX - ScreenEdgeBuffer) {
	    this.transform.Translate(Vector3.left * Time.deltaTime * speedScale);
	}
	if (right && this.transform.position.x < ScreenRightX + ScreenEdgeBuffer) {
	    this.transform.Translate(Vector3.right * Time.deltaTime * speedScale);
	}

	if (this.transform.position == previousPosition) {
	    return;
	}

	// check depth map
	int depth = this.GetComponent<Sprite3D>().GetDepth();
	
	bool belowFull = depth == 4 ? false :  Tile.TestCoordinate(currentTile, depth + 1, this.transform.position.x, this.transform.position.y) == 1;
	bool currentFull = Tile.TestCoordinate(currentTile, depth, this.transform.position.x, this.transform.position.y) == 1;
	bool aboveFull = depth < 1 ? false : Tile.TestCoordinate(currentTile, depth - 1, this.transform.position.x, this.transform.position.y) == 1;
	bool abovePlusOneFull = depth < 2 ? false : Tile.TestCoordinate(currentTile, depth - 2, this.transform.position.x, this.transform.position.y) == 1;
	
	/*
	Debug.Log("---");
	Debug.Log(belowFull);
	Debug.Log(currentFull);
	Debug.Log(aboveFull);
	Debug.Log(abovePlusOneFull);
	*/
	// undo move if you'd go into a wall
	if (aboveFull && abovePlusOneFull) {
	    Debug.Log("walled");
	    this.transform.position = previousPosition;
	    return;
	}

	// step up
	if (aboveFull) {
	    if (depth == 1) {
		// at depth 1, walls immediately above block you
		Debug.Log("top walled");
		this.transform.position = previousPosition;
		return;
	    }

	    Debug.Log("stepup");
	    this.GetComponent<Sprite3D>().SetDepth(depth - 1);
	    return;
	}

        // move normally
	if (currentFull) {
	    Debug.Log("moved");
	    return;
	}

	// step down
	if (belowFull) {
	    Debug.Log("stepdown");
	    this.GetComponent<Sprite3D>().SetDepth(depth + 1);
	    return;
	}

	// undo move if there is no where to step to
	Debug.Log("cliff");
	this.transform.position = previousPosition;
	return;
    }
}
