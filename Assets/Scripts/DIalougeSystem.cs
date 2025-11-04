using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    [Header("for text")]
    [SerializeField] private Dialouge[] dialouges;
    [SerializeField] private TextMeshProUGUI npcNametxt;
    [SerializeField] private TextMeshProUGUI npcConvotxt;

    [Header("for characters")]
    [SerializeField] private Image npcOneImage;
    [SerializeField] private Image npcTwoImage;
    [SerializeField] private Sprite HuhSprite;
    [SerializeField] private Sprite RabbidSprite;

    [Header("for everything else")]
    private int dialogueCount = 0;
    private bool isEven = false;
    [SerializeField] private float convoSpeed = 0.01f;

    private const string HUH = "Huh";
    private const string RABBID = "Rabbid";

    private Coroutine typingCoroutine;
    private bool isTyping = false;

    private void Start()
    {
        ProcessDialogue(0);
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (isTyping)
            {
                SkipTyping();
            }
            else if (dialogueCount < dialouges.Length - 1)
            {
                ProcessDialogue(1);
            }
        }
    }

    public void ProcessDialogue(int count)
    {
        dialogueCount += count;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        ChangeImageSprite();
        ChangeImageColor();

        npcConvotxt.gameObject.SetActive(true);
        npcNametxt.text = dialouges[dialogueCount].npcNames;
        typingCoroutine = StartCoroutine(StartDialouge(npcConvotxt));
    }

    private void ChangeImageSprite()
    {
        if (dialouges[dialogueCount].npcNames == HUH)
        {
            npcOneImage.sprite = HuhSprite;
            isEven = false;
        }
        else if (dialouges[dialogueCount].npcNames == RABBID)
        {
            npcTwoImage.sprite = RabbidSprite;
            isEven = true;
        }
    }

    private void ChangeImageColor()
    {
        if (!isEven)
        {
            npcOneImage.color = Color.white;
            npcTwoImage.color = new Color(0.5f, 0.5f, 0.5f, 1f);
        }
        else
        {
            npcTwoImage.color = Color.white;
            npcOneImage.color = new Color(0.5f, 0.5f, 0.5f, 1f);
        }
    }

    private IEnumerator StartDialouge(TextMeshProUGUI convoText)
    {
        convoText.text = "";
        isTyping = true;

        string dialogueLine = dialouges[dialogueCount].npcDialogue;

        foreach (char letter in dialogueLine.ToCharArray())
        {
            convoText.text += letter;
            yield return new WaitForSeconds(convoSpeed);
        }

        isTyping = false;
    }

    private void SkipTyping()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        npcConvotxt.text = dialouges[dialogueCount].npcDialogue;
        isTyping = false;
    }
}
