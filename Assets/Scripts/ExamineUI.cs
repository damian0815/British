﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class ExamineUI : MonoBehaviour {

	public static ExamineUI Instance { get; private set; }

	public GameObject Object { 
		get {
			return m_Object;
		}
		set {
			if (m_Object != value) {
				m_Object = value;
				UpdateUI();
			}
		}
	}
	private GameObject m_Object;

	public bool Visible {
		get {
			return m_Visible;
		}
		set {
			m_ShouldBeVisible = value;
		}
	}
	private bool m_Visible = false;
	private bool m_ShouldBeVisible = false;

	void Awake() {
		m_Visible = GetComponent<Canvas>().enabled;
		GetComponent<CanvasGroup>().interactable = m_Visible;

		Debug.Assert(Instance == null);
		Instance = this;

		Visible = false;
	}

	// Use this for initialization
	void Start () {
		
		transform.FindDeepChild("CloseButton").GetComponent<Button>().onClick.AddListener(() => {Visible = false;});
	}
	
	// Update is called once per frame
	void Update () {
		if (Visible && CrossPlatformInputManager.GetButtonDown("Cancel")) {
			Visible = false;
		}
	}

	void LateUpdate() {
		var value = m_ShouldBeVisible;
		if (m_Visible != value) {
			GetComponent<Canvas>().enabled = value;
			GetComponent<CanvasGroup>().interactable = value;
			m_Visible = value;
			if (m_Visible) {
				transform.FindDeepChild("CloseButton").GetComponent<Button>().Select();
			}
		}

	}

	void UpdateUI() {
		if (m_Object == null) {
			return;
		} 
		var pd = m_Object.GetComponent<CreativeProduct>().ProductData;

		transform.FindDeepChild("Title").GetComponent<Text>().text = "Title: " + pd.Title;
		transform.FindDeepChild("ColourSwatch").GetComponent<Image>().color = pd.Color;
		transform.FindDeepChild("Topic").GetComponent<Text>().text = "Topic: " + pd.Topic;
		transform.FindDeepChild("Flavor").GetComponent<Text>().text = "Flavor: " + pd.Flavor;
	}


}

