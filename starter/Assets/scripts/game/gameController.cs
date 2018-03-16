using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class gameController: MonoBehaviour {
  public float speed;
  public float rotationSpeed;
  private GameObject currentObject;
  private int selectedChildIndex = 0;
  private Obstacle curObstacle;

  public int levelMenuIndex = 1;


  //public Image target;
  public GameObject objectiveObject;

  // Use this for initialization
  void Start () {

    // update the tutorial image object
    //target.sprite = Resources.Load<Sprite>("sample2");
    setupLevel();

    currentObject = transform.GetChild (0).gameObject;
    curObstacle = currentObject.GetComponent<Obstacle> ();
  }
  private LevelData levelConfig;
  void setupLevel(){
    // load data File
    GameObject lvlDataObj = GameObject.Find("LevelDataObj");
    levelConfig = lvlDataObj.GetComponent<PersistentLevelObj>().data;
    print("levelConfig: " + levelConfig);

    setupObjective();
    setupObstacles();
  }
  void setupObjective(){
    //levelConfig.objectiveName
    Sprite sprite = Resources.Load<Sprite>(levelConfig.objectiveMaterialName);
    objectiveObject.GetComponent<SpriteRenderer>().sprite = sprite;

    /* objectiveObject.transform.position = new Vector3(-0.5f,3.5f,0); */
    objectiveObject.transform.position = new Vector3(
        levelConfig.objPosX,
        levelConfig.objPosY,
        levelConfig.objPosZ);
  }

  //public GameObject prefab;
  void setupObstacles(){
    // load the obstacle files
    print("going to look for files in " + "Assets/Data/Levels/" + levelConfig.name);
    DirectoryInfo dir = new DirectoryInfo("Assets/Data/Levels/" + levelConfig.name);
    FileInfo[] info = dir.GetFiles("*.json");
    foreach (FileInfo f in info) {
      print("found file: " + f);
      // convert json to object
      print("dir.name: " + dir.FullName + " f.name: " + f.Name);
      string dataAsString = File.ReadAllText("Assets/Data/Levels/"+levelConfig.name + "/"+f.Name);
      print("dataAsString: " + dataAsString);
      ObstacleData curObs = JsonUtility.FromJson<ObstacleData>(dataAsString);
      print("read the file " + curObs);
      loadObject(curObs);
    }
  }
  void loadObject(ObstacleData o){
    GameObject prefab = Resources.Load ("prefabs/"+o.prefab) as GameObject;
    print("adding the game object");
    GameObject go = GameObject.Instantiate(prefab, 
        new Vector3(o.x,o.y,o.z), 
        Quaternion.Euler(o.rotX, o.rotY, o.rotZ));
    go.transform.parent = this.transform;
    go.name = o.name;
    Obstacle obstacle = go.GetComponent<Obstacle>();
    obstacle.isRotatable = o.isRotatable;
    obstacle.isMovable = o.isMovable;
    obstacle.posSol = new Vector3(o.solX, o.solY, o.solZ);
    print("setting o.isRotatable: " + o.isRotatable + " o.isMovable: " + o.isMovable);
    print("setting o.x: " + o.x + " o.y: " + o.y + " o.z: " + o.z);
    obstacle.rotateSol = Quaternion.Euler(o.solRotX, o.solRotY, o.solRotZ);
  }

  void FixedUpdate(){
    adjustCurrentObject();
    handleMovement();

    if (checkWin ()) handleWin();
  }

  // ----- win condition ----------
  void handleWin(){
    if(Game.current == null) Game.current = new Game();
    Game.current.levelCompleted(levelConfig);
    SaveLoad.savedGame = Game.current;
    SaveLoad.Save();

    SceneManager.LoadScene(levelMenuIndex);// main menu has an id of 0
  }

  bool checkWin(){
    foreach (Transform child in transform){
      GameObject obj = child.gameObject;
      Obstacle obstacle = obj.GetComponent<Obstacle>();
      if(obstacle.isMovable && !obstaclePositionCorrect(child.GetComponent<Rigidbody>().position, obstacle.posSol)) return false;
      if(obstacle.isRotatable && !obstacleRotationCorrect(child.GetComponent<Rigidbody>().rotation, obstacle.rotateSol)) return false;
    }
    return true;
  }

  private float epsilon = 0.5f;
  bool obstaclePositionCorrect(Vector3 pos, Vector3 posSol){
    print ("pos: " + pos + " posSol: " + posSol);
    print ("diff: " + Vector3.SqrMagnitude (pos - posSol));
    return Vector3.SqrMagnitude(pos - posSol) < epsilon;
  }
  bool obstacleRotationCorrect(Quaternion rot, Quaternion rotSol){
    //print ("diff is actually: " + Quaternion.Angle(rot, rotSol));
    //print ("rot: " + rot);
    //print ("rotSol: " + rotSol);
    return Quaternion.Angle(rot, rotSol) < epsilon;
  }
  // ------- end win condition ----------


  // -------- calculations --------
  void adjustCurrentObject(){
    if (Input.GetKeyUp (KeyCode.Tab)) {
      selectedChildIndex++;
      if(selectedChildIndex >= transform.childCount)
        selectedChildIndex = 0;

      displayObjectSelected(selectedChildIndex);
      currentObject = transform.GetChild(selectedChildIndex).gameObject;
      curObstacle = currentObject.GetComponent<Obstacle> ();
    }
    if (Input.GetMouseButtonDown (0)) updateObjectSelected();

  }
  void updateObjectSelected(){
    if (Input.GetMouseButtonDown(0)){
      RaycastHit hitInfo = new RaycastHit();
      bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
      if (hit && hitInfo.transform.gameObject.tag == "obstacle"){
        Debug.Log ("changing current Object");
        currentObject = hitInfo.transform.gameObject;
        curObstacle = currentObject.GetComponent<Obstacle> ();
      }
    }
  }
  // -------- calculations --------



  // --------- manipulating objects -----------------
  void handleMovement(){
    float hor = Input.GetAxis ("Horizontal");
    float ver = Input.GetAxis ("Vertical");
    if(curObstacle.isRotatable) rotateObject(currentObject.GetComponent<Rigidbody>(), hor, ver);
    if(curObstacle.isMovable) moveObject(currentObject.GetComponent<Rigidbody>(), hor, ver);
  }
  void displayObjectSelected(int selectedChildIndex){
    // TODO uhh highlight the object that is selected somehow or give some indication
  }
  void moveObject(Rigidbody obj, float hor, float ver){
    Vector3 movement = new Vector3 (0.0f, ver, -hor);
    obj.MovePosition(obj.position + movement * speed);
    //Vector3 movement = new Vector3 (moveHorizontal, moveVertical, 0.0f);
    obj.AddForce (movement * speed);

  }
  void rotateObject(Rigidbody obj, float hor, float ver){
    Vector3 movementRotation = new Vector3 (hor, ver, 0.0f) * 100;
    Vector3 m_EulerAngleVelocity = movementRotation * speed * 10; //new Vector3(0, , 0);
    Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * Time.deltaTime);
    obj.MoveRotation(obj.rotation * deltaRotation);
  }
  // --------- manipulating objects -----------------

  // Update is called once per frame
  void Update () {

  }
}
