using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Instantiates prefabs to fill a grid
[RequireComponent(typeof(GridLayout))]
public class ItemSlotGridDimensioner : MonoBehaviour
{
    [SerializeField]
    GameObject itemSlotPrefab;
    int counter = 1;

    [SerializeField]
    Vector2Int GridDimensions = new Vector2Int(6, 6);
    BoxCollider2D[] m_tileColliderArray;

    [SerializeField]
    private TileType m_tileType;

    void Start()
    {
        int numCells = GridDimensions.x * GridDimensions.y;

        while (transform.childCount < numCells)
        {
            GameObject newObject = Instantiate(itemSlotPrefab, this.transform);
            newObject.GetComponent<TileAttributes>().m_tileType = m_tileType;
            newObject.name = "Tile " + counter;
            newObject.GetComponent<TileAttributes>().m_iTileNumber = counter;
            counter++;
        }
        m_tileColliderArray = GetComponentsInChildren<BoxCollider2D>();

        for (int i = 0; i < m_tileColliderArray.Length; i++)
        {
            m_tileColliderArray[i].size =  m_tileColliderArray[i].GetComponent<RectTransform>().sizeDelta/4.0f;
        }
    }
}
