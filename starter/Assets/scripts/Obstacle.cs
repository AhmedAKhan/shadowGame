using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {
  public bool isRotatable;
  public bool isMovable;
  public Quaternion rotateSol = new Quaternion(0,0,0, 0);
  public Vector3 posSol = new Vector3(0,0,0);
}
