using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class CharSelectCam : MonoBehaviour
{

  [SerializeField]
  private Material mat;
  [SerializeField]
  private Shader shader;

  [SerializeField]
  public bool IsSelected;

  public Camera cam;


  // Use this for initialization
  void Awake()
  {
    mat = new Material(shader);
  }

  void OnRenderImage(RenderTexture src, RenderTexture dst)
  {
    if (IsSelected)
    {
      Graphics.Blit(src, dst, mat);
    }
    else
    {
      Graphics.Blit(src, dst);
    }
  }
}
