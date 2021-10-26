using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class OnDrag : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    private GameObject m_GameObjectPrefab;
    private GameObject m_createdGameObject;
    private RectTransform m_iconRectTransform;

    [SerializeField]
    private GameObject m_canvas;
    private float m_fCanvasScale;

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_createdGameObject = Instantiate(m_GameObjectPrefab) as GameObject;
        m_createdGameObject.transform.SetParent(m_canvas.transform);
        m_iconRectTransform = m_createdGameObject.GetComponentInChildren<RectTransform>();
    }

    public void OnDrop(PointerEventData eventData)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log(transform.parent.gameObject.name);
        
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (m_iconRectTransform != null)
        {
            m_iconRectTransform.anchoredPosition = eventData.position;
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
