using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistributeUI : MonoBehaviour {

	public static DistributeUI Instance { get; set; }

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

	public string[] m_Countries = new []{"USA", "Japan", "Europe"};


    void Awake() {
		m_Visible = GetComponent<Canvas>().enabled;
		GetComponent<CanvasGroup>().interactable = m_Visible;

		Debug.Assert(Instance == null);
		Instance = this;

		Visible = false;
	}

	// Use this for initialization
	void Start () {
		transform.FindDeepChild("DistributeButton").GetComponent<Button>().onClick.AddListener(OnDistributeButtonClicked);	
		transform.FindDeepChild("CloseButton").GetComponent<Button>().onClick.AddListener(() => { Visible = false; });	

		foreach (var country in m_Countries) {
			GetCountryToggle(country).onValueChanged.AddListener((value) => { UpdateUI(); });
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (Visible && Input.GetKeyDown("escape")) {
			Visible = false;
		}

	}

	void LateUpdate() {
		if (m_Visible != m_ShouldBeVisible) {
			var value = m_ShouldBeVisible;
			GetComponent<Canvas>().enabled = value;
			GetComponent<CanvasGroup>().interactable = value;
			m_Visible = value;
			if (m_Visible) {
				transform.FindDeepChild("DistributeButton").GetComponent<Button>().Select();
			}
		}

	}

	void UpdateUI() {
		var pd = m_Object.GetComponent<CreativeProduct>().ProductData;

		transform.FindDeepChild("IntroText").GetComponent<Text>().text = "Distribute '" + pd.Title + "' to:";
		UpdatePrice();
	}

	void UpdatePrice() {
		var pd = m_Object.GetComponent<CreativeProduct>().ProductData;

		var distributionPriceCalculator = DistributionHQ.Instance;

		var price = 0;
		var desiredMediaSaturationPct = 0.3f;
		foreach (var country in m_Countries) {
			if (IsCountryToggleOn(country)) {
				price += distributionPriceCalculator.GetDistributionPrice(pd, country, desiredMediaSaturationPct);
			}
		}

		transform.FindDeepChild("PriceText").GetComponent<Text>().text = "Cost: " + CurrencyFormatter.Format(price);
	}
	
	void OnDistributeButtonClicked() 
	{
		var pd = m_Object.GetComponent<CreativeProduct>().ProductData;
		var desiredMediaSaturationPct = 0.3f;
		foreach (var country in m_Countries) {
			DistributionHQ.Instance.Distribute(pd, country, desiredMediaSaturationPct);
		}
		Visible = false;
	}

	bool IsCountryToggleOn(string country) {
		return GetCountryToggle(country).isOn;
	}

	Toggle GetCountryToggle(string country) {
		return transform.FindDeepChild(country + "Toggle").GetComponent<Toggle>();
	}

}
