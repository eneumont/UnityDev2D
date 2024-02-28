using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup2D : MonoBehaviour {
	enum eType {
		Health,
		Score,
		Power
	}

	[SerializeField] eType type;
	[SerializeField] float value;
}