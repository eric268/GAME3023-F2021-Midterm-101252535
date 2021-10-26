using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine;

public class OnDrag : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    private GameObject m_GameObjectPrefab;
    private GameObject m_createdGameObject;
    private RectTransform m_iconRectTransform;
    private Collider2D m_shapeCollider;

    [SerializeField]
    private int m_iNumRequiredSlots;

    [SerializeField]
    private GameObject m_canvas;
    private float m_fCanvasScale;

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_createdGameObject = Instantiate(m_GameObjectPrefab) as GameObject;
        m_createdGameObject.transform.SetParent(m_canvas.transform);
        m_iconRectTransform = m_createdGameObject.GetComponentInChildren<RectTransform>();
        m_shapeCollider = m_createdGameObject.GetComponentInChildren<Collider2D>();
    }

    public void OnDrop(PointerEventData eventData)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        List<Collider2D> collArray = new List<Collider2D>();
        int collisionsWithInventory = 0;
        
        if (m_shapeCollider != null)
        {
            m_shapeCollider.OverlapCollider(new ContactFilter2D(), collArray);
            Collider2D m_lastInventoryCollider = null;
            float lowestYValue = Mathf.Infinity;
            float largestXValue = -Mathf.Infinity;

            foreach (Collider2D col in collArray)
            {
                Debug.Log(col.gameObject.name);
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
                    Destroy(m_createdGameObject);
                    break;
                }
            }
            //Set the slot quantity to 1, remove 1 from taken from inventory
            if (collisionsWithInventory >= m_iNumRequiredSlots)
            {
                Debug.Log("Name: " + m_lastInventoryCollider.transform.parent.gameObject.name);
                Debug.Log("Offset: " + m_iconRectTransform.GetComponent<ScaleCollidersWithSize>().m_offset);

                m_iconRectTransform.transform.SetParent(m_lastInventoryCollider.transform.parent.gameObject.transform);
                m_iconRectTransform.anchoredPosition = Vector2.zero;
                m_iconRectTransform.transform.SetParent(m_canvas.transform);
                m_iconRectTransform.anchoredPosition += m_iconRectTransform.GetComponent<ScaleCollidersWithSize>().m_offset;
            }
            else
            {
                Destroy(m_createdGameObject);
            }
        }
    }
    static float SortByScore(Collider2D x, Collider2D y)
    {
        RectTransform obj1 = x.transform.parent.gameObject.GetComponent<RectTransform>();
        RectTransform obj2 = y.transform.parent.gameObject.GetComponent<RectTransform>();

        if (obj1.anchoredPosition.x == obj2.anchoredPosition.x)
        {
            return obj1.anchoredPosition.y.CompareTo(obj2.anchoredPosition.y);
        }

        return obj1.anchoredPosition.y.CompareTo(obj2.anchoredPosition.y);

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (m_iconRectTransform != null)
        {
            m_iconRectTransform.anchoredPosition = eventData.position;
            Debug.Log(m_iconRectTransform.anchoredPosition);        
        }


    }

    // Start is called before the first frame update
    void Start()
    {
        m_GameObjectPrefab = Resources.Load("Prefabs/" +transform.parent.gameObject.name) as GameObject;
        m_canvas = GameObject.Find("Canvas");
        m_fCanvasScale = m_canvas.GetComponent<Canvas>().scaleFactor;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
