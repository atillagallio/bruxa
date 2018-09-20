using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFilmSet : MonoBehaviour
{
  [SerializeField]
  private Camera camera;
  public RenderTexture Texture;

  void Awake()
  {
    Texture = RenderTexture.GetTemporary(500, 500);
    camera.targetTexture = Texture;
  }

  void Destroy()
  {
    Texture.Release();
  }

  void Update()
  {

  }
}
