using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpable : MonoBehaviour
{
    bool needsInit;
    bool initInRealWorld;
    bool needsScaling;

    
    // Start is called before the first frame update
    void Start()
    {
	// collision
        this.gameObject.AddComponent<BoxCollider2D>();
	this.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
	
	// description
	this.gameObject.AddComponent<Dialogue>().startingKnot = this.gameObject.name.Replace('/', '_');
    }

    void Update() {
	// we need to init this in several steps to avoid race conditions
	if (this.needsScaling) {
	    this.GetComponent<Sprite3D>().ResetScaleSprite();
	    this.GetComponent<Sprite3D>().isDepthDirty = true;
	    this.needsScaling = false;
	    this.gameObject.GetComponent<Sprite3D>().isScaling = true;

	    // collision
	    this.gameObject.AddComponent<BoxCollider2D>();
	    this.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
	}
	if (this.needsInit) {	    
	    if (this.initInRealWorld) {
		this.gameObject.GetComponent<Sprite3D>().EnterRealWorld();
	    }
	    this.needsInit = false;
	    this.needsScaling = true;

	    // remove old collision
	    // TODO: do something more graceful here
	    Destroy(this.gameObject.GetComponent<BoxCollider2D>());
	}

    }
    
    public void InitCreatedItem(int depth, bool inRealWorld) {
	this.needsInit = true;
	this.gameObject.AddComponent<Sprite3D>().SetDepth(depth);
	this.initInRealWorld = inRealWorld;
    }

    public string GetItemName() {
	return this.gameObject.name;
    }
}
