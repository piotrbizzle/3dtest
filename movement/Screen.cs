using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screen : MonoBehaviour {
    public Screen north;
    public Screen east;
    public Screen south;
    public Screen west;
    public bool inRealWorld;

    void Start() {
	// center 
	this.gameObject.transform.position = new Vector3(0,0,0);

	// 3dify if needed
	if (!this.inRealWorld) {		
	    Destroy(this.GetComponent<SpriteRenderer>());

	    // render all the layers
	    for (int i = 0; i < 5; i++) {
		GameObject tile = new GameObject();
		tile.name = "tiles/" + this.name + i.ToString();
		tile.transform.SetParent(this.transform);
		tile.AddComponent<SpriteRenderer>();
		Sprite3D tileSprite3D = tile.AddComponent<Sprite3D>();
		tileSprite3D.isStatic = true;
		tileSprite3D.depth = i;
	    }
	}
	
	// deactivate, the player will reactivate the start screen on frame 1
	this.gameObject.SetActive(false);

    }
}
