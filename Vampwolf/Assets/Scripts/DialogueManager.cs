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
        public List<DialogueLine> level1CompleteLines;
        public List<DialogueLine> level2CompleteLines;
        private List<DialogueLine> currentLines;
        public float typeSpeed = 0.04f;

        private int currentLineIndex = 0;
        private bool isTyping = false;
        private bool waitingForInput = false;

        void Start()
        {
            if (lines.Count > 0 && !ProgressTracker.Instance.level1Complete && !ProgressTracker.Instance.level2Complete)
            {
                UpdateCurrentLines(lines);
                StartCoroutine(PlayDialogue());
            }
            else if (level1CompleteLines.Count > 0 && ProgressTracker.Instance.level1Complete && !ProgressTracker.Instance.level2Complete)
            {
                UpdateCurrentLines(level1CompleteLines);
                StartCoroutine(PlayDialogue());
            }
            else if (level2CompleteLines.Count > 0 && ProgressTracker.Instance.level1Complete && ProgressTracker.Instance.level2Complete)
            {
                UpdateCurrentLines(level2CompleteLines);
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

            DialogueLine current = currentLines[currentLineIndex];
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

        private void UpdateCurrentLines(List<DialogueLine> newLines)
        {
            currentLines = newLines;
        }

        void Update()
        {
            if (waitingForInput && UnityEngine.Input.GetMouseButtonDown(0))
            {
                continueText.gameObject.SetActive(false);
                currentLineIndex++;

                if (currentLineIndex < currentLines.Count)
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
