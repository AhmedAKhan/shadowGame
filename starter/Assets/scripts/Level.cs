using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {
  public string level;
  public int number;
  public bool isLocked;
  public Material unlockedTexture;
  void Awake(){
    print("setting locked = true, name: " +name);
    isLocked = true;
  }
  public void unlockObject(){
    print("unlocking object");
    GetComponent<Renderer>().material = unlockedTexture;
    isLocked = false;
    print("set isLocked = false name: " + name);
  }
}
