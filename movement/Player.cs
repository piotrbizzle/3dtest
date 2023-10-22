using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Screen currentScreen;

    // ink stuff
    public InkStory inkStory;
    
    // screen boundaries
    private const float ScreenTopY = 4.2f;
    private const float ScreenBottomY = -4.2f;
    private const float ScreenRightX = 5.2f;
    private const float ScreenLeftX = -5.2f;
    private const float ScreenEdgeBuffer = 0.25f;

    // name of current screen, for collisions
    private string currentTile;
    private bool inited;

    // controls
    private bool qHeld;
    private bool eHeld;
    private bool spaceHeld;
    
    void Start() {
       this.currentTile = this.currentScreen.name;

       // collision
       this.gameObject.AddComponent<BoxCollider2D>();
       this.gameObject.GetComponent<BoxCollider2D>().size = this.GetComponent<SpriteRenderer>().size;

       Rigidbody2D rb = this.gameObject.AddComponent<Rigidbody2D>();
       rb.gravityScale = 0.0f;
       rb.constraints = RigidbodyConstraints2D.FreezeRotation;
       rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
       rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
    }

    // init runs on the first call to Update
    void Init() {
	this.inited = true;
	this.currentScreen.gameObject.SetActive(true);
	if (this.currentScreen.inRealWorld) {
	   this.EnterRealWorld();
       }

    }

    void Update() {	
	if (!inited) {
	    this.Init();
	}
	if (this.inkStory.isVisible) {
	    return;
	}
	this.Act();
	this.Move();	
	this.MoveScreens();
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
	    foreach (Transform child in this.gameObject.transform) {
		if (child.gameObject.GetComponent<PickUpable>()) {
		    child.GetComponent<Sprite3D>().SetDepth(depth - 1);
		}
	    }
	    this.FixInventory();
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
	    foreach (Transform child in this.gameObject.transform) {
		if (child.gameObject.GetComponent<PickUpable>()) {
		    child.GetComponent<Sprite3D>().SetDepth(depth + 1);
		}
	    }
	    this.FixInventory();
	    return;
	}

	// undo move if there is no where to step to
	Debug.Log("cliff");
	this.transform.position = previousPosition;
	return;
    }

    private void MoveScreens() {
	// go north
	Transform playerTransform = this.gameObject.transform;
	if (playerTransform.position.y > ScreenTopY && currentScreen.north != null) {
	    playerTransform.Translate(new Vector3(0.0f, -8f, 0.0f));
    	    this.SwapScreens(this.currentScreen.north);
	}
	// go south
	if (playerTransform.position.y < ScreenBottomY && currentScreen.south != null) {
	    playerTransform.Translate(new Vector3(0.0f, 8f, 0.0f));
    	    this.SwapScreens(this.currentScreen.south);
	}
	// go east
	if (playerTransform.position.x > ScreenRightX && currentScreen.east != null) {
    	    playerTransform.Translate(new Vector3(-10f, 0.0f, 0.0f));
	    this.SwapScreens(this.currentScreen.east);
	}
	// go west
	if (playerTransform.position.x < ScreenLeftX && currentScreen.west != null) {
	    playerTransform.Translate(new Vector3(10f, 0.0f, 0.0f));
	    this.SwapScreens(this.currentScreen.west);
	}
    }

    private void SwapScreens(Screen newScreen) {
	// check if graphics style needs to change
	if (this.currentScreen.inRealWorld && !newScreen.inRealWorld) {
	    this.ExitRealWorld();
	}
	if (!this.currentScreen.inRealWorld && newScreen.inRealWorld) {
	    this.EnterRealWorld();
	}

	
	// swap screens
	this.currentScreen.gameObject.SetActive(false);
	this.currentScreen = newScreen;
	this.currentTile = this.currentScreen.name;
	this.currentScreen.gameObject.SetActive(true);
    }

    private void Act() {	
	// pick up item
	this.eHeld = Input.GetKey("e");

	// drop item
	bool qPressed = Input.GetKey("q");
	if (qPressed && !this.qHeld) {
	    this.Drop();
	}
	this.qHeld = qPressed;

	// interact
	this.spaceHeld = Input.GetKey("space");
    }
    
    void OnTriggerStay2D(Collider2D collider) {
	if (this.inkStory.isVisible) {
	    return;
	}

	// start dialogue
	Dialogue dialogue = collider.gameObject.GetComponent<Dialogue>();
	if (dialogue != null && this.spaceHeld) {
	    Sprite dialogueSprite = this.currentScreen.inRealWorld ? dialogue.gameObject.GetComponent<Sprite3D>().spriteReal : dialogue.gameObject.GetComponent<Sprite3D>().dialogueSprite;
	    this.inkStory.OpenStory(dialogue.startingKnot, dialogueSprite);
	    return;
	}
	
	// pick up item
	PickUpable item = collider.gameObject.GetComponent<PickUpable>();
	if (item != null && this.eHeld) {
	    this.PickUp(item);
	    return;
	}
    }

    private void PickUp(PickUpable item) {
	// don't pick up if already held
	if (item.gameObject.transform.parent == this.gameObject.transform) {
	    return;
	}

	// don't pick up if on a different depth
	if (item.GetComponent<Sprite3D>() == null || item.GetComponent<Sprite3D>().GetDepth() != this.GetComponent<Sprite3D>().GetDepth()) {
	    return;
	}	

	// check if there is room in the inventory
	if (this.ItemCount() >= 3) {
	    return;
	}

	// add item to player
	GameObject itemGo = item.gameObject;
	itemGo.transform.parent = this.gameObject.transform;

	this.FixInventory();       
	this.UpdateInkStoryInventory();
    }

    private int ItemCount() {
	int heldItemsCount = 0;
	for (int i = 0; i < this.gameObject.transform.childCount; i++) {
	    Transform child = this.gameObject.transform.GetChild(i);

	    // count pickupables only
	    if (child.gameObject.GetComponent<PickUpable>() == null) {
		continue;
	    }
	    heldItemsCount += 1;
	}
	return heldItemsCount;
    }
    
    private void FixInventory() {
	Vector3 playerPosition = this.gameObject.transform.position;
	int itemCount = 0;
	for (int i = 0; i < this.gameObject.transform.childCount; i++) {
	    Transform child = this.gameObject.transform.GetChild(i);

	    // skip non-pickupables
	    if (child.gameObject.GetComponent<PickUpable>() == null) {
		continue;
	    }

	    float scaleFactor = this.GetComponent<Sprite3D>().GetScaleFactor();
	    float offset = 1.5f * scaleFactor;
	    child.position = new Vector3(playerPosition.x, playerPosition.y + (itemCount + 1) * offset, 0.0f);
	    itemCount += 1;
	}
    }

    private void Drop() {
	for (int i = 0; i < this.gameObject.transform.childCount; i++) {
	    Transform child = this.gameObject.transform.GetChild(this.gameObject.transform.childCount - i - 1);
	    if (child.GetComponent<PickUpable>()) {
		// drop first item found and return
		child.position = this.gameObject.transform.position;
		child.parent = this.currentScreen.gameObject.transform;
		return;
	    }
	}

	this.UpdateInkStoryInventory();
    }

    public void UpdateInkStoryInventory() {
	List<string> itemNames = new List<string>();
	foreach (Transform child in this.transform) {
	    if (child.GetComponent<PickUpable>() == null) {
		continue;
	    }
	    itemNames.Add(child.gameObject.GetComponent<PickUpable>().GetItemName());
	}
	this.inkStory.UpdateInventory(itemNames);
    }
    
    public void ReceiveItem(string itemName) {
	GameObject itemGo = new GameObject(itemName.Replace('_', '/'));
	itemGo.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(itemGo.name);
	PickUpable item = itemGo.AddComponent<PickUpable>();
	item.InitCreatedItem(this.GetComponent<Sprite3D>().GetDepth(), this.currentScreen.inRealWorld);

	if (this.ItemCount() < 3) {
	    // pick up item if there's room
	    this.PickUp(item);
	} else {
	    // otherwise, drop item on the ground
	    item.gameObject.transform.parent = this.currentScreen.transform;
	    item.gameObject.transform.position = this.gameObject.transform.position;
	}
    }

    public void LoseItem(string itemName) {
	Vector3 playerPosition = this.gameObject.transform.position;

	for (int i = 0; i < this.gameObject.transform.childCount; i++) {
	    Transform child = this.gameObject.transform.GetChild(i);
	    if (child.gameObject.GetComponent<PickUpable>() == null) {
		continue;
	    }	       
	    if (child.gameObject.GetComponent<PickUpable>().GetItemName() == itemName.Replace('_', '/')) {
		child.parent = child.parent.parent; // fixes held item count
		GameObject.Destroy(child.gameObject);
		break;
	    }
	}

	this.FixInventory();
	this.UpdateInkStoryInventory();
    }

    public void EnterRealWorld() {
	this.GetComponent<Sprite3D>().EnterRealWorld();
	foreach (Transform child in this.transform) {
	    if (child.GetComponent<Sprite3D>() == null) {
		continue;
	    }
	    child.GetComponent<Sprite3D>().EnterRealWorld();
	}
    }

    public void ExitRealWorld() {
	this.GetComponent<Sprite3D>().ExitRealWorld();
	foreach (Transform child in this.transform) {
	    if (child.GetComponent<Sprite3D>() == null) {
		continue;
	    }
	    child.GetComponent<Sprite3D>().ExitRealWorld();
	}
    }
}
