using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CreativeProduct : MonoBehaviour {

	[FormerlySerializedAs("m_Shapes")]
	public Sprite[] m_PossibleShapes;

	[FormerlySerializedAs("m_Colors")]	
	public Color[] m_PossibleColors;

	public string[] m_PossibleTopics;
	
	[FormerlySerializedAs("m_PossibleFlavours")]	
	public string[] m_PossibleFlavors;
	public string[] m_PossibleTitles;

	public CreativeProductData ProductData;

	private static List<string> m_UsedTitles = new List<string>();


	// Use this for initialization
	void Start () {
		var whichShape = UnityEngine.Random.Range(0, m_PossibleShapes.Length);
		GetComponent<SpriteRenderer>().sprite = m_PossibleShapes[whichShape];

		Populate();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Populate()
	{

		var whichColor = UnityEngine.Random.Range(0, m_PossibleColors.Length);
		var color = m_PossibleColors[whichColor];
		GetComponent<SpriteRenderer>().color = color;

		var title = MakeTitleUnique(PickRandom(m_PossibleTitles));
		var flavor = PickRandom(m_PossibleFlavors);
		var topic = PickRandom(m_PossibleTopics);
	
		ProductData = new CreativeProductData() {
			Color = color,
			Title = title,
			Flavor = flavor,
			Topic = topic
		};
		m_UsedTitles.Add(title);
	}

    private static string PickRandom(string[] options)
    {
		var which = UnityEngine.Random.Range(0, options.Length);
        return options[which];
    }

	private static string MakeTitleUnique(string originalTitle) {
		var title = originalTitle;
		int counter = 0;
		while (m_UsedTitles.Contains(title)) {
			++counter;
			title = originalTitle + " " + counter.ToString();
		}
		return title;
	}


}
