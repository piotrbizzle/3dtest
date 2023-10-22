using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screen : MonoBehaviour {
    void Start() {
	for (int i = 0; i < 5; i++) {
	    GameObject tile = new GameObject();
	    tile.name = this.name + i.ToString();
	    tile.transform.SetParent(this.transform);
	    tile.AddComponent<SpriteRenderer>();
	    Sprite3D tileSprite3D = tile.AddComponent<Sprite3D>();
	    tileSprite3D.isStatic = true;
	    tileSprite3D.depth = i;
	}
    }
}
