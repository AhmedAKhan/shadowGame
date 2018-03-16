using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objective : MonoBehaviour {
  
  void Start () { 
    GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
    GetComponent<Renderer>().receiveShadows = true;
    SpriteRenderer renderer = GetComponent<SpriteRenderer>();
    Color color = renderer.color;
    color.a = 0.5f;
    renderer.color = color;
  }
  void Update () { }
}
