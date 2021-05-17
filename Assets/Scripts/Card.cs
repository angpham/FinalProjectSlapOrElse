using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    private DeckManager deckManger;

    public DeckManager.CardSuit cardSuit;
    public DeckManager.CardValue cardValue;

    private void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
    }

    public void placeCardOnTable(Vector3 moveLocation)
    {
        this.transform.position = moveLocation;
    }

    //public void moveCardToDisplay(Vector3 moveLocation, float speed)
    //{
    //    StopAllCoroutines();
    //    StartCoroutine(MoveCard(moveLocation, speed));
    //}

    //IEnumerator MoveCard(Vector3 moveLocation, float speed)
    //{
    //    while ((this.transform.position - moveLocation).sqrMagnitude > 0.01f)
    //    {
    //        this.transform.position = Vector3.MoveTowards(transform.position, moveLocation, speed * Time.deltaTime);
    //        yield return null;
    //    }
    //}
}