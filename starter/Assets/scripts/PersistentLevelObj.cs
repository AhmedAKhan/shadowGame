using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentLevelObj : MonoBehaviour {

  public LevelData data;
  private static PersistentLevelObj selfInstance;

  void Awake() {
    DontDestroyOnLoad(this);

    if (selfInstance == null) {
        selfInstance = this;
    } else {
        DestroyObject(gameObject);
    }
  }

  // Update is called once per frame
  void Update () {

  }
} 
