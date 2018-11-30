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
	public string[] m_PossibleFlavours;
	public string[] m_PossibleTitles;

	public CreativeProductData ProductData;


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

		var title = PickRandom(m_PossibleTitles);
		var flavour = PickRandom(m_PossibleFlavours);
		var topic = PickRandom(m_PossibleTopics);
		
		ProductData = new CreativeProductData() {
			Color = color,
			Title = title,
			Flavour = flavour,
			Topic = topic
		};
	}

    private static string PickRandom(string[] options)
    {
		var which = UnityEngine.Random.Range(0, options.Length);
        return options[which];
    }



}
