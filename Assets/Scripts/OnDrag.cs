using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine;

public class OnDrag : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    private GameObject m_createdGameObject = null;
    private RectTransform m_iconRectTransform = null;
    private GameObject m_previousStackItem = null;
    private Collider2D m_shapeCollider = null;

    private string m_prefabPath;

    [SerializeField]
    private int m_iNumRequiredSlots;

    [SerializeField]
    private GameObject m_canvas;

    private GameObject m_containerCanvas;
    private float m_fCanvasScale;

    public void OnBeginDrag(PointerEventData eventData)
    {   
        m_createdGameObject = Instantiate(eventData.pointerEnter) as GameObject;

        m_createdGameObject.transform.SetParent(m_canvas.transform);
        m_iconRectTransform = m_createdGameObject.GetComponentInChildren<RectTransform>();
        m_iconRectTransform.GetComponent<ItemSlot>().ItemCount = 1;
        m_iconRectTransform.GetComponent<ItemSlot>().itemInSlot = eventData.pointerEnter.GetComponent<ItemSlot>().itemInSlot;
        m_shapeCollider = m_createdGameObject.GetComponentInChildren<Collider2D>();
        m_previousStackItem = eventData.pointerEnter;
        m_iconRectTransform.SetParent(m_previousStackItem.transform.parent.transform);
        
        m_iconRectTransform.anchoredPosition = m_previousStackItem.GetComponent<RectTransform>().anchoredPosition;
        Debug.Log(m_iconRectTransform.anchoredPosition);
    }

    public void OnDrop(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        List<Collider2D> collArray = new List<Collider2D>();
        int collisionsWithInventory = 0;
        //Debug.Break();
        if (m_shapeCollider != null)
        {
            m_shapeCollider.OverlapCollider(new ContactFilter2D(), collArray);
            Collider2D m_lastInventoryCollider = null;
            float lowestYValue = Mathf.Infinity;
            float largestXValue = -Mathf.Infinity;

            foreach (Collider2D col in collArray)
            {
                //Debug.Log(col.gameObject.name);
                if (col.transform.parent.gameObject.GetComponent<InvTileAttributes>())
                {
                    collisionsWithInventory++;
                    RectTransform tempRT = col.transform.parent.gameObject.GetComponent<RectTransform>();
                    if (tempRT.anchoredPosition.x >= largestXValue && tempRT.anchoredPosition.y <= lowestYValue)
                    {
                        m_lastInventoryCollider = col;
                        largestXValue = tempRT.anchoredPosition.x;
                        lowestYValue = tempRT.anchoredPosition.y;
                    }
                }
                else if (col.gameObject.GetComponent<OnDrag>())
                {
                    //Add one to the collided slot inventory if they are the same dimensions otherwise delete
                    //Subtract one from the taken from inventory
                    //Destroy(m_createdGameObject);
                    if (m_previousStackItem.GetComponent<ItemSlot>().itemInSlot == col.GetComponent<ItemSlot>().itemInSlot)
                    {
                        col.GetComponent<ItemSlot>().ItemAddedToStack();
                        m_previousStackItem.GetComponent<ItemSlot>().ItemFromStackMoved();
                        break;
                    }
                    else
                    {
                        Destroy(m_createdGameObject);
                        break;
                    }

                }
            }
            //Set the slot quantity to 1, remove 1 from taken from inventory
            if (collisionsWithInventory >= m_iNumRequiredSlots)
            {
                //Debug.Log("Name: " + m_lastInventoryCollider.transform.parent.gameObject.name);
                //Debug.Log("Offset: " + m_iconRectTransform.GetComponent<ScaleCollidersWithSize>().m_offset);

                if (m_lastInventoryCollider != null)
                {
                    m_iconRectTransform.transform.SetParent(m_lastInventoryCollider.transform.parent.gameObject.transform);
                    m_previousStackItem.GetComponent<ItemSlot>().ItemFromStackMoved();
                }    

                m_iconRectTransform.anchoredPosition = Vector2.zero;
                m_iconRectTransform.transform.SetParent(m_containerCanvas.transform);
                m_iconRectTransform.anchoredPosition += m_iconRectTransform.GetComponent<ScaleCollidersWithSize>().m_offset;


                //m_iconRectTransform.GetComponentInChildren<TextMeshProUGUI>().text.toin
            }
            else
            {
                Destroy(m_createdGameObject);
            }
        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        //eventData.pointerEnter
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (m_iconRectTransform != null)
        {
            m_iconRectTransform.anchoredPosition += eventData.delta/m_fCanvasScale;
        }


    }

    // Start is called before the first frame update
    void Start()
    {
        //m_prefabPath = "Prefabs/" + transform.parent.gameObject.name;
        //m_GameObjectPrefab = Resources.Load(m_prefabPath) as GameObject;
        m_canvas = GameObject.Find("Canvas");
        m_containerCanvas = GameObject.Find("ContainerCanvas");
        m_fCanvasScale = m_canvas.GetComponent<Canvas>().scaleFactor;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
