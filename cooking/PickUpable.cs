using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
	// collision
        this.gameObject.AddComponent<BoxCollider2D>();
	this.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;

	// description
	this.gameObject.AddComponent<Dialogue>().startingKnot = this.gameObject.name.Replace('/', '_');
    }
}
