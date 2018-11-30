using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistributionHQ : MonoBehaviour {

	public static DistributionHQ Instance { get; private set; }

	void Awake() {
		Debug.Assert(Instance == null);
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log("OnTriggerEnter2D " + other.gameObject.name);
	}

	public int GetDistributionPrice(CreativeProductData productData, string country, float desiredMediaSaturationPct) {
		var data = CountryRegistry.Instance.GetCountryData(country);
		if (data == null) {
			Debug.LogWarning("Unknown country: " + country);
			return 0;
		}

		var marketingCostMax = data.m_Population * data.m_MarketingScaleFactor;

		return (int)(data.m_DistributionBasePrice + desiredMediaSaturationPct * marketingCostMax);
	}


	public void Distribute(CreativeProductData productData, string country, float desiredMediaSaturationPct) {
		Debug.Log("Distributing '" + productData.Title + "' to " + country + "...");
		GameState.Instance.m_Funds -= GetDistributionPrice(productData, country, desiredMediaSaturationPct);
	}

}
