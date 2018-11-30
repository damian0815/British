using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using System.Linq;

public class CountryRegistry : MonoBehaviour {

	public static CountryRegistry Instance { get; private set; }

	[System.Serializable]
	public class CountryData {
		public string m_Name;
		public int m_Population;
		public float m_DistributionBasePrice;
		public float m_MarketingScaleFactor;

		public bool IsValid() { return m_Name != null; }
	}


	public CountryData[] m_Countries;

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

	public CountryData GetCountryData(string countryName) {
		return m_Countries.FirstOrDefault(c => c.m_Name.Equals(countryName));
	}
}
