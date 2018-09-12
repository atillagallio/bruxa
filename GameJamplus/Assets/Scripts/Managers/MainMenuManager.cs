using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuManager : MonoBehaviour
{

  public TextMeshPro playerCount;
  public List<GameObject> controllers;

  public int numberOfControllers = 0;

  public List<string> realJoysticks;
  // Use this for initialization
  void Start()
  {
    PopulateRealJoysticks();

  }

  // Update is called once per frame
  void TurnOffAllControllers()
  {
    foreach (GameObject controller in controllers)
    {
      controller.SetActive(false);
    }
  }

  void PopulateRealJoysticks()
  {
    realJoysticks = new List<string>();
    foreach (string name in Input.GetJoystickNames())
    {
      if (name != "")
        realJoysticks.Add(name);
    }
  }
  void Update()
  {
    int i = 0;
    if (Input.GetButtonDown("Submit"))
    {
      UnityEngine.SceneManagement.SceneManager.LoadScene("MainGameplay");
    }
    if (numberOfControllers != realJoysticks.Count)
    {
      TurnOffAllControllers();
      foreach (string name in realJoysticks)
      {
        if (i <= 3)
        {
          controllers[i].SetActive(true);
          i++;
        }
      }
      playerCount.text = i + " Players";
    }
  }
}
