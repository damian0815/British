using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

	Text m_FundsText;

	// Use this for initialization
	void Start () {
		m_FundsText = transform.FindDeepChild("FundsText").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		m_FundsText.text = CurrencyFormatter.Format(GameState.Instance.m_Funds);
	}
}
