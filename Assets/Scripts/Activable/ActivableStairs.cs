using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivableStairs : MonoBehaviour, I_Activable
{
    [SerializeField]
    private float m_Speed;

    private bool m_HasBeenActivated;

    private void Start()
    {
        Vector3 newPosition = transform.position;
        newPosition.y = 1;
        transform.position = newPosition;
    }

    private void Update()
    {
        if (m_HasBeenActivated)
        {
            if (transform.position.y > 0)
            {
                Vector3 newPosition = transform.position;
                newPosition.y -= m_Speed * Time.deltaTime;
                if (newPosition.y <= 0)
                {
                    newPosition.y = 0;
                    GetComponent<I_Tile>().SetWalkable(true);
                    m_HasBeenActivated = false;
                }
                transform.position = newPosition;
            }
        }
    }

    public void OnActivation()
    {
        m_HasBeenActivated = true;
    }
}
