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

	public void Distribute(GameObject o) {
		var product = o.GetComponent<CreativeProduct>();
		var productData = product.ProductData;

		Debug.Log("Distributing '" + productData.Title + "'...");

		o.transform.parent = null;
		Destroy(o);
	}
}
