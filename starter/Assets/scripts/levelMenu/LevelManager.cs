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
    /* Game.current = new Game(); */
    /* Game.current.setupEmpty(); */
    /* SaveLoad.Save(); */

    if(!SaveLoad.Load()){
      Game.current = new Game();
      Game.current.setupEmpty();
      //Game.current.setupFull();
    }

    levelMap = new Dictionary<string, LevelData>();
    //string path = Path.Combine(Application.StreamingAssetsPath, gameDataFileName);
    string path = "Assets/Data/levels.json";
    /* print("checking if path exists"); */
    if(File.Exists(path)){
      /* print("path exists"); */
      string dataAsJson = File.ReadAllText(path);
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
      if(child == null){ print("could not find level"); continue; }
      Level lvl = child.GetComponent<Level>();
      print("going to unlock level " + levelName + " lvl: " + lvl);
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
    if(levelMap.ContainsKey(obj.level)) return;
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
