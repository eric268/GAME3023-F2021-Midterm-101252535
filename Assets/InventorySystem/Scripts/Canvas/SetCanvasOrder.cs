using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCanvasOrder : MonoBehaviour
{
    BoxCollider2D m_collider;
    [SerializeField]
    TileType m_type;
    bool m_mouseButtonDown = false;

    //Sets canvas collider values
    private void Start()
    {
        m_collider = GetComponent<BoxCollider2D>();
        m_collider.size = GetComponent<RectTransform>().sizeDelta;

        if (m_type == TileType.CONTAINER_TILE)
        {
            m_collider.offset = -GetComponent<RectTransform>().sizeDelta/2;
        }
        
    }
    //Checks if the mouse if over which canvas and sets that to be the first child under the main canvas
    //This is so it is rendered above other items
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            m_mouseButtonDown = true;
        else if (Input.GetMouseButtonUp(0))
            m_mouseButtonDown = false;

        if (m_mouseButtonDown && m_collider.OverlapPoint(Input.mousePosition))
        {
            transform.SetAsFirstSibling();
        }
    }

    
}
