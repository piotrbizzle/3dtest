using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;

public class InkStory : MonoBehaviour
{
    public TextAsset inkJSONAsset;
    public GameObject subCanvas;
    public SpriteRenderer screenDimmer;
    private Story story;
    public bool isVisible;
    public Player player;
    private Sprite speakerSprite;


    // UI Prefabs
    public Text textPrefab;
    public Button buttonPrefab;
    public Image imagePrefab;
    private Image shownImage;
    
    public void Start() {
	this.story = new Story(inkJSONAsset.text);
    }
    
    public void OpenStory(string startingKnot, Sprite speakerSprite) {
	this.speakerSprite = speakerSprite;
	this.screenDimmer.color = new Color(0f, 0f, 0f, 0.5f);
	this.story.ChoosePathString(startingKnot);
	this.isVisible = true;
	this.RefreshView();
    }

    private void RefreshView () {
	this.ClearView();

	// show speaker image	
	Image speakerImage = Instantiate(imagePrefab) as Image;
	speakerImage.sprite = this.speakerSprite;
	speakerImage.transform.SetParent(this.subCanvas.transform);
	this.shownImage = speakerImage;
	
	// show next story text
	while (this.story.canContinue) {
	    Text storyText = Instantiate(textPrefab) as Text;
	    storyText.text = this.story.Continue().Trim();
	    storyText.transform.SetParent(this.subCanvas.transform);

	    this.ProcessTags();
	}
	
	if (story.currentChoices.Count > 0) {
            // show choice buttons
	    for (int i = 0; i < this.story.currentChoices.Count; i++) {
		Choice choice = this.story.currentChoices[i];
		Button button = CreateChoiceButton(choice.text.Trim());
		button.onClick.AddListener(delegate {
			this.OnClickChoiceButton(choice);
		    }
		);
	    }
	} else {
	    // exit out of story
	    this.ClearView();
	    this.isVisible = false;	    	    
	    this.screenDimmer.color = Color.clear;		
	}
    }

    private void ClearView() {
	foreach (Transform child in this.subCanvas.transform) {
	    GameObject.Destroy(child.gameObject);
	}
    }

    private void ProcessTags() {
	for (int i = 0; i < story.currentTags.Count; i++) {
	    string tag = story.currentTags[i].Trim();

	    /*
	    // give item
	    if (tag.StartsWith("give_")) {
		string itemName = tag.Substring(5);
		this.player.ReceiveItem(itemName);
	    }
	    
	    // take item
	    if (tag.StartsWith("take_")) {
		string itemName = tag.Substring(5);
		this.player.LoseItem(itemName);
	    }
	    */
	}
    }

    // Creates a button showing the choice text
    Button CreateChoiceButton (string text) {
	// Creates the button from a prefab
	Button choice = Instantiate(buttonPrefab) as Button;
	choice.transform.SetParent(this.subCanvas.transform);
		
	// Gets the text from the button prefab
	Text choiceText = choice.GetComponentInChildren<Text>();
	choiceText.text = text;

	// Make the button expand to fit the text
	HorizontalLayoutGroup layoutGroup = choice.GetComponent <HorizontalLayoutGroup> ();
	layoutGroup.childForceExpandHeight = false;
	
	return choice;
    }

    // When we click the choice button, tell the story to choose that choice!
    private void OnClickChoiceButton (Choice choice) {
	this.story.ChooseChoiceIndex(choice.index);
	this.story.Continue();
	RefreshView();
    }

    public void UpdateInventory(List<string> itemNames) {
	for (int i = 0; i < 3; i++) {
	    if (i < itemNames.Count) {
		this.story.variablesState["inventory_" + i.ToString()] = itemNames[i];
	    } else {
		this.story.variablesState["inventory_" + i.ToString()] = "";
	    }
	}
    }
}
