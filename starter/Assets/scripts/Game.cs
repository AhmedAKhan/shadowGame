
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
  public void setupFull(){
    completedLevels = new List<string>();
    completedLevels.Add("tutorial1");
    completedLevels.Add("tutorial2");
    completedLevels.Add("tutorial3");
    completedLevels.Add("tutorial4");
    completedLevels.Add("tutorial5");
    completedLevels.Add("tutorial6");
    completedLevels.Add("tutorial7");
    completedLevels.Add("tutorial8");
  }

  public void levelCompleted(LevelData levelConfig){
    // update variables to indicate the the level "level" has been completed
    MonoBehaviour.print("the user has completed the level");
    MonoBehaviour.print("going to add next levels: " + levelConfig.nextLevels);
    foreach(string newLevel in levelConfig.nextLevels){
      MonoBehaviour.print("new level: " + newLevel);
      completedLevels.Add(newLevel);
    }
  }
}
