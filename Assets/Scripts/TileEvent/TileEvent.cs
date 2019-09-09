using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileEvent : MonoBehaviour
{
    private I_Tile m_Tile;

    private void Start()
    {
        m_Tile = GetComponent<I_Tile>();
        if (m_Tile != null)
        {
            m_Tile.AddTileEvent(this);
        }
    }

    public abstract void ActivateEvent(I_Unit _UnitThatWalkedOnTile);
}
