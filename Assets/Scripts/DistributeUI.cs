using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
			if (value != m_Object) {
				m_Object = value;
				UpdateUI();
			}
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
		ButtonStyling.StyleAllChildButtons(transform);

		transform.FindDeepChild("DistributeButton").GetComponent<Button>().onClick.AddListener(OnDistributeButtonClicked);	
		transform.FindDeepChild("CloseButton").GetComponent<Button>().onClick.AddListener(() => { Visible = false; });	

		foreach (var country in m_Countries) {
			GetCountryToggle(country).onValueChanged.AddListener((value) => { UpdatePrice(); });
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
			UpdateUI();
			GetComponent<Canvas>().enabled = value;
			GetComponent<CanvasGroup>().interactable = value;
			m_Visible = value;
			if (m_Visible) {
				var firstEnabledCountry = m_Countries.FirstOrDefault(c => GetCountryToggle(c).enabled);
				if (firstEnabledCountry != null) {
					GetCountryToggle(firstEnabledCountry).Select();
				} else {
					transform.FindDeepChild("CloseButton").GetComponent<Button>().Select();
				}
			}
		}

	}

	void UpdateUI() {
		var pd = m_Object.GetComponent<CreativeProduct>().ProductData;

		transform.FindDeepChild("IntroText").GetComponent<Text>().text = "Distribute '" + pd.Title + "' to:";
		var dhq = DistributionHQ.Instance;
		foreach (var country in m_Countries) {
			var hasDistributedToThisCountry = dhq.HasDistributed(pd, country);
			var toggle = GetCountryToggle(country);
			toggle.isOn = false;
			toggle.enabled = !hasDistributedToThisCountry;
			var colors = toggle.colors;
			if (toggle.enabled) {
				colors.normalColor = Color.white;
				colors.highlightedColor = Color.HSVToRGB(0, 0, 0.8f);
				toggle.transform.Find("Label").GetComponent<Text>().color = Color.HSVToRGB(0, 0, 0.2f);
			} else {
				colors.normalColor = Color.gray;
				colors.highlightedColor = Color.grey;
				toggle.transform.Find("Label").GetComponent<Text>().color = Color.grey;
			}
			toggle.colors = colors;
		}

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
			if (IsCountryToggleOn(country)) {
				DistributionHQ.Instance.Distribute(pd, country, desiredMediaSaturationPct);
			}
		}
		Visible = false;
	}

	bool IsCountryToggleOn(string country) {
		var ct = GetCountryToggle(country);
		return ct.isOn && ct.enabled;
	}

	Toggle GetCountryToggle(string country) {
		return transform.FindDeepChild(country + "Toggle").GetComponent<Toggle>();
	}

}
