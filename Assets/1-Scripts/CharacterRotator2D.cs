using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterRotator2D : MonoBehaviour
{
    //Rotation Vectors
    private Vector3 NorthVec = Vector3.up;
    private Vector3 NorthEastVec = Quaternion.AngleAxis(-60, Vector3.forward) * Vector3.up;
    private Vector3 SouthEastVec = Quaternion.AngleAxis(-120, Vector3.forward) * Vector3.up;
    private Vector3 SouthVec =  Vector3.down;
    private Vector3 SouthWestVec = Quaternion.AngleAxis(-240, Vector3.forward) * Vector3.up;
    private Vector3 northWestVec = Quaternion.AngleAxis(-300, Vector3.forward) * Vector3.up;

    [SerializeField] public List<ObjectOrder> northObjectOrders;
    [SerializeField] public List<ObjectOrder> northEastObjectOrders;
    [SerializeField] public List<ObjectOrder> southEastObjectOrders;
    [SerializeField] public List<ObjectOrder> southObjectOrders;
    [SerializeField] public List<ObjectOrder> southWestObjectOrders;
    [SerializeField] public List<ObjectOrder> northWestObjectOrders;

    [Header("Attack Objects")]
    [SerializeField] public List<GameObject> objectsToDeactivateOnAttack;


    private List<GameObject> currentActiveObjects;
    [HideInInspector]
    public Direction currentDirection;
    [HideInInspector]
    public Vector3 currentDirectionVector = Vector3.up;

    public IAnimationController animationControllerRef;
    private void Start()
    {
        animationControllerRef = gameObject.GetComponent<IAnimationController>();
        currentActiveObjects = new List<GameObject>();
        foreach (var item in southObjectOrders)
        {
            currentActiveObjects.Add(item.Object);
        }
    }

    public void RotateCharacter(Direction directionToRotate)
    {
        if(currentDirection != directionToRotate)
        {
            DeactivateCurrentObjects();
            currentDirection = directionToRotate;
            switch (directionToRotate)
            {
                case Direction.North:
                    {
                        foreach(ObjectOrder g in northObjectOrders)
                        {
                            SetObjectOrders(g);
                            currentDirectionVector = NorthVec;
                            animationControllerRef.SetAnimatorVec(currentDirectionVector);
                        }
                        break;
                    }
                case Direction.NorthEast:
                    {
                        foreach (ObjectOrder g in northEastObjectOrders)
                        {
                            SetObjectOrders(g);
                            currentDirectionVector = NorthEastVec;
                            animationControllerRef.SetAnimatorVec(currentDirectionVector);
                        }
                        break;
                    }
                case Direction.SouthEast:
                    {
                        foreach (ObjectOrder g in southEastObjectOrders)
                        {
                            SetObjectOrders(g);
                            currentDirectionVector = SouthEastVec;
                            animationControllerRef.SetAnimatorVec(currentDirectionVector);
                        }
                        break;
                    }
                case Direction.South:
                    {
                        foreach (ObjectOrder g in southObjectOrders)
                        {
                            SetObjectOrders(g);
                            currentDirectionVector = SouthVec;
                            animationControllerRef.SetAnimatorVec(currentDirectionVector);
                        }
                        break;
                    }
                case Direction.SouthWest:
                    {
                        foreach (ObjectOrder g in southWestObjectOrders)
                        {
                            SetObjectOrders(g);
                            currentDirectionVector = SouthWestVec;
                            animationControllerRef.SetAnimatorVec(currentDirectionVector);
                        }
                        break;
                    }
                case Direction.NorthWest:
                    {
                        foreach (ObjectOrder g in northWestObjectOrders)
                        {
                            SetObjectOrders(g);
                            currentDirectionVector = northWestVec;
                            animationControllerRef.SetAnimatorVec(currentDirectionVector);
                        }
                        break;
                    }
                default: break;
            }
        }
    }

    private void SetObjectOrders(ObjectOrder g)
    {
        g.Object.SetActive(true);
        currentActiveObjects.Add(g.Object);
        if(g.Object.GetComponent<SpriteRenderer>() != null)
        {
            g.Object.GetComponent<SpriteRenderer>().sortingOrder = g.Order;
        }
        else
        {
            foreach(SpriteRenderer r in g.Object.GetComponentsInChildren<SpriteRenderer>())
            {
                r.sortingOrder = g.Order;
            }
        }
    }

    private void DeactivateCurrentObjects()
    {
        foreach (GameObject g in currentActiveObjects)
        {
            g.SetActive(false);
        }
        currentActiveObjects.Clear();
    }

    public void DeactivateAttackObjects()
    {
        foreach (GameObject g in objectsToDeactivateOnAttack)
        {
            g.SetActive(false);
        }
    }
}
