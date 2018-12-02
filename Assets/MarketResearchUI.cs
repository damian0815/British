using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class MarketResearchUI : MonoBehaviour {

	public static MarketResearchUI Instance { get; private set; }

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

	private string ActiveCountry { get { return m_ActiveCountry; } set { m_ActiveCountry = value; UpdateUI(); } }
	private string m_ActiveCountry;

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

		transform.FindDeepChild("CloseButton").GetComponent<Button>().onClick.AddListener(() => Visible = false);
		//transform.FindDeepChild("NextButton").GetComponent<Button>().onClick.AddListener(GoToNextCountry);
		//transform.FindDeepChild("PrevButton").GetComponent<Button>().onClick.AddListener(GoToPrevCountry);
		ActiveCountry = CountryRegistry.Instance.AllCountryNames.First();
	}
	
	// Update is called once per frame
	void Update () {

		if (!Visible) {
			return;
		}
		
		if (CrossPlatformInputManager.GetButtonDown("Cancel")) {
			Visible = false;
		}
		if (Input.GetKeyDown(KeyCode.LeftArrow)) {
			GoToPrevCountry();
		} 
		if (Input.GetKeyDown(KeyCode.RightArrow)) {
			GoToNextCountry();
		}

	}

	void UpdateUI() {
		if (Object == null) {
			return;
		}
		var pd = Object.GetComponent<CreativeProduct>().ProductData;
		transform.FindDeepChild("IntroText").GetComponent<Text>().text = string.Format("Market suitability for {0} in {1}:", pd.Title, ActiveCountry);
		transform.FindDeepChild("DetailsText").GetComponent<Text>().text = MarketResearchHQ.Instance.DescribeSuitability(pd, ActiveCountry);

		var order = transform.FindDeepChild("PageCountText").GetComponent<Text>().text = string.Format("{0}/{1}", CountryRegistry.Instance.AllCountryNames.ToList().IndexOf(ActiveCountry) + 1, CountryRegistry.Instance.AllCountryNames.Count());
	}

	void LateUpdate() {
		if (m_Visible != m_ShouldBeVisible) {
			var value = m_ShouldBeVisible;
			GetComponent<Canvas>().enabled = value;
			GetComponent<CanvasGroup>().interactable = value;
			m_Visible = value;
			if (m_Visible) {
				transform.FindDeepChild("CloseButton").GetComponent<Button>().Select();
			}
		}

	}


	void GoToNextCountry() {
		var nextIndex = CountryRegistry.Instance.AllCountryNames.ToList().IndexOf(ActiveCountry) + 1;
		if (nextIndex >= CountryRegistry.Instance.AllCountryNames.Count()) {
			nextIndex = 0;
		}
		ActiveCountry = CountryRegistry.Instance.AllCountryNames.Skip(nextIndex).First();
	}

	void GoToPrevCountry() {
		var nextIndex = CountryRegistry.Instance.AllCountryNames.ToList().IndexOf(ActiveCountry) - 1;
		if (nextIndex < 0) {
			nextIndex = CountryRegistry.Instance.AllCountryNames.Count() - 1;
		}
		ActiveCountry = CountryRegistry.Instance.AllCountryNames.Skip(nextIndex).First();
	}

}
