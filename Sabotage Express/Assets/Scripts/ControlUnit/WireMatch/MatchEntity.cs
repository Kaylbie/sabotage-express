using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MatchEntity : MonoBehaviour
{
    [SerializeField] public MatchFeedback matchFeedback;
    [SerializeField] public MovablePair movablePair;
    [SerializeField] public GameObject cylinderWire;
    [SerializeField] public GameObject end;

    public Renderer fixedPairRendered;
    [SerializeField] public MatchSystemManager matchSystemManager;
    [SerializeField] public Port port;



    private bool isMatched;

    public Vector3 GetMovablePairPosition()
    {
        return movablePair.GetPosition();
    }
    public void ConnectToPort()
    {
        movablePair.SetPosition(port.gameObject.transform.position);
    }
    public void ConnectToPort(Vector3 newPortPosition)
    {
        movablePair.SetPosition(newPortPosition);
    }
    public void SetMovablePairPosition(Vector3 newMovablePairPosition)
    {
        movablePair.SetInitialPosition(newMovablePairPosition);
        cylinderWire.transform.position = newMovablePairPosition;
        end.transform.position = newMovablePairPosition;
    }
    public void SetMaterialToPairs(Material pairMaterial)
    {
        movablePair.GetComponent<Renderer>().material = pairMaterial;
        cylinderWire.GetComponent<Renderer>().material = pairMaterial;
        end.GetComponent<Renderer>().material = pairMaterial;
        fixedPairRendered.material = pairMaterial;
    }
    public void PairObjectInteraction(bool isEnter, MovablePair movable)
    {
        if (isEnter && !isMatched)
        {
            isMatched = (movable == movablePair);
            if (isMatched)
            {
                matchSystemManager.NewMatchRecord(isMatched);
                matchFeedback.ChangeMaterialWithMatch(isMatched);
            }
        }
        else if (!isEnter && isMatched)
        {
            isMatched = !(movable == movablePair);
            if (!isMatched)
            {
                matchSystemManager.NewMatchRecord(isMatched);
                matchFeedback.ChangeMaterialWithMatch(isMatched);
            }
        }
    }

}
