using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{

    public Text dialoguePugText;
    public Text dialogueHumanText;
    public GameObject nextButton;

    private Queue<Sentence> sentences;
    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<Sentence>();
        gm = FindObjectOfType<GameManager>();
    }

    public void StartDialogue (Dialogue dialogue)
    {  
        sentences.Clear();
        foreach(Sentence s in dialogue.sentences)
        {
            sentences.Enqueue(s);
        }

        gm.setDialogueActive(true);
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0){           
            nextButton.SetActive(false);
            EndDialogue();
            return;
        }

        Sentence current = sentences.Dequeue();
        clearTexts();
        StopAllCoroutines();
        nextButton.SetActive(false);
        StartCoroutine(TypeSentence(current));
        Debug.Log(current.talker == 'P' ? "Sprinkles" : "Human" + " : " + current.sentence);
    }

    void clearTexts()
    {
        dialoguePugText.text = "";
        dialogueHumanText.text = "";
    }

    IEnumerator TypeSentence (Sentence sentence)
    {
        Text d = sentence.talker == 'P' ? dialoguePugText : dialogueHumanText;
        d.text = "";
        foreach (char c in sentence.sentence.ToCharArray())
        {
            d.text += c;
            yield return new WaitForSeconds(0.05f);
        }
        nextButton.SetActive(true);
    }

    void EndDialogue()
    {
        gm.setDialogueActive(false);
        clearTexts();
        Debug.Log("End of Convo");
    }
}
