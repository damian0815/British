using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DistributionHQ : MonoBehaviour {

	public class DistributionFinishedEventArgs: EventArgs {
		public DistributionFinishedEventArgs(DistributingData data, int revenue) { Data = data; Revenue = revenue; }
		public DistributingData Data { get; private set; }
        public int Revenue { get; private set; }
    }

	public event EventHandler<DistributionFinishedEventArgs> DistributionFinished;

	public static DistributionHQ Instance { get; private set; }

	public class DistributingData : IEquatable<DistributingData> {
		public CreativeProductData productData;
		public float timer;
		public string country;

        public bool Equals(DistributingData other)
        {
			return productData.Equals(other.productData) && country.Equals(other.country);
        }
    }

	private List<DistributingData> m_CurrentlyDistributing = new List<DistributingData>();

	private List<DistributingData> m_FinishedDistribution = new List<DistributingData>();

	void Awake() {
		Debug.Assert(Instance == null);
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		foreach (var d in m_CurrentlyDistributing) {
			d.timer -= Time.deltaTime;
		}

		var finished = m_CurrentlyDistributing.Where(d => d.timer <= 0).ToList();
		m_FinishedDistribution.AddRange(finished);
		m_CurrentlyDistributing.RemoveAll(d => finished.Contains(d));

		foreach (var d in finished) {
			OnDistributionFinished(d);
		}
	}


    void OnTriggerEnter2D(Collider2D other) {
		//Debug.Log("OnTriggerEnter2D " + other.gameObject.name);
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

	public bool HasDistributed(CreativeProductData productData, string country) {
		if (m_FinishedDistribution.Any(d => d.productData.Equals(productData) && d.country.Equals(country))) {
			return true;
		}
		if (m_CurrentlyDistributing.Any(d => d.productData.Equals(productData) && d.country.Equals(country))) {
			return true;
		}

		return false;

	}

	public void Distribute(CreativeProductData productData, string country, float desiredMediaSaturationPct) {
		if (HasDistributed(productData, country)) {
			Debug.LogError("Already distributed " + productData.Title + " to " + country);
			return;
		}
		Debug.Log("Distributing '" + productData.Title + "' to " + country + "...");
		GameState.Instance.m_Funds -= GetDistributionPrice(productData, country, desiredMediaSaturationPct);
		var timer = UnityEngine.Random.Range(20,60);
		var data = new DistributingData() { productData = productData, timer = timer, country = country };
		m_CurrentlyDistributing.Add(data);
	}


    private void OnDistributionFinished(DistributingData d)
    {
		var actualAudienceSize = CountryRegistry.Instance.GetActualAudienceSize(d.country, d.productData);
		var revenue = (int)((float)actualAudienceSize * CountryRegistry.Instance.GetCountryData(d.country).m_RevenuePerAudienceMember);
		GameState.Instance.m_Funds += revenue;

		var handler = DistributionFinished;
		if (handler != null) {
			handler(this, new DistributionFinishedEventArgs(d, revenue));
		}
    }

}
