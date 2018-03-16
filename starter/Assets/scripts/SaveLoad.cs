
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoad {
  public static Game savedGame = null;

  public static void Save() {
    if (savedGame == null) return;
    savedGame = Game.current;

    // store the game object to the savedGame.gd file
    BinaryFormatter bf = new BinaryFormatter();
    FileStream file = File.Create (Application.persistentDataPath + "/savedGame.gd");
    bf.Serialize(file, SaveLoad.savedGame);
    file.Close();
    //print("file has been stored");
  }
  public static bool Load(){
    Debug.Log("load starting the function");
    if(File.Exists(Application.persistentDataPath + "/savedGame.gd")) {
      Debug.Log("load inside if statement");
      BinaryFormatter bf = new BinaryFormatter();
      FileStream file = File.Open(Application.persistentDataPath + "/savedGame.gd", FileMode.Open);
      SaveLoad.savedGame = (Game)bf.Deserialize(file);
      file.Close();
      Game.current = savedGame;
      return true;
    } else{
      Debug.Log("nothing to load");
      return false;
    }
  }
}
