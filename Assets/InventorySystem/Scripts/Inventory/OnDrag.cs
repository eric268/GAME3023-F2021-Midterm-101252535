using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine;

public class OnDrag : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private GameObject m_createdGameObject = null;
    private GameObject m_itemGameObject = null;
    private GameObject m_previousStackItem = null;
    private Collider2D m_itemCollider = null;

    [SerializeField]
    private int m_iNumRequiredSlots;

    [SerializeField]
    private GameObject m_canvas;

    private float m_fCanvasScale;

    public void OnBeginDrag(PointerEventData eventData)
    {
        CreateItemWithSelectedStats(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        CheckIfItemCanBePlaced();
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (m_itemGameObject != null)
        {
            m_itemGameObject.GetComponent<RectTransform>().anchoredPosition += eventData.delta / m_fCanvasScale;
        }
    }

    void PlaceItemInInventory(Collider2D bRItemSlotCollider)
    {
        GameObject tileOwningCollider = bRItemSlotCollider.transform.parent.gameObject;
        m_itemGameObject.transform.SetParent(tileOwningCollider.transform);
        m_previousStackItem.GetComponent<ItemSlot>().ItemFromStackMoved();

        m_itemGameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        m_itemGameObject.GetComponent<ItemManager>().AttachToCanvas(tileOwningCollider.GetComponent<TileAttributes>().m_tileType);
        m_itemGameObject.GetComponent<RectTransform>().anchoredPosition += m_itemGameObject.GetComponent<ItemManager>().m_offset;
    }

    void CreateItemWithSelectedStats(PointerEventData eventData)
    {
        m_createdGameObject = Instantiate(eventData.pointerEnter) as GameObject;
        m_itemGameObject = m_createdGameObject.GetComponentInChildren<ItemManager>().gameObject;
        m_itemGameObject.GetComponent<ItemSlot>().ItemCount = 1;
        m_itemGameObject.GetComponent<ItemSlot>().itemInSlot = eventData.pointerEnter.GetComponent<ItemSlot>().itemInSlot;
        m_itemCollider = m_createdGameObject.GetComponentInChildren<Collider2D>();
        m_previousStackItem = eventData.pointerEnter;
        m_itemGameObject.transform.SetParent(m_previousStackItem.transform.parent.transform);
        m_itemGameObject.transform.localScale = Vector3.one;
        m_itemGameObject.GetComponent<RectTransform>().anchoredPosition = m_previousStackItem.GetComponent<RectTransform>().anchoredPosition;
    }

    void CheckIfItemCanBePlaced()
    {
        int collisionsWithInventory = 0;
        Collider2D m_bottomRightItemSlotCollider = null;

        CheckCollisionWithItemSlots(ref collisionsWithInventory, ref m_bottomRightItemSlotCollider);

        if (m_itemCollider != null && m_bottomRightItemSlotCollider != null)
        {
            if (collisionsWithInventory >= m_iNumRequiredSlots)
            {
                PlaceItemInInventory(m_bottomRightItemSlotCollider);
            }
            else
            {
                Destroy(m_createdGameObject);
            }
        }
    }

    void CheckCollisionWithItemSlots(ref int collisionsWithInv, ref Collider2D m_bottomRightColliderSlot)
    {
        List<Collider2D> collArray = new List<Collider2D>();
        m_itemCollider.OverlapCollider(new ContactFilter2D(), collArray);
        
        float lowestYValue = Mathf.Infinity;
        float largestXValue = -Mathf.Infinity;

        foreach (Collider2D col in collArray)
        {
            if (col.transform.parent.gameObject.GetComponent<TileAttributes>())
            {
                collisionsWithInv++;
                RectTransform tempRT = col.transform.parent.gameObject.GetComponent<RectTransform>();
                if (tempRT.anchoredPosition.x >= largestXValue && tempRT.anchoredPosition.y <= lowestYValue)
                {
                    m_bottomRightColliderSlot = col;
                    largestXValue = tempRT.anchoredPosition.x;
                    lowestYValue = tempRT.anchoredPosition.y;

                    m_itemGameObject.GetComponent<ItemManager>().m_iCurrentTileNumber = col.transform.parent.gameObject.GetComponent<TileAttributes>().m_iTileNumber;
                }
            }
            else if (col.gameObject.GetComponent<OnDrag>())
            {
                if (m_previousStackItem.GetComponent<ItemSlot>().itemInSlot == col.GetComponent<ItemSlot>().itemInSlot)
                {
                    col.GetComponent<ItemSlot>().ItemAddedToStack();
                    m_previousStackItem.GetComponent<ItemSlot>().ItemFromStackMoved();
                    Destroy(m_createdGameObject);
                    return;
                }
                else
                {
                    Destroy(m_createdGameObject);
                    return;
                }
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        m_fCanvasScale = m_canvas.GetComponent<Canvas>().scaleFactor;
    }  
}