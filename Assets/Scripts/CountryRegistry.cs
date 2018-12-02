using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using System.Linq;
using System;

public class CountryRegistry : MonoBehaviour {

	public static CountryRegistry Instance { get; private set; }

	[System.Serializable]
	public class CountryData {
		public string m_Name;
		public int m_Population;
		public float m_DistributionBasePrice;
		public float m_MarketingScaleFactor;
        public float m_MinAudienceShare = 0.03f;
        public float m_MaxAudienceShare = 0.3f;
		public float m_RevenuePerAudienceMember = 1.0f;

		public Color m_LikesColor;
		public float m_LikesColorIntensity = 0.5f;
		public Color m_DislikesColor;
		public float m_DislikesColorIntensity = 0.5f;

		public string m_LikesFlavor;
		public float m_LikesFlavorIntensity = 0.5f;
		public string m_DislikesFlavor;
		public float m_DislikesFlavorIntensity = 0.5f;

		public string m_LikesTopic;
		public float m_LikesTopicIntensity = 0.5f;
		public string m_DislikesTopic;
		public float m_DislikesTopicIntensity = 0.5f;

        public bool IsValid() { return m_Name != null; }

        internal float GetColorScore(Color color)
        {
            if (m_LikesColor != null && m_LikesColor == color) {
				return m_LikesColorIntensity;
			}
			if (m_DislikesColor != null && m_DislikesColor == color) {
				return m_DislikesColorIntensity;
			}
			return 0;
        }

        internal float GetFlavorScore(string flavor)
        {
            if (m_LikesFlavor != null && m_LikesFlavor == flavor) {
				return m_LikesColorIntensity;
			}
			if (m_DislikesFlavor != null && m_DislikesFlavor == flavor) {
				return m_DislikesColorIntensity;
			}
			return 0;
        }

        internal float GetTopicScore(string topic)
        {
            if (m_LikesFlavor != null && m_LikesFlavor == topic) {
				return m_LikesColorIntensity;
			}
			if (m_DislikesFlavor != null && m_DislikesFlavor == topic) {
				return m_DislikesColorIntensity;
			}
			return 0;
        }
    }

	public IEnumerable<string> AllCountryNames { get { return m_Countries.Select(c => c.m_Name); } }

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

    internal int GetActualAudienceSize(string country, CreativeProductData productData)
    {
		var countryData = GetCountryData(country);

		var colorScore = countryData.GetColorScore(productData.Color);
		var flavorScore = countryData.GetFlavorScore(productData.Flavor);
		var topicScore = countryData.GetTopicScore(productData.Topic);

		var overallScore = Mathf.Max(0, (colorScore + flavorScore + topicScore) / 3);

		var audienceSharePct = Mathf.Lerp(countryData.m_MinAudienceShare, countryData.m_MaxAudienceShare, overallScore*overallScore);
		var audienceCount = (int)(countryData.m_Population * audienceSharePct);

		return audienceCount;
    }
}
