using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    private DeckManager deckManager;

    public DeckManager.CardSuit cardSuit;
    public DeckManager.CardValue cardValue;

    public void placeCardOnTable(Vector3 moveLocation)
    {
        this.transform.position = moveLocation;
    }

    /*
    Initially planned to show movement of the cards as they moved on the table; however, it looked quite messy with all the moving parts on the screen

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
    }

    public void moveCardToDisplay(Vector3 moveLocation, float speed)
    {
        StopAllCoroutines();
        StartCoroutine(MoveCard(moveLocation, speed));
    }

    IEnumerator MoveCard(Vector3 moveLocation, float speed)
    {
        while ((this.transform.position - moveLocation).sqrMagnitude > 0.01f)
        {
            this.transform.position = Vector3.MoveTowards(transform.position, moveLocation, speed * Time.deltaTime);
            yield return null;
        }
    }
    */
}