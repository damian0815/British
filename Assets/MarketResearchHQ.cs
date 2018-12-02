using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketResearchHQ : MonoBehaviour {

	public static MarketResearchHQ Instance { get; private set; }

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

	public string DescribeSuitability(CreativeProductData productData, string country) {
		string suitability = "";
		var countryData = CountryRegistry.Instance.GetCountryData(country);

		var colorScore = countryData.GetColorScore(productData.Color);
		var flavorScore = countryData.GetFlavorScore(productData.Flavor);
		var topicScore = countryData.GetTopicScore(productData.Topic);

		if (colorScore > 0.5f) {
			suitability += string.Format("+ {0} is in a color that {1} likes\n", productData.Title, country);
		} else if (colorScore < 0) {
			suitability += string.Format("- {0} is in a color that {1} dislikes - consider changing color\n", productData.Title, productData.Color, country);
		}
		if (flavorScore > 0.5f) {
			suitability += string.Format("+ {0} is {1} and {2} likes {1}\n", productData.Title, productData.Flavor, country);
		} else if (flavorScore < 0) {
			suitability += string.Format("- {0} is {1} but {2} doesn't like {1} - consider changing flavor\n", productData.Title, productData.Flavor, country);
		}
		if (topicScore > 0.5f) {
			suitability += string.Format("+ {0} is {1} and {2} likes {1}\n", productData.Title, productData.Topic, country);
		} else if (topicScore < 0) {
			suitability += string.Format("- {0} is {1} but {2} doesn't like {1} - consider changing topic\n", productData.Title, productData.Topic, country);
		}

		if (suitability.Length > 0) {
			suitability += "\n";
		}

		var overallScore = Mathf.Max(0, (colorScore + flavorScore + topicScore) / 3);

		var audienceSharePct = Mathf.Lerp(countryData.m_MinAudienceShare, countryData.m_MaxAudienceShare, overallScore*overallScore);
		var audienceCount = countryData.m_Population * audienceSharePct;

		suitability += string.Format("Likely audience share: {0:0.0}% ({1:n0} people)", audienceSharePct * 100.0f, audienceCount);

		return suitability;
	}

}
