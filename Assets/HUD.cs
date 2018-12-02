using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

	Text m_FundsText;
	MessagesPanel m_MessagesPanel;

	// Use this for initialization
	void Start () {
		m_FundsText = transform.FindDeepChild("FundsText").GetComponent<Text>();
		m_MessagesPanel = transform.FindDeepChild("MessagesPanel").GetComponent<MessagesPanel>();

		DistributionHQ.Instance.DistributionFinished += OnDistributionFinished;
	}


    // Update is called once per frame
    void Update () {
		m_FundsText.text = CurrencyFormatter.Format(GameState.Instance.m_Funds);
	}

    private void OnDistributionFinished(object sender, DistributionHQ.DistributionFinishedEventArgs e) {
		var description = "'" + e.Data.productData.Title + "' finished distribution to " + e.Data.country + ": revenue " + CurrencyFormatter.Format(e.Revenue);
		AddMessage(description);
    }

	private void AddMessage(string message) 
	{
		m_MessagesPanel.AddMessage(message);
	}
}
