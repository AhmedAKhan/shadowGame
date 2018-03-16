
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Game {
	public static Game current;

	public List<string> completedLevels;

	public Game () {
		completedLevels = new List<string>();
	}
    public void setupEmpty(){
        completedLevels = new List<string>();
        completedLevels.Add("tutorial1");
    }

	public void levelCompleted(int level){
		// update variables to indicate the the level "level" has been completed
		MonoBehaviour.print("the user has completed the level: " + level);
	}
}
