using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFilmSet : MonoBehaviour
{
  [SerializeField]
  private Camera camera;

  [SerializeField]
  private CharacterSelectionModel model;
  public RenderTexture Texture
  { get; private set; }

  void Awake()
  {
    Texture = RenderTexture.GetTemporary(500, 500);
    camera.targetTexture = Texture;
  }

  void Destroy()
  {
    Texture.Release();
  }

  public void SetCharacter(Character c)
  {
    var cc = Instantiate(c.CharacterSelectionRepresentation, model.transform);
    SetLayerRecursively(cc, gameObject.layer);
  }
  void SetLayerRecursively(GameObject obj, int newLayer)
  {
    if (null == obj)
    {
      return;
    }

    obj.layer = newLayer;

    foreach (Transform child in obj.transform)
    {
      if (null == child)
      {
        continue;
      }
      SetLayerRecursively(child.gameObject, newLayer);
    }
  }

}
