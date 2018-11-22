using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExamineUI : MonoBehaviour {

	public static ExamineUI Instance { get; private set; }

	public GameObject Object { 
		get {
			return m_Object;
		}
		set {
			m_Object = value;
			UpdateUI();
		}
	}
	private GameObject m_Object;

	public bool Visible {
		get {
			return m_Visible;
		}
		set {
			if (m_Visible != value) {
				GetComponent<Canvas>().enabled = value;
				m_Visible = value;
				m_DebounceInput = true;
			}
		}
	}
	private bool m_Visible = false;
	private bool m_DebounceInput = false;

	void Awake() {
		m_Visible = GetComponent<Canvas>().enabled;

		Debug.Assert(Instance == null);
		Instance = this;

		Visible = false;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (m_DebounceInput) {
			m_DebounceInput = false;
		} else {
			if (Visible && Input.anyKeyDown) {
				Visible = false;
			}
		}

	}

	void UpdateUI() {
		var pd = m_Object.GetComponent<CreativeProduct>().ProductData;

		transform.FindDeepChild("Title").GetComponent<Text>().text = "Title: " + pd.Title;
		transform.FindDeepChild("ColourSwatch").GetComponent<Image>().color = pd.Color;
		transform.FindDeepChild("Topic").GetComponent<Text>().text = "Topic: " + pd.Topic;
		transform.FindDeepChild("Flavour").GetComponent<Text>().text = "Flavour: " + pd.Flavour;
	}


}

