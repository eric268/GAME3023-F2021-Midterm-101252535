using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    GameObject containerCanvas;

    [SerializeField]
    ItemTable itemTable;

    [SerializeField]
    List<GameObject> m_inventoryItems;

    private void Start()
    {
        itemTable.AssignItemIDs();
    }

    public void OpenContainer()
    {
        containerCanvas.SetActive(true);
        foreach(GameObject item in m_inventoryItems)
        {
            item.SetActive(true);
        }
    }

    public void CloseContainer()
    {
        m_inventoryItems = new List<GameObject>();

        ItemSlot[] itemSlotArray  = containerCanvas.GetComponentsInChildren<ItemSlot>();

        foreach (ItemSlot item in itemSlotArray)
        {
            if (item.gameObject.activeSelf)
            {
                m_inventoryItems.Add(item.gameObject);
                item.gameObject.SetActive(false);
            }
        }
        containerCanvas.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
       //if (collision.gameObject == m_playerRef)
        {
            OpenContainer();
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        //if (collision.gameObject == m_playerRef)
        {
            CloseContainer();
        }
    }
}