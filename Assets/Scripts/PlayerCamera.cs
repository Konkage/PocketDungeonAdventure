using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    private Transform m_Player;

	void Update ()
    {
        transform.position = m_Player.position;
	}
}
