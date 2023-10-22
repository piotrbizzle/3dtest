using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpable : MonoBehaviour
{
    bool needsScaling = false;
    
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
	if (this.needsScaling) {
	    this.GetComponent<Sprite3D>().ResetScaleSprite();
	    this.needsScaling = false;
	}
    }
    
    public void InitCreatedItem(string itemName, int depth) {
	// set name
	this.gameObject.name = itemName;
	
	// create sprites
	this.gameObject.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(itemName);
	this.gameObject.AddComponent<Sprite3D>().isScaling = true;
	this.gameObject.GetComponent<Sprite3D>().SetDepth(depth);
	this.needsScaling = true;
    }

    public string GetItemName() {
	return this.gameObject.name;
    }
}
