using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIScore : MonoBehaviour
{
  private struct ScorePlayerThing
  {
    public PlayerBehaviour Player;
    public Image line;
  }
  [SerializeField]
  private Image filler;
  [SerializeField]
  private Image linePrefab;
  private float height;
  private List<ScorePlayerThing> playerThings = new List<ScorePlayerThing>();
  private List<PlayerBehaviour> _players;
  public List<PlayerBehaviour> Players
  {
    get
    {
      return _players;
    }
    set
    {
      playerThings = value.Select((p) =>
      {
        var line = Instantiate(linePrefab, this.transform);
        line.color = p.Color;
        //line.rectTransform.anchoredPosition = Vector2.down;
        return new ScorePlayerThing()
        {
          Player = p,
          line = line
        };
      }).ToList();
      _players = value;
    }
  }

  // Use this for initialization
  void Start()
  {
    height = filler.rectTransform.sizeDelta.y;
    filler.rectTransform.sizeDelta = new Vector2(filler.rectTransform.sizeDelta.x, 0);
  }

  float PointsToSize(int Points)
  {
    var p = (float)Points / GameDataManager.Data.EndCondition.ScoreToWin;
    return height * p;
  }

  // Update is called once per frame
  void Update()
  {
    int maxScore = 0;
    playerThings.ForEach((pt) =>
    {
      var p = pt.Player;
      if (maxScore < p.Points)
      {
        maxScore = p.Points;
        filler.color = p.CharacterInfo.Color;
        filler.rectTransform.sizeDelta = new Vector2(filler.rectTransform.sizeDelta.x, PointsToSize(p.Points));
      }
      pt.line.rectTransform.anchoredPosition = new Vector2(0, PointsToSize(p.Points));
    });
  }
}
