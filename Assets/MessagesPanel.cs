using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessagesPanel : MonoBehaviour {

	Text m_MessageText;

	// Use this for initialization
	void Start () {
		m_MessageText = transform.FindDeepChild("MessageText").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    internal void AddMessage(string message)
    {
		m_MessageText.text = message;
    }
}
