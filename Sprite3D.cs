using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprite3D : MonoBehaviour
{
    public Color lColor;
    public Color mColor;
    public Color rColor;
    public int depth;
    
    private GameObject lGo;
    private GameObject rGo;

    private Sprite[] lSprites = new Sprite[5];
    private Sprite[] mSprites = new Sprite[5];
    private Sprite[] rSprites = new Sprite[5];
    
    void Start()
    {
	// load all sprites
	for (int i = 0; i < 5; i++) {
	    this.lSprites[i] = Resources.Load<Sprite>(i.ToString() + "l_" + this.name);
	    this.mSprites[i] = Resources.Load<Sprite>(i.ToString() + "m_" + this.name);
	    this.rSprites[i] = Resources.Load<Sprite>(i.ToString() + "r_" + this.name);
	}
	
	// create left and right view and colorize
	this.lGo = new GameObject();
	this.lGo.transform.position = this.transform.position;
	this.lGo.transform.SetParent(this.transform);
	this.lGo.AddComponent<SpriteRenderer>().color = this.lColor;

	this.GetComponent<SpriteRenderer>().color = this.mColor;
	
	this.rGo = new GameObject();
	this.rGo.transform.position = this.transform.position;
	this.rGo.transform.SetParent(this.transform);
	this.rGo.AddComponent<SpriteRenderer>().color = this.rColor;
    }

    void Update()
    {
        this.lGo.GetComponent<SpriteRenderer>().sprite = this.lSprites[this.depth];
	this.lGo.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("D" + this.depth.ToString());
	
	this.GetComponent<SpriteRenderer>().sprite = this.mSprites[this.depth];
	this.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("D" + this.depth.ToString());
	
	this.rGo.GetComponent<SpriteRenderer>().sprite = this.rSprites[this.depth];
	this.rGo.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("D" + this.depth.ToString());
    }
}
