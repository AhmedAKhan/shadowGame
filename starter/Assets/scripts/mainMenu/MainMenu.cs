using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (Game.current == null){
			bool res = SaveLoad.Load();
			if(!res) Game.current = new Game();
		}
	}

	// Update is called once per frame
	void Update () {

	}
}
