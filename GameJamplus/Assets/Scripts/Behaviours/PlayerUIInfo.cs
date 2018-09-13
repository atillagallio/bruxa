using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PlayerUIInfo : MonoBehaviour
{
  public Image CooldownFillImg;
  public Image HeroBG;
  public Image HeroImg;
  public Image ItemImg;
  public TextMeshProUGUI Points;

  public PlayerBehaviour Player;
  // Use this for initialization
  void Start()
  {
  }
  // Update is called once per frame
  void Update()
  {
    Points.text = "" + Player.points;
  }
}
