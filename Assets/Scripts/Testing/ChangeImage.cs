using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeImage : MonoBehaviour {

	// Use this for initialization
	GameState gameState; 
	SpriteRenderer[] spriteRendererArray;
	void Awake()
	{
		gameState = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameState> ();
	}
	void Start () 
	{
		spriteRendererArray = this.GetComponentsInChildren<SpriteRenderer> ();
		foreach (SpriteRenderer renderer in spriteRendererArray) {
			renderer.enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		Debug.Log ("Game Result:" + GameState.setResult);
		foreach(SpriteRenderer renderer in spriteRendererArray){
			if (GameState.setResult && renderer.sprite.name == "correct1") 
			{
				renderer.enabled = true;
				Debug.Log ("correct 1");
			}else if (!GameState.setResult && renderer.sprite.name =="incorrect1")
			{
				Debug.Log ("incorrect 1");
				renderer.enabled = true;
			}else{
				renderer.enabled = false;
			}

		}
		gameState.transistionStart (true);

		
	}
}
