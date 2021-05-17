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
    public GameObject tableDisplayTop;
    public GameObject TableDisplayMiddle;
    public GameObject tableDisplayBottom;

    private ArrayList playDeck = new ArrayList();
    private Card[] table = new Card[3];
    public Card tempCard1;
    public Card tempCard2;
    public Card tempCard3;

    public float gameTimerMaxInSeconds;
    private float currentGameTime;
    private bool gameTimerRunning = false;

    public float cardDealSpeed;

    private float slapTimerMaxInSeconds;
    private float currentSlapTime;
    bool isSlapping = false;
    bool slapTimerRunning = false;
    bool needsSlap = false;

    private int slapScore;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < deckManager.deck.Length; ++i)
        {
            this.playDeck.Add(deckManager.deck[i]);
        }
        table[0] = tempCard1;
        table[1] = tempCard2;
        table[2] = tempCard3;

        Time.fixedDeltaTime = cardDealSpeed;

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
            moveCardsOnTable();
            detectSlapOrElse();
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
        int randomIndex = Random.Range(0, playDeck.Count);
        Card randomCard = (Card)playDeck[randomIndex];
        playDeck.RemoveAt(randomIndex);
        return randomCard;
    }

    void moveCardsOnTable()
    {
        if (table[2].cardValue == DeckManager.CardValue.None)
        {
            table[2].placeCardOnTable(deckManager.transform.position);
        }
        else
        {
            table[2].placeCardOnTable(deckManager.transform.position);
            playDeck.Add(table[2]);
        }

        table[2] = table[1];
        table[2].placeCardOnTable(tableDisplayBottom.transform.position);

        table[1] = table[0];
        table[1].placeCardOnTable(TableDisplayMiddle.transform.position);

        table[0] = dealRandomCard();
        table[0].placeCardOnTable(tableDisplayTop.transform.position);
    }

    void detectSlapOrElse()
    {
        if (table[0].cardValue == DeckManager.CardValue.Jack)
        {
            needsSlap = true;
            slapTimerRunning = true;
        }
        else if (table[0].cardValue == table[1].cardValue)
        {
            needsSlap = true;
            slapTimerRunning = true;
        }
        else if (table[0].cardValue == table[2].cardValue)
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
        if (Input.GetButtonDown("Jump"))
        {
            isSlapping = true;
        }

        while (needsSlap)
        {
            if (isSlapping && slapTimerRunning)
            {
                gameStatusManager.updateGameStatus("SLAPPED CORRECTLY!");
                slapScore = slapScore + 10;
                slapScoreManager.updateSlapScoreText(slapScore);
                currentSlapTime = cardDealSpeed;
                needsSlap = false;
                slapTimerRunning = false;
                isSlapping = false;
                yield break;
            }

            if (!isSlapping && !slapTimerRunning)
            {
                gameStatusManager.updateGameStatus("SLAP MISSED!");
                slapScore = slapScore - 5;
                slapScoreManager.updateSlapScoreText(slapScore);
                currentSlapTime = cardDealSpeed;
                needsSlap = false;
                slapTimerRunning = false;
                yield break;
            }

            if (currentSlapTime < 0.1f)
            {
                currentSlapTime = cardDealSpeed;
                slapTimerRunning = false;
            }
            else
            {
                currentSlapTime = currentSlapTime - Time.deltaTime;
            }

            yield return null;
        }

        if (!needsSlap && !slapTimerRunning && isSlapping)
        {
            gameStatusManager.updateGameStatus("SLAPPED WRONG!");
            slapScore = slapScore - 10;
            slapScoreManager.updateSlapScoreText(slapScore);
            currentSlapTime = cardDealSpeed;
            needsSlap = false;
            slapTimerRunning = false;
            isSlapping = false;
        }
    }
}