using UnityEngine;
using UnityEngine.UI;

public enum CardType { IncreaseHandle, DecreaseHandle, ReverseDirection, Shield }

public class Card
{
    public CardType type;
    public int value;

    public Card(CardType t, int v = 0)
    {
        type = t;
        value = v;
    }
}
public class CardGameManager : MonoBehaviour
{
    public SurpriseBox surpriseBox;
    public BoxPlayer[] players;

    public int currentPlayerIndex = 0;
    public int direction = 1;

    [Header("UI")]
    public Transform cardPanel;
    public GameObject cardPrefab;
    public Text handleText;
    public Text turnText;

    void Start()
    {
        DealCards();
        UpdateUI();
    }

    void DealCards()
    {
        System.Random rnd = new System.Random();
        for (int p = 0; p < players.Length; p++)
        {
            for (int i = 0; i < 4; i++)
            {
                int typeIndex = rnd.Next(0, 4);
                CardType type = (CardType)typeIndex;
                int value = (type == CardType.IncreaseHandle || type == CardType.DecreaseHandle) ? rnd.Next(1, 4) : 0;
                players[p].AddCard(new Card(type, value));
            }
        }
    }

    public void PlayCardFromUI(int index)
    {
        BoxPlayer player = players[currentPlayerIndex];
        if (index < 0 || index >= player.handCount) return;

        Card card = player.hand[index];
        ExecuteCard(player, card);
        player.RemoveCard(index);

        NextTurn();
    }

    void ExecuteCard(BoxPlayer player, Card card)
    {
        if (card.type == CardType.IncreaseHandle) surpriseBox.ModifyHandle(card.value);
        else if (card.type == CardType.DecreaseHandle) surpriseBox.ModifyHandle(-card.value);
        else if (card.type == CardType.ReverseDirection) direction *= -1;
        else if (card.type == CardType.Shield) player.hasShield = true;

        if (surpriseBox.isOpen && player.isAlive)
        {
            if (player.hasShield)
            {
                player.hasShield = false;
                surpriseBox.handleValue = surpriseBox.handleLimit - 1;
                surpriseBox.isOpen = false;
                surpriseBox.isSpinning = true;
                Debug.Log(player.playerName + " se salvó con escudo!");
            }
            else
            {
                player.isAlive = false;
                Debug.Log(player.playerName + " murió!");
            }
        }
    }

    void NextTurn()
    {
        currentPlayerIndex += direction;
        if (currentPlayerIndex >= players.Length) currentPlayerIndex = 0;
        if (currentPlayerIndex < 0) currentPlayerIndex = players.Length - 1;

        if (!players[currentPlayerIndex].isAlive) NextTurn();

        UpdateUI();
    }

    void UpdateUI()
    {
        handleText.text = "Manija: " + surpriseBox.handleValue + "/" + surpriseBox.handleLimit;
        turnText.text = "Turno: " + players[currentPlayerIndex].playerName;

        for (int i = 0; i < cardPanel.childCount; i++) Destroy(cardPanel.GetChild(i).gameObject);

        BoxPlayer current = players[currentPlayerIndex];
        if (current.isHuman)
        {
            for (int i = 0; i < current.handCount; i++)
            {
                Card c = current.hand[i];
                GameObject obj = Instantiate(cardPrefab, cardPanel);
                obj.GetComponentInChildren<Text>().text = c.type.ToString() + (c.value > 0 ? " (" + c.value + ")" : "");
                int copyIndex = i;
                obj.GetComponent<Button>().onClick.AddListener(() => PlayCardFromUI(copyIndex));
            }
        }
    }
}
