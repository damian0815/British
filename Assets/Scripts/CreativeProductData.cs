using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public struct CreativeProductData : IEquatable<CreativeProductData> {

	public Color Color { get; set; }
	public string Title { get; set; }
	public string Topic { get; set; }
	public string Flavor { get; set; }

    public bool Equals(CreativeProductData other)
    {
        return other.Title == Title;
    }
}
