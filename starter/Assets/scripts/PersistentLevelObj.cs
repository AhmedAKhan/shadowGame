using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentLevelObj : MonoBehaviour {

	public LevelData data;

	void Awake() {
		DontDestroyOnLoad(this);
	}

	// Update is called once per frame
	void Update () {

	}
}
