using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScaleCollidersWithSize : MonoBehaviour
{
    private BoxCollider2D m_collider;
    private RectTransform m_rect;

    private 

    // Start is called before the first frame update
    void Start()
    {
        m_collider = GetComponent<BoxCollider2D>();
        m_rect = GetComponent<RectTransform>();
        //m_rect.sizeDelta *= GameObject.Find("Canvas").GetComponent<Canvas>().scaleFactor;
        m_collider.size = m_rect.sizeDelta;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
