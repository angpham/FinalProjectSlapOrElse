using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayManager : MonoBehaviour
{
    public DeckManager deckManager;
    public SlapScoreManager slapScoreManager;
    public GameTimerManager gameTimerManager;
    public GameStatusManager gameStatusManager;
    public GoBackToMenuButtonManager goBackToMenuButtonManager;
    public GameObject tableDisplayTop; // To store locations on table
    public GameObject tableDisplayMiddle; // To store locations on table
    public GameObject tableDisplayBottom; // To store locations on table

    private ArrayList playDeck = new ArrayList(); // Stores deck of cards to modify -> Using ArrayList beacause it is easier to add/remove cards as game plays
    private Card[] table = new Card[3]; // Represents cards on the table display
    public Card tempCard1; // Placeholder
    public Card tempCard2; // Placeholder
    public Card tempCard3; // Placeholder

    public float gameTimerMaxInSeconds;
    private float currentGameTime;
    private bool gameTimerRunning = false;

    public float cardDealSpeed; // Sets how fast cards are placed on the table and how much time is given to slap

    private float slapTimerMaxInSeconds;
    private float currentSlapTime;
    bool isSlapping = false;
    bool slapTimerRunning = false;
    bool needsSlap = false;

    private int slapScore; // Logs scoring throughout game

    // Start is called before the first frame update
    void Start()
    {
        // Populating ArrayList 'playDeck' with Array 'deck' from DeckManager class
        for (int i = 0; i < deckManager.deck.Length; ++i)
        {
            this.playDeck.Add(deckManager.deck[i]);
        }

        table[0] = tempCard1; // Assign tempCard
        table[1] = tempCard2; // Assign tempCard
        table[2] = tempCard3; // Assign tempCard

        Time.fixedDeltaTime = cardDealSpeed; // Set FixedUpdate() time rate

        gameTimerRunning = true;
        currentGameTime = gameTimerMaxInSeconds;
        gameTimerManager.updateGameTimerText(gameTimerMaxInSeconds);

        slapTimerMaxInSeconds = cardDealSpeed;
        currentSlapTime = slapTimerMaxInSeconds;
    }

    void FixedUpdate()
    {
        if (gameTimerRunning)
        {
            moveCardsOnTable(); // Move cards on table and deal new card
            detectSlapOrElse(); // Detects if a slap is needed -> Jack, Double, Sandwich
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameTimerRunning)
        {
            StopAllCoroutines();
            StartCoroutine(CheckGamePlayStatus());

            if (currentGameTime < 0f)
            {
                currentGameTime = 0;
                gameTimerRunning = false;
            }
            else
            {
                currentGameTime = currentGameTime - Time.deltaTime;
                gameTimerManager.updateGameTimerText(currentGameTime);
            }
        }
    }

    Card dealRandomCard()
    {
        int randomIndex = Random.Range(0, playDeck.Count); // Get random index in 'playDeck'
        Card randomCard = (Card)playDeck[randomIndex];
        playDeck.RemoveAt(randomIndex); // Temporarily remove from playDeck, so it does not repeat on table
        return randomCard;
    }

    void moveCardsOnTable() // Handles moving cards back on the table to make room for new card deal
    {
        if (table[2].cardValue == DeckManager.CardValue.None) // To handle first 3 card deals that requires the moving 'tempCards' -> Do not want to add 'tempCards' to 'playDeck'
        {
            table[2].placeCardOnTable(deckManager.transform.position);
        }
        else
        {
            table[2].placeCardOnTable(deckManager.transform.position);
            playDeck.Add(table[2]); // Once card previously dealt has reached the end of the table, we want to add back to the 'playDeck'
        }

        table[2] = table[1];
        table[2].placeCardOnTable(tableDisplayBottom.transform.position);

        table[1] = table[0];
        table[1].placeCardOnTable(tableDisplayMiddle.transform.position);

        table[0] = dealRandomCard(); // Deal new card to table
        table[0].placeCardOnTable(tableDisplayTop.transform.position);
    }

    void detectSlapOrElse()
    {
        if (table[0].cardValue == DeckManager.CardValue.Jack) // JACK CASE
        {
            needsSlap = true;
            slapTimerRunning = true;
        }
        else if (table[0].cardValue == table[1].cardValue) // DOUBLE CASE -> Only look at first two cards on table because this method will run every time a new card is dealed in FixedUpdate() through moveCardsOnTable()
        {
            needsSlap = true;
            slapTimerRunning = true;
        }
        else if (table[0].cardValue == table[2].cardValue) // SANDWICH CASE
        {
            needsSlap = true;
            slapTimerRunning = true;
        }
        else
        {
            needsSlap = false;
            slapTimerRunning = false;
            currentSlapTime = cardDealSpeed;
        }
    }

    IEnumerator CheckGamePlayStatus()
    {
        if (Input.GetButtonDown("Jump")) // Detecting slap when 'Space' key is hit
        {
            isSlapping = true;
        }

        while (needsSlap)
        {
            if (isSlapping && slapTimerRunning) // Slapping correctly is when a slap is needed and while the slap time is running 
            {
                gameStatusManager.updateGameStatus("SLAPPED CORRECTLY!");
                slapScore = slapScore + 10;
                slapScoreManager.updateSlapScoreText(slapScore);
                currentSlapTime = slapTimerMaxInSeconds;
                needsSlap = false;
                slapTimerRunning = false;
                isSlapping = false;
                yield break;
            }

            if (!isSlapping && !slapTimerRunning) // Slap is missed when a slap is needed, but a slap did not occur during when the slap time is running
            {
                gameStatusManager.updateGameStatus("SLAP MISSED!");
                slapScore = slapScore - 5;
                slapScoreManager.updateSlapScoreText(slapScore);
                currentSlapTime = slapTimerMaxInSeconds;
                needsSlap = false;
                slapTimerRunning = false;
                yield break;
            }

            if (currentSlapTime < 0.1f) // Using 0.1f because of slight race condition with FixedUpdate()
            {
                currentSlapTime = slapTimerMaxInSeconds;
                slapTimerRunning = false;
            }
            else
            {
                currentSlapTime = currentSlapTime - Time.deltaTime; // Increment 'currentSlapTime' while currentSlapTime has not essentially reached 0
            }

            yield return null;
        }

        if (!needsSlap && !slapTimerRunning && isSlapping) // Slapping wrong is when slap is not needed, but a slap is detected
        {
            gameStatusManager.updateGameStatus("SLAPPED WRONG!");
            slapScore = slapScore - 10;
            slapScoreManager.updateSlapScoreText(slapScore);
            currentSlapTime = slapTimerMaxInSeconds;
            needsSlap = false;
            slapTimerRunning = false;
            isSlapping = false;
        }
    }
}