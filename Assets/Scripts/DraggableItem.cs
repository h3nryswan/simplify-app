using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEditor;
using TMPro;
using Unity.VisualScripting;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    // Prefabs
    public GameObject slotPrefab;
    public GameObject plusPrefab;
    public GameObject minusPrefab;
    public GameObject zeroPrefab;
    [HideInInspector] public GameObject grid;

    //Bools
    public bool makeSlot = false;
    public bool term2term = false;
    public bool dragged_lhs = false;
    public bool dragged_rhs = false;
    public bool dropped_lhs = false;
    public bool dropped_rhs = false;

    //Transforms
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public Transform parentBeforeDrag;
    [HideInInspector] public Transform objectBeingDragged;
    [HideInInspector] public Transform operatorForObject;
    [HideInInspector] public Transform EqualSign;
    [HideInInspector] public Transform Addend;
    [HideInInspector] public List<Transform> lhs;
    [HideInInspector] public List<Transform> rhs;

    //Game Objects
    [HideInInspector] public GameObject newOperator;
    [HideInInspector] public GameObject newSlot;

    [HideInInspector] public int siblingIndexBefore;

    public void DestroySlot()
    {
        Destroy(newOperator);
        Destroy(newSlot);
        makeSlot = false;
    }
    public void createNewSlot(GameObject prefab, int index_Operator, int index_Slot)
    {
        newOperator = Instantiate(prefab, new Vector2(0, 0), Quaternion.identity);
        newOperator.transform.SetParent(GameObject.Find("GRID").transform);
        newOperator.transform.SetSiblingIndex(index_Operator);

        newSlot = Instantiate(slotPrefab, new Vector2(0, 0), Quaternion.identity);
        newSlot.transform.SetParent(GameObject.Find("GRID").transform);
        newSlot.transform.SetSiblingIndex(index_Slot);
        makeSlot = true;
    }

    public void getObjects()
    {
        lhs.Clear(); rhs.Clear();
        lhs = new List<Transform>(); rhs = new List<Transform>();
        EqualSign = GameObject.Find("EQUALS").transform;
        grid = GameObject.Find("GRID");
        int equalsIndex = EqualSign.GetSiblingIndex();
        for (int i = 0; i < grid.transform.childCount; i++)
        {
            if (i < equalsIndex)
            {
                lhs.Add(grid.transform.GetChild(i));
            }
            if (i > equalsIndex)
            {
                rhs.Add(grid.transform.GetChild(i));
            }
        }
    }

    public void checkForZero(List<Transform> side)
    {
        if (side.First().GetComponentInChildren<TextMeshProUGUI>().text == "0")
        {
            side.First().SetParent(null);
            Destroy(side.First().gameObject);
        }
    }

    public void checkForPlus(List<Transform> side)
    {
        if (side.First().childCount == 0)
        {
            if (side.First().GetComponent<TextMeshProUGUI>().text == "+")
            {
                side.First().SetParent(null);
                Destroy(side.First().gameObject);
            }
        }
        if (side.Last().childCount == 0)
        {
            if (side.Last().GetComponent<TextMeshProUGUI>().text == "+")
            {
                side.Last().SetParent(null);
                Destroy(side.Last().gameObject);
            }
            if (side.Last().GetComponent<TextMeshProUGUI>().text == "-")
            {
                side.Last().SetParent(null);
                Destroy(side.Last().gameObject);
            }
        }

    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        getObjects();
        makeSlot = false;
        term2term = false;
        //check = false;
        parentAfterDrag = transform.parent;
        parentBeforeDrag = parentAfterDrag;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
        siblingIndexBefore = parentBeforeDrag.transform.GetSiblingIndex();
        objectBeingDragged = grid.transform.GetChild(siblingIndexBefore);
        operatorForObject = null;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Set the position of the object to the mouse position while dragging
        transform.position = Input.mousePosition;

        //If the object is from the LHS
        if (lhs.Contains(objectBeingDragged))
        {
            dragged_lhs = true;
            dragged_rhs = false;
            // If its on the LHS
            if (transform.position.x < EqualSign.position.x)
            {
                dropped_lhs = true;
                dropped_rhs = false;
                if (makeSlot)
                {
                    DestroySlot();
                }
            }
            // If its on the RHS
            if (transform.position.x > EqualSign.position.x)
            {
                dropped_lhs = false;
                dropped_rhs = true;
                if (makeSlot)
                {
                    DestroySlot();
                }
                if (!makeSlot)
                {
                    int index = lhs.IndexOf(objectBeingDragged);
                    if (index == 0)
                    {
                        createNewSlot(minusPrefab, grid.transform.hierarchyCount, grid.transform.hierarchyCount + 1);
                    }
                    else if (lhs[index - 1].GetComponent<TextMeshProUGUI>().text == "-")
                    {
                        createNewSlot(plusPrefab, grid.transform.hierarchyCount, grid.transform.hierarchyCount + 1);
                        operatorForObject = lhs[index - 1];
                    }
                    else
                    {
                        createNewSlot(minusPrefab, grid.transform.hierarchyCount, grid.transform.hierarchyCount + 1);
                        operatorForObject = lhs[index - 1];
                    }

                }

            }
        }
        // If the object is from the RHS
        if (rhs.Contains(objectBeingDragged))
        {
            dragged_lhs = false;
            dragged_rhs = true;
            // If its on the RHS
            if (transform.position.x > EqualSign.position.x)
            {
                dropped_lhs = false;
                dropped_rhs = true;
                if (makeSlot)
                {
                    DestroySlot();
                }
            }
            // If its on the LHS
            if (transform.position.x < EqualSign.position.x)
            {
                dropped_lhs = true;
                dropped_rhs = false;
                if (makeSlot)
                {
                    DestroySlot();
                }
                if (!makeSlot)
                {
                    int index = rhs.IndexOf(objectBeingDragged);
                    if (index == 0)
                    {
                        createNewSlot(minusPrefab, lhs.Last().GetSiblingIndex() + 1, lhs.Last().GetSiblingIndex() + 2);
                    }
                    else if (rhs[index - 1].GetComponent<TextMeshProUGUI>().text == "-")
                    {
                        createNewSlot(plusPrefab, lhs.Last().GetSiblingIndex() + 1, lhs.Last().GetSiblingIndex() + 2);
                        operatorForObject = rhs[index - 1];
                    }
                    else
                    {
                        createNewSlot(minusPrefab, lhs.Last().GetSiblingIndex() + 1, lhs.Last().GetSiblingIndex() + 2);
                        operatorForObject = rhs[index - 1];
                    }

                }

            }
        }

    }
    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = parentAfterDrag.position;
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
        // If the object has not moved slots
        if (parentAfterDrag == parentBeforeDrag)
        {
            // If new slots have been made
            if (makeSlot)
            {
                DestroySlot();
            }
        }
        if (term2term)
        {
            bool draggedMinus = false;
            bool draggedPlus = false;
            bool addendMinus = false;
            bool addendPlus = false;
            Transform draggedOperator = null;
            Transform addendOperator = null;
            int numToAdd = int.Parse(objectBeingDragged.GetComponentInChildren<TextMeshProUGUI>().text);
            int addend = int.Parse(Addend.GetComponentInChildren<TextMeshProUGUI>().text);
            int sum = 0;
            if (objectBeingDragged.GetSiblingIndex() != 0)
            {
                draggedOperator = objectBeingDragged.parent.GetChild(objectBeingDragged.GetSiblingIndex() - 1);
                draggedOperator.TryGetComponent(out TextMeshProUGUI dragged_t);
                if (dragged_t.text == "-") { draggedMinus = true; }
                if (dragged_t.text == "+") { draggedPlus = true; }
            }
            if (Addend.GetSiblingIndex() != 0)
            {
                addendOperator = Addend.parent.GetChild(Addend.GetSiblingIndex() - 1);
                addendOperator.TryGetComponent(out TextMeshProUGUI addend_t);
                if (addend_t.text == "-") { addendMinus = true; }
                if (addend_t.text == "+") { addendPlus = true; }
            }
            if (draggedMinus || addendMinus)
            {
                if (draggedMinus && addendMinus)
                {
                    sum = -addend - numToAdd;
                    draggedOperator.SetParent(null);
                    Destroy(draggedOperator.gameObject);
                    addendOperator.SetParent(null);
                    Destroy(addendOperator.gameObject);
                }
                else if (draggedMinus)
                {
                    sum = addend - numToAdd;
                    draggedOperator.SetParent(null);
                    Destroy(draggedOperator.gameObject);
                }
                else if (addendMinus)
                {
                    Debug.Log("addend minus");
                    sum = -addend + numToAdd;
                    addendOperator.SetParent(null);
                    Destroy(addendOperator.gameObject);
                    if (draggedPlus)
                    {
                        draggedOperator.SetSiblingIndex(addendOperator.GetSiblingIndex() - 1);
                    }
                    
                }
            }
            else
            {
                sum = addend + numToAdd;
                if (draggedOperator != null)
                {
                    if (draggedPlus)
                    {
                        draggedOperator.SetParent(null);
                        Destroy(draggedOperator.gameObject);
                    }
                }
                if (addendOperator!= null)
                {
                    if (addendOperator.GetSiblingIndex() == 0)
                    {
                        if (addendPlus)
                        {
                            addendOperator.SetParent(null);
                            Destroy(addendOperator.gameObject);
                        }
                    }
                    if (addendOperator.GetSiblingIndex() == GameObject.Find("EQUALS").transform.GetSiblingIndex()+1)
                    {
                        if (addendPlus)
                        {
                            addendOperator.SetParent(null);
                            Destroy(addendOperator.gameObject);
                        }
                    }
                }
                //if (addendOperator != null)
                //{
                //    if (addendPlus)
                //    {
                //        addendOperator.SetParent(null);
                //        Destroy(addendOperator.gameObject);
                //    }
                //}
                //getObjects();
                //if (lhs.Count > 0)
                //{
                //    checkForPlus(lhs);
                //}
                //if (rhs.Count > 0)
                //{
                //    checkForPlus(rhs);
                //}
            }
            if (sum > 0)
            {
                Addend.GetComponentInChildren<TextMeshProUGUI>().text = sum.ToString();
            }
            else
            {
                Addend.GetComponentInChildren<TextMeshProUGUI>().text = System.Math.Abs(sum).ToString();
                newOperator = Instantiate(minusPrefab, new Vector2(0, 0), Quaternion.identity);
                newOperator.transform.SetParent(GameObject.Find("GRID").transform);
                if (Addend.GetSiblingIndex() == 0)
                {
                    newOperator.transform.SetSiblingIndex(0);
                }
                else
                {
                    newOperator.transform.SetSiblingIndex(Addend.GetSiblingIndex());
                }
            }
            // Remove old slot
            parentBeforeDrag.SetParent(null);
            Destroy(parentBeforeDrag.gameObject);


            getObjects();
            if (lhs.Count > 0)
            {
                checkForPlus(lhs);
            }
            if (rhs.Count > 0)
            {
                checkForPlus(rhs);
            }
        }
        if (parentAfterDrag != parentBeforeDrag)  // If the object has moved slots
        {
            // Remove old slot
            parentBeforeDrag.SetParent(null);
            Destroy(parentBeforeDrag.gameObject);
            // Remove accompanying operator, if present
            if (operatorForObject != null)
            {
                operatorForObject.SetParent(null);
                Destroy(operatorForObject.gameObject);
            }
            // Remove unecessary plus signs
            getObjects();
            if (lhs.Count > 0)
            {
                checkForPlus(lhs);
            }
            if (rhs.Count > 0)
            {
                checkForPlus(rhs);
            }
            // Remove unecessary zeros
            getObjects();
            if (lhs.Count > 0)
            {
                checkForZero(lhs);
            }
            if (rhs.Count > 0)
            {
                checkForZero(rhs);
            }
            // Check again for unessary plus signs
            getObjects();
            if (lhs.Count > 0)
            {
                checkForPlus(lhs);
            }
            if (rhs.Count > 0)
            {
                checkForPlus(rhs);
            }
            // check if there are two operaters next to eachother -> apply logic table

            // check if lhs or rhs are empty and add a zero if true
            getObjects();
            if (rhs.Count == 0)
            {
                newSlot = Instantiate(zeroPrefab, new Vector2(0, 0), Quaternion.identity);
                newSlot.transform.SetParent(GameObject.Find("GRID").transform);
                newSlot.transform.SetAsLastSibling();
            }
            if (lhs.Count == 0)
            {
                newSlot = Instantiate(zeroPrefab, new Vector2(0, 0), Quaternion.identity);
                newSlot.transform.SetParent(GameObject.Find("GRID").transform);
                newSlot.transform.SetAsFirstSibling();
            }
        }

    }
}

