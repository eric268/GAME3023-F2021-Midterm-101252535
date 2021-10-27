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

    //Attempts to create an item if the mouse dragged over that area
    public void OnBeginDrag(PointerEventData eventData)
    {
        CreateItemWithSelectedStats(eventData);
    }
    //Tries to place item in an inventory menu
    public void OnEndDrag(PointerEventData eventData)
    {
        CheckIfItemCanBePlaced();
    }
    //Ensures item stays on mouse position
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (m_itemGameObject != null)
        {
            m_itemGameObject.GetComponent<RectTransform>().anchoredPosition += eventData.delta / m_fCanvasScale;
        }
    }
    //Places an item into the scene
    void PlaceItemInInventory(Collider2D bRItemSlotCollider)
    {
        //Gets the collider position at the bottom right of the inventory
        //Every item is a rectangle and therefore to ensure things fit if they collide it will always be put
        //at the bottom right collider
        GameObject tileOwningCollider = bRItemSlotCollider.transform.parent.gameObject;

        //Sets the collider as the parent so that the game object can have 0 anchored position and snap to the tile
        m_itemGameObject.transform.SetParent(tileOwningCollider.transform);
        m_itemGameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        //Decreases number of item count
        m_previousStackItem.GetComponent<ItemSlot>().ItemFromStackMoved();

        //Attaches tile back to correct canvas count so that it can be enabled/disabled accordingly
        m_itemGameObject.GetComponent<ItemManager>().AttachToCanvas(tileOwningCollider.GetComponent<TileAttributes>().m_tileType);

        //Offsets the item position as its pivot point is in the center of the object
        m_itemGameObject.GetComponent<RectTransform>().anchoredPosition += m_itemGameObject.GetComponent<ItemManager>().m_offset;
    }

    void CreateItemWithSelectedStats(PointerEventData eventData)
    {
        //Creates an item from what the mouse had touched and populates it with appropriate values
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
        //Each item knows the number of collisions with tiles it should have if it is to be placed
        //This checks to see if it has collided with the correct amount
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
        //List to hold all current collisions
        List<Collider2D> collArray = new List<Collider2D>();
        m_itemCollider.OverlapCollider(new ContactFilter2D(), collArray);
        
        //Values for finding the bottom right collider
        float lowestYValue = Mathf.Infinity;
        float largestXValue = -Mathf.Infinity;

        foreach (Collider2D col in collArray)
        {
            //If the item is colliding with a tile then increment its tile collision counter
            if (col.transform.parent.gameObject.GetComponent<TileAttributes>())
            {
                collisionsWithInv++;
                RectTransform tempRT = col.transform.parent.gameObject.GetComponent<RectTransform>();

                //If a more bottom/right collider is found set that to the currentBottomRightSlot
                if (tempRT.anchoredPosition.x >= largestXValue && tempRT.anchoredPosition.y <= lowestYValue)
                {
                    m_bottomRightColliderSlot = col;
                    largestXValue = tempRT.anchoredPosition.x;
                    lowestYValue = tempRT.anchoredPosition.y;

                    //Gives the number of the bottom right collider
                    m_itemGameObject.GetComponent<ItemManager>().m_iCurrentTileNumber = col.transform.parent.gameObject.GetComponent<TileAttributes>().m_iTileNumber;
                }
            }
            //Checks if we are colliding with other item
            else if (col.gameObject.GetComponent<OnDrag>())
            {
                //If it is the same item then stack them
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
