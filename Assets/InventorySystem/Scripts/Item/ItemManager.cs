using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemManager : MonoBehaviour
{
    private BoxCollider2D m_collider;
    private RectTransform m_rect;
    [SerializeField]
    private Vector2 m_dimensions;
    public Vector2 m_offset;
    public int m_iCurrentTileNumber = -1;

    TileType m_canvasCurrentlyAttachedToo;

    [SerializeField]
    public Canvas m_containerCanvas;

    [SerializeField]
    public Canvas m_inventoryCanvas;


    // Start is called before the first frame update
    void Start()
    {
        //Scales the items to fit any canvas size as canvas scales with screen size
        m_collider = GetComponent<BoxCollider2D>();
        m_rect = GetComponent<RectTransform>();
        m_collider.size = (m_rect.sizeDelta*0.85f);
        float xOffset = (m_dimensions.x-1.0f)/2.0f;
        float yOffset = (m_dimensions.y - 1.0f) / 2.0f;
        float sizePerSquare = m_rect.sizeDelta.x / m_dimensions.x;
        m_offset = new Vector2(-(xOffset * (sizePerSquare)), (yOffset * (sizePerSquare)));
    }

    public void AttachToCanvas(TileType type)
    {
        switch (type)
        {
            //Places the item under the correct canvas
            case TileType.INVENTORY_TILE:
                transform.SetParent(m_inventoryCanvas.transform);
                m_canvasCurrentlyAttachedToo = TileType.INVENTORY_TILE;
                break;
            case TileType.CONTAINER_TILE:
                transform.SetParent(m_containerCanvas.transform);
                m_canvasCurrentlyAttachedToo = TileType.CONTAINER_TILE;
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
