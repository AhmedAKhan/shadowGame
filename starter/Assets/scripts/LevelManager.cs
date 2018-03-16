using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
  private	GameObject selectedObject;
  private string gameDataFileName = "levels.json";
  private string[] levelNames;
  private Dictionary<string, LevelData> levelMap;
  void Start () {
    if(!SaveLoad.Load()){ // TEMP done in main menu
      Game.current = new Game();
      Game.current.setupEmpty();
    }

    levelMap = new Dictionary<string, LevelData>();
    //string path = Path.Combine(Application.StreamingAssetsPath, gameDataFileName);
    string path = "Assets/Data/levels.json";
    /* print("checking if path exists"); */
    if(File.Exists(path)){
      /* print("path exists"); */
      string dataAsJson = File.ReadAllText("Assets/Data/levels.json");
      AllLevels loadedData = JsonUtility.FromJson<AllLevels>(dataAsJson);
      levelNames = loadedData.levels;
    }else{
      Debug.LogError("Cannot load level data");
    }

    unlockLevels();
  }
  void unlockLevels(){
    print("starting unlockLevels");
    foreach(string levelName in Game.current.completedLevels){
      print("looking for levelName: " + levelName);
      Transform child = transform.Find(levelName);
      if(child == null) continue;
      Level lvl = child.GetComponent<Level>();
      lvl.unlockObject();
    }
  }
  void changeScene(){
    Level obj = selectedObject.GetComponent<Level> ();
    Debug.Log("the level you clicked on is " + obj.level);
    if(obj.isLocked){
      print("level is locked");
      return;
    }
    // get level 
    string key = obj.level;
    print("changeScene - going to look for key: " + key);
    LevelData lvl = levelMap[key];
    print("changeScene - lvl: " + lvl);

    // update level data object
    GameObject lvlDataObj = GameObject.Find("LevelDataObj");
    PersistentLevelObj levelScript = lvlDataObj.GetComponent<PersistentLevelObj>();
    levelScript.data = lvl;

    print("changeScene - going to change to level " + lvl.scene);
    SceneManager.LoadScene (lvl.scene);
  }
  void Update(){
    updateObjectSelected();
  }
  void loadLevelInformation(){
    Level obj = selectedObject.GetComponent<Level> ();
    Debug.Log("loadLevel - the level you clicked on is " + obj.level);
    string levelPath = "Assets/Data/"+obj.level+".json";
    if(File.Exists(levelPath)){
      print("loadLevel -path exists");
      string dataAsJson = File.ReadAllText(levelPath);
      LevelData levelData = JsonUtility.FromJson<LevelData>(dataAsJson);
      print("loadLevel - going to add levelData to level: " + levelData);
      print("loadLevel - levelData.name: " + levelData.name);
      if(levelData == null) print("loadLevel - levelData is null");
      levelMap.Add(obj.level, levelData);
    }else{
      Debug.LogError("loadLevel - Cannot load level data");
    }
  }
  void updateObjectSelected(){
    if (Input.GetMouseButtonDown(0)){
      RaycastHit hitInfo = new RaycastHit();
      bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
      if (hit && hitInfo.transform.gameObject.tag == "Level"){
        if(hitInfo.transform.gameObject == selectedObject){
          changeScene();
          return;
        }
        selectedObject = hitInfo.transform.gameObject;
        loadLevelInformation();
      }
    }
  }
}
