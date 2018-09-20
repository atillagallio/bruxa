using System;
using System.Collections;
using System.Collections.Generic;
using Rewired;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CharacterSelectionScreen
{
  internal class CharacterSelectSlot : MonoBehaviour
  {
    // Use this for initialization
    public int RewiredPlayerId { get; private set; } = -1;
    public int SlotNum;
    public bool Occupied = false;
    private int TotalWitches => Characters.Count;
    private CharacterSelection CurrentCharacter => Characters[currentCharacterIndex];
    private bool CharacterFree => !Characters[currentCharacterIndex].Selected;
    private int currentCharacterIndex = 0;

    [HideInInspector]
    public SlotState State;

    [HideInInspector]
    public List<CharacterSelection> Characters;


    [Header("UI Elements")]
    [SerializeField]
    private TextMeshProUGUI CharacterNameUI;

    [SerializeField]
    private RawImage CharacterViewUI;

    void Start()
    {
      State = SlotState.Free;
    }

    // Update is called once per frame
    void Update()
    {
      { // UI Update
        foreach (var el in GetComponentsInChildren<CharacterSelectionElement>(true))
        {
          el.gameObject.SetActive(el.StatesToDisplay.HasFlag(State));
        }
        if (State != SlotState.Free)
        {
          CharacterNameUI.text = CurrentCharacter.Character.Nome;
          CharacterNameUI.fontStyle = CharacterFree ? FontStyles.Normal : FontStyles.Strikethrough;
          CharacterViewUI.texture = CurrentCharacter.FilmSet.Texture;
        }
      }

      // Input Update
      {
        if (State == SlotState.Free) return;
        var player = ReInput.players.GetPlayer(RewiredPlayerId);


        if (player.GetButtonDown(RewiredConsts.Action.UISubmit) && CharacterFree)
        {
          // Ready
          ChangeState(SlotState.Ready);
          CurrentCharacter.Selected = true;
        }

        if (State == SlotState.Joined)
        {
          if (player.GetButtonDown(RewiredConsts.Action.UIHorizontal))
          {
            currentCharacterIndex = (currentCharacterIndex + TotalWitches + 1) % TotalWitches;
          }
          if (player.GetNegativeButtonDown(RewiredConsts.Action.UIHorizontal))
          {
            currentCharacterIndex = (currentCharacterIndex + TotalWitches - 1) % TotalWitches;
          }
          if (player.GetButtonDown(RewiredConsts.Action.UICancel))
          {
            // Leave
            RewiredPlayerId = -1;
            ChangeState(SlotState.Free);
          }
        }

        if (State == SlotState.Ready)
        {
          if (player.GetButtonDown(RewiredConsts.Action.UICancel))
          {
            // UnReady
            ChangeState(SlotState.Joined);
            CurrentCharacter.Selected = false;
          }
        }

      }


    }

    public void Join(int rewiredId)
    {
      RewiredPlayerId = rewiredId;
      ChangeState(SlotState.Joined);
      print($"O player {rewiredId} deu join no slot {SlotNum}");
    }

    void ChangeState(SlotState nextState)
    {
      StartCoroutine(NextFrameState(nextState));
    }

    IEnumerator NextFrameState(SlotState nextState)
    {
      yield return new WaitForEndOfFrame();
      State = nextState;
    }
  }
}