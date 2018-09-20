using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using System.Linq;

namespace CharacterSelectionScreen
{
  [System.Serializable]
  internal class CharacterSelection
  {
    public Character Character;
    public bool Selected = false;

    public UIFilmSet FilmSet;

    public CharacterSelection(Character c)
    {
      Character = c;
    }
  }

  internal class CharacterSelectionScreenManager : MonoBehaviour
  {
    public const int MAX_PLAYERS = 4;
    private CharacterSelectSlot[] playerSlots = new CharacterSelectSlot[MAX_PLAYERS];

    private List<CharacterSelection> Characters;

    [SerializeField]
    private UIFilmSet FilmSetPrefab;

    [Header("UI")]
    [SerializeField]
    private CharacterSelectSlot SlotPrefab;

    [SerializeField]
    private RectTransform CharactersSlotsUI;

    // Use this for initialization
    void Awake()
    {
      var rawCharacters = Resources.LoadAll<Character>("Characters");
      Characters = rawCharacters.Select((r) => new CharacterSelection(r)).ToList();

      for (int i = 0; i < playerSlots.Length; i++)
      {
        var slot = Instantiate(SlotPrefab, CharactersSlotsUI);
        slot.SlotNum = i;
        slot.Characters = Characters;
      }
    }

    bool HasPlayerJoined(int rewired)
    {
      return playerSlots.Any(s => s.RewiredPlayerId == rewired);
    }

    void PrintSlots()
    {
      foreach (var s in playerSlots)
      {
        print($"Slot {s.SlotNum} : {s.RewiredPlayerId}");
      }
    }

    void Update()
    {
      { // Joining Logic
        for (int i = 0; i < ReInput.players.playerCount; i++)
        {
          var player = ReInput.players.GetPlayer(i);
          if (HasPlayerJoined(i)) continue;
          if (player.GetButtonDown(RewiredConsts.Action.UISubmit))
          {
            PlayerJoin(i);
          }
        }
        /* 
        foreach (var slot in playerSlots)
        {
          if (!slot.Occupied) continue;
          var player = ReInput.players.GetPlayer(slot.RewiredPlayerId);
          if (player.GetButtonDown(RewiredConsts.Action.UICancel))
          {
            PlayerLeave(slot.RewiredPlayerId);
          }
        }*/
      }
    }

    void PlayerJoin(int rewired)
    {
      for (int i = 0; i < playerSlots.Length; i++)
      {
        var slot = playerSlots[i];
        if (slot.State == SlotState.Free)
        {
          slot.Join(rewired);
          return;
        }
      }
      print($"Nao há slots pro player {rewired}");
    }
  }
}