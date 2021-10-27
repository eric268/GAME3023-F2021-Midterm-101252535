using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ItemHolderCanvas : MonoBehaviour
{
    [SerializeField]
    private int m_iNumberOfContainers;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < m_iNumberOfContainers; i++)
        {
            GameObject holder = new GameObject();
            holder.name = "Item Container " + (i + 1);
            holder.transform.SetParent(this.transform);
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
