using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData {
  public string level;
  public string name;
  public int number;
  public int parent;

  public string scene;
  public string[] nextLevels;

  public float objPosX;
  public float objPosY;
  public float objPosZ;
  public string objectiveMaterialName;
}

