using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
  public Player RewiredPlayer;
  public Color Color => CharacterInfo.Color;
  public int GameUiPosition;
  public Character CharacterInfo { get; set; }
  public Spell Spell;
  public bool isInControl = false;
  //private List<Spells> spells;
  public float SwitchCooldown = 0;
  public int Points = 0;
  public float parryCoolDown = 0;
  private bool InSwitchCooldown
  {
    get
    {
      return SwitchCooldown <= GameDataManager.Data.SwitchCooldown;
    }
  }

  private bool switchButton => RewiredPlayer.GetButtonDown(RewiredConsts.Action.Switch);
  private bool itemButton => RewiredPlayer.GetButtonDown(RewiredConsts.Action.Item);
  private bool parryButton => RewiredPlayer.GetButtonDown(RewiredConsts.Action.Parry);
  public float VerticalInput => RewiredPlayer.GetAxis(RewiredConsts.Action.MoveVertical);
  public float HorizontalInput => RewiredPlayer.GetAxis(RewiredConsts.Action.MoveHorizontal);

  // Use this for initialization
  void Start()
  {
    SwitchCooldown = GameDataManager.Data.SwitchCooldown;
  }


  // Update is called once per frame
  void Update()
  {
    print(RewiredPlayer.id);
    CheckButtonPress();
    parryCoolDown += Time.deltaTime;
    if (!isInControl)
    {
      SwitchCooldown += Time.deltaTime;
    }
  }

  void CheckButtonPress()
  {
    if (!InGameManager.Instance.HasGameStarted()) return;
    if (!isInControl)
    {
      if (switchButton)
      {
        if (!InSwitchCooldown && !InGameManager.Instance.spell1Lock)
        {
          SwitchCooldown = 0;
          if (InGameManager.Instance.parryActive)
          {
            GameUIManager.Instance.StartBlockedAnimation();
            //GameUIManager.Instance.UpdateUISkillCD(gameUiPosition);
            // GameUIManager.Instance.UpdateUISkillCD(gameUiPosition);

          }
          else
          {
            InGameManager.Instance.gameCharacter.ChangeCharacterControl(this);
            //inblockChangeSkillCooldown = false;
          }
        }
      }
    }
    else if (parryButton)
    {
      if (parryCoolDown >= GameDataManager.Data.ParryCooldown)
      {
        InGameManager.Instance.UseChangeBlockSkill(this);
        parryCoolDown = 0;
      }
    }
    if (itemButton && InGameManager.Instance.HasGameStarted())
    {
      if (Spell != null)
      {
        InGameManager.Instance.PlayerUseSpell(this);
      }
    }
  }

}