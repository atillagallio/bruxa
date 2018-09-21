using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterSelectionScreen
{
  public class UIFilmSet : MonoBehaviour
  {
    [SerializeField]
    private CharSelectCam camera;

    [SerializeField]
    private CharacterSelectionModel model;

    public void SetCameraEffect(bool v)
    {
      camera.IsSelected = v;
    }

    public void ToggleSelect()
    {
      model.ToggleSelect();
    }

    public RenderTexture Texture
    { get; private set; }

    void Awake()
    {
      Texture = RenderTexture.GetTemporary(500, 500);
      camera.cam.targetTexture = Texture;
    }

    void Destroy()
    {
      Texture.Release();
    }

    public void SetCharacter(Character c)
    {
      var cc = Instantiate(c.CharacterSelectionRepresentation, model.transform);
      SetLayerRecursively(cc.gameObject, gameObject.layer);
      model.Character = cc;
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
}
