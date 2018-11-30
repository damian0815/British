using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {
    public static GameState Instance { get; internal set; }

	public int m_Funds = 10^8; // 100 million pounds

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
}
