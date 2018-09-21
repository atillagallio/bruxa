using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
  private Joystick playerJoystick;
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

  private bool switchButton;
  private bool spellButton;

  private bool parryButton;
  // Use this for initialization
  void Start()
  {
    SwitchCooldown = GameDataManager.Data.SwitchCooldown;
  }

  void GetControls()
  {
    switchButton = Input.GetButtonDown(playerJoystick.input.Fire1);
    spellButton = Input.GetButtonDown(playerJoystick.input.Fire3);
    parryButton = Input.GetButtonDown(playerJoystick.input.Fire1);
  }

  public Joystick GetJoystick()
  {
    return playerJoystick;
  }

  public void SetJoystick(Joystick _joystick)
  {
    playerJoystick = _joystick;
  }
  // Update is called once per frame
  void Update()
  {
    GetControls();
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
          EventManager.OnTryingToGetControl(this);
          SwitchCooldown = 0;
          if (InGameManager.Instance.parryActive)
          {
            EventManager.OnFailingToGetControl(this);
            GameUIManager.Instance.StartBlockedAnimation();
            //GameUIManager.Instance.UpdateUISkillCD(gameUiPosition);
            // GameUIManager.Instance.UpdateUISkillCD(gameUiPosition);

          }
          else
          {
            InGameManager.Instance.ChangeCharacterControl(this);
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
    if (spellButton && InGameManager.Instance.HasGameStarted())
    {
      if (Spell != null)
      {
        EventManager.OnPlayerUsingItem(this);
        InGameManager.Instance.PlayerUseSpell(this);
      }
    }
  }

}