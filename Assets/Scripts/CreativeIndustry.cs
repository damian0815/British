using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreativeIndustry : MonoBehaviour {

	public GameObject m_CreativeProductPrefab;
	public float m_ProductionInterval = 10;

	public Transform m_ProductEjectionPoint;
	public Transform m_ProductEjectionTarget;
	public float m_EjectionForceFactor = 1;

	private float m_Timer;

	// Use this for initialization
	void Start () {
		m_Timer =  Mathf.Min(2, m_ProductionInterval);
	}
	
	// Update is called once per frame
	void Update () {
		
		m_Timer -= Time.deltaTime;
		if (m_Timer < 0) {
			EjectProduct();
			m_Timer += m_ProductionInterval * UnityEngine.Random.Range(0.7f,1.0f/0.7f);
		}
	}

	void EjectProduct() {
		var product = GameObject.Instantiate(m_CreativeProductPrefab, m_ProductEjectionPoint.position, Quaternion.identity);

		var ejectionForceVector = m_ProductEjectionTarget.position-m_ProductEjectionPoint.position;

		product.GetComponent<Rigidbody2D>().AddForce(ejectionForceVector * m_EjectionForceFactor);

	}
}
