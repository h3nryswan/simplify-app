using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TermSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        bool number1 = false;
        bool number2 = false;
        TextMeshProUGUI t = transform.GetComponentInChildren<TextMeshProUGUI>();
        GameObject object1 = eventData.pointerDrag;
        TextMeshProUGUI t1 = object1.GetComponentInChildren<TextMeshProUGUI>();
        DraggableItem item = eventData.pointerDrag.GetComponentInChildren<DraggableItem>();
        if (t != null)
        {
            number1 = int.TryParse(t.text, out _);
        }
        if (t1 != null) 
        {
            number2 = int.TryParse(t1.text, out _);
        }
        if (transform.childCount == 0)
        {
            GameObject dropped = eventData.pointerDrag;
            DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();
            draggableItem.parentAfterDrag = transform;
        }
        if (number1 && number2)
        {
            if (item.dragged_lhs && item.dropped_lhs)
            {
                GameObject dropped = eventData.pointerDrag;
                DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();
                draggableItem.term2term = true;
                draggableItem.Addend = transform;
            }
            if (item.dragged_rhs && item.dropped_rhs)
            {
                GameObject dropped = eventData.pointerDrag;
                DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();
                draggableItem.term2term = true;
                draggableItem.Addend = transform;
            }
            
        }
        
    }


}
