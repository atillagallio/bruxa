using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverUIManager : MonoBehaviour {

	// Use this for initialization

	public TextMeshProUGUI playerNameText;
	public TextMesh pointsText;

	public List<SpriteRenderer> witches;

	public Fade fade;

	public List<Sprite> witchesWinners;
	void Start () {
		fade.StartImageFade();
		PlayerBehaviour ed = EndGame.Instance.winner;
		playerNameText.text = "Player " + (ed.gameUiPosition+1);
		pointsText.text = ed.points.ToString();
		witches[ed.gameUiPosition].sprite = witchesWinners[ed.gameUiPosition];
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
