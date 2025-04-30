using DG.Tweening;
using System;
using UnityEngine.UI;

namespace Vampwolf.Utilities.Typewriter
{
    public class Typewriter : IDisposable
    {
        private readonly Text text;
        private float characterSpeed;
        private Tween typeTween;

        public Typewriter(Text text, float characterSpeed)
        {
            this.text = text;
            this.characterSpeed = characterSpeed;
        }

        /// <summary>
        /// Type a text out using the typewriter
        /// </summary>
        public void Write(string textToType, TweenCallback onComplete = null)
        {
            // Kill the type sequence if it exists
            typeTween?.Kill();

            // Clear the text
            text.text = string.Empty;

            // Compute the total duration
            float totalDuration = textToType.Length * characterSpeed;

            // Set the type tween
            typeTween = text.DOText(textToType, totalDuration, true).SetEase(Ease.Linear);

            // Exit case - there's no completion action
            if (onComplete == null) return;

            // Set the completion action
            typeTween.OnComplete(onComplete);
        }

        /// <summary>
        /// Clear the typewriter
        /// </summary>
        public void Clear()
        {
            // Kill the type sequence if it exists
            typeTween?.Kill();

            // Clear the text
            text.text = string.Empty;
        }

        /// <summary>
        /// Handle the disposing of the Typewriter
        /// </summary>
        public void Dispose()
        {
            // Kill the type tween if it exists
            typeTween?.Kill();
        }
    }
}
