using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

namespace Vampwolf
{
    [System.Serializable]
    public class DialogueLine
    {
        public string speakerName;
        [TextArea(2, 5)] public string line;
    
    }
    public class DialogueManager : MonoBehaviour
    {
        [Header("UI Elements")]
        public GameObject dialoguePanel;
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI dialogueText;
        public TextMeshProUGUI continueText;

        [Header("Dialogue")]
        public List<DialogueLine> lines;
        public float typeSpeed = 0.04f;

        private int currentLineIndex = 0;
        private bool isTyping = false;
        private bool waitingForInput = false;

        void Start()
        {
            if (lines.Count > 0)
            {
                StartCoroutine(PlayDialogue());
            }
        }

        IEnumerator PlayDialogue()
        {
            dialoguePanel.SetActive(true);
            yield return StartCoroutine(ShowLine());
        }

        IEnumerator ShowLine()
        {
            isTyping = true;
            waitingForInput = false;
            continueText.gameObject.SetActive(false);

            DialogueLine current = lines[currentLineIndex];
            nameText.text = current.speakerName;
            dialogueText.text = "";

            foreach (char c in current.line)
            {
                dialogueText.text += c;
                yield return new WaitForSeconds(typeSpeed);
            }

            isTyping = false;
            waitingForInput = true;
            continueText.gameObject.SetActive(true);
        }

        void Update()
        {
            if (waitingForInput && Keyboard.current.eKey.wasPressedThisFrame)
            {
                continueText.gameObject.SetActive(false);
                currentLineIndex++;

                if (currentLineIndex < lines.Count)
                {
                    StartCoroutine(ShowLine());
                }
                else
                {
                    dialoguePanel.SetActive(false);
                }
            }
        }
    }
}
