using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using System.Linq;
using UnityEngine.UI;

namespace CharacterSelectionScreen
{
  [System.Serializable]
  internal class CharacterSelection
  {
    public Character Character;
    public bool Selected = false;

    public UIFilmSet FilmSetSelecting;
    public UIFilmSet FilmSetSelected;

    public CharacterSelection(Character c, UIFilmSet selecting, UIFilmSet selected)
    {
      Character = c;
      FilmSetSelecting = selecting;
      FilmSetSelected = selected;
    }
  }

  internal class CharacterSelectionScreenManager : MonoBehaviour
  {
    public const int MAX_PLAYERS = 4;
    public const int MIN_PLAYERS = 2;
    private CharacterSelectSlot[] playerSlots = new CharacterSelectSlot[MAX_PLAYERS];
    private bool AllPlayersReady =>
      playerSlots.Where((p) => p.State == SlotState.Joined).Count() == 0 &&
      playerSlots.Where((p) => p.State == SlotState.Ready).Count() >= MIN_PLAYERS;

    private float MaxPressTime =>
      playerSlots.Where(p => p.RewiredPlayerId != -1).Select(p => ReInput.players.GetPlayer(p.RewiredPlayerId).GetButtonTimePressed(RewiredConsts.Action.UISubmit)).Max();

    private List<CharacterSelection> Characters;
    [SerializeField]
    private string SceneAfterCharacterSelectionName;

    [SerializeField]
    private float holdToStartTime;
    [Header("Film Set")]
    [SerializeField]
    private UIFilmSet FilmSetPrefab;
    [SerializeField]
    private float Offset;

    [Header("UI")]
    [SerializeField]
    private CharacterSelectSlot SlotPrefab;

    [SerializeField]
    private RectTransform CharactersSlotsUI;
    [SerializeField]
    private Image StartTimerUI;

    // Use this for initialization
    void Awake()
    {
      {
        var rawCharacters = Resources.LoadAll<Character>("Characters");
        int i = 0;
        Characters = rawCharacters.Select((r) =>
        {
          var filmSetSelecting = Instantiate(FilmSetPrefab, transform);
          filmSetSelecting.transform.position += i * Offset * Vector3.right;
          filmSetSelecting.SetCharacter(r);
          filmSetSelecting.SetCameraEffect(false);

          var filmSetSelected = Instantiate(FilmSetPrefab, transform);
          filmSetSelected.transform.position += i * Offset * Vector3.right;
          filmSetSelected.transform.position += Offset * Vector3.up;
          filmSetSelected.SetCharacter(r);
          filmSetSelected.SetCameraEffect(true);

          i++;
          return new CharacterSelection(r, filmSetSelecting, filmSetSelected);

        }).ToList();
      }

      for (int i = 0; i < playerSlots.Length; i++)
      {
        var slot = Instantiate(SlotPrefab, CharactersSlotsUI);
        slot.SlotNum = i;
        slot.Characters = Characters;
        playerSlots[i] = slot;
      }
      SlotPrefab.gameObject.SetActive(false);
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


    //private IEnumerator HoldToStart

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
        if (AllPlayersReady)
        {
          StartTimerUI.gameObject.SetActive(true);
          var k = MaxPressTime / holdToStartTime;
          StartTimerUI.fillAmount = Mathf.Clamp01(k);
          if (!startedGame && k > 1)
          {
            // Start Game
            startedGame = true;
            JoinedPlayersContainer.SetPlayingPlayers(
              playerSlots
                .Where(p => p.State == SlotState.Ready)
                .Select(p => new JoinedPlayerData(p.Player, p.CurrentCharacter.Character))
                .ToList()
            );
            UnityEngine.SceneManagement.SceneManager.LoadScene(SceneAfterCharacterSelectionName);
          }
        }
        else
        {
          StartTimerUI.gameObject.SetActive(false);
        }
      }
    }

    private bool startedGame;

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