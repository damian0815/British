using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExamineController : MonoBehaviour {

	public float m_ExamineDistance = 2;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		var player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlatformerCharacter2D>();
		
		if (DistributeUI.Instance.Visible) {
			ExamineUI.Instance.Visible = false;
		} else if (player.IsCarryingObject) {
			ExamineUI.Instance.Object = player.CarryingObject;
			ExamineUI.Instance.Visible = true;
		} else {
			var pickupableObjects = GameObject.FindGameObjectsWithTag("Pickupable");

			GameObject bestObject = null;
			var bestDistance = float.MaxValue;

			var closestPickupable = pickupableObjects.OrderBy(o => (o.transform.position - player.transform.position).magnitude).FirstOrDefault();
			if ((closestPickupable.transform.position - player.transform.position).magnitude < m_ExamineDistance) {
				ExamineUI.Instance.Object = closestPickupable;
				ExamineUI.Instance.Visible = true;
			} else {
				ExamineUI.Instance.Object = null;
				ExamineUI.Instance.Visible = false;
			}
		}
	}
}
