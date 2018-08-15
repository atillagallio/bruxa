using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{

    private Joystick playerJoystick;
    private Color playerColor;

    public int gameUiPosition;

    public Spell spell;

    public bool isInControl = false;
    //private List<Spells> spells;
    public float switchCooldown = 0;
    private bool InSwitchCooldown
    {
        get
        {
            return switchCooldown <= ConfigurationTestBruxaManager.Instance.switchCooldown;
        }
    }
    public float parryCoolDown = 0;
    private bool switchButton;
    private bool spellButton;

    public int points = 0;
    private bool parryButton;
    // Use this for initialization
    void Start()
    {
        switchCooldown = ConfigurationTestBruxaManager.Instance.switchCooldown;
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

    public Color GetColor()
    {
        return playerColor;
    }
    public void SetPlayerInfo(Joystick _joystick, Color _color)
    {
        playerJoystick = _joystick;
        playerColor = _color;
    }
    // Update is called once per frame
    void Update()
    {
        GetControls();
        CheckButtonPress();

        parryCoolDown += Time.deltaTime;
        if (!isInControl)
        {
            switchCooldown += Time.deltaTime;
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
                    switchCooldown = 0;
                    if (InGameManager.Instance.parryActive)
                    {
                        GameUIManager.Instance.StartBlockedAnimation();
                        Debug.Log("PLAYER UI POS ->" + gameUiPosition);
                        //GameUIManager.Instance.UpdateUISkillCD(gameUiPosition);
                        // GameUIManager.Instance.UpdateUISkillCD(gameUiPosition);

                    }
                    else
                    {
                        Debug.Log("player " + playerJoystick.name + "Trying to get control");
                        InGameManager.Instance.ChangeCharacterControl(this);
                        //inblockChangeSkillCooldown = false;
                    }
                }
            }
        }
        else if (parryButton)
        {
            if (parryCoolDown >= ConfigurationTestBruxaManager.Instance.parryCooldown)
            {
                InGameManager.Instance.UseChangeBlockSkill(this);
                parryCoolDown = 0;
            }
        }
        if (spellButton && InGameManager.Instance.HasGameStarted())
        {
            if (spell != null)
            {
                InGameManager.Instance.PlayerUseSpell(this);
            }
        }
    }

}
