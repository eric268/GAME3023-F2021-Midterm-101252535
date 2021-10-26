using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScaleCollidersWithSize : MonoBehaviour
{
    private BoxCollider2D m_collider;
    private RectTransform m_rect;
    [SerializeField]
    private Vector2 m_dimensions;
    public Vector2 m_offset;



    // Start is called before the first frame update
    void Start()
    {
        m_collider = GetComponent<BoxCollider2D>();
        m_rect = GetComponent<RectTransform>();
        m_collider.size = (m_rect.sizeDelta*0.9f);
        float xOffset = (m_dimensions.x-1.0f)/2.0f;
        float yOffset = (m_dimensions.y - 1.0f) / 2.0f;
        float sizePerSquare = m_rect.sizeDelta.x / m_dimensions.x;
        m_offset = new Vector2(-(xOffset * (sizePerSquare)), (yOffset * (sizePerSquare)));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
