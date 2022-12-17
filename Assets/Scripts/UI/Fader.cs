using System.Collections;
using UnityEngine;

namespace UI
{
    public class Fader : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;
        private Coroutine _currentlyActiveFade;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutImmediate()
        {
            _canvasGroup.alpha = 1;
        }

        public Coroutine FadeOut(float time)
        {
            return Fade(1, time);
        }

        public Coroutine FadeIn(float time)
        {
            return Fade(0, time);
        }

        private Coroutine Fade(float target, float time)
        {
            if (_currentlyActiveFade != null)
            {
                StopCoroutine(_currentlyActiveFade);
            }

            _currentlyActiveFade = StartCoroutine(FadeRoutine(target, time));
            return _currentlyActiveFade;
        }

        private IEnumerator FadeRoutine(float target, float time)
        {
            while (!Mathf.Approximately(_canvasGroup.alpha, target))
            {
                _canvasGroup.alpha =
                    Mathf.MoveTowards(_canvasGroup.alpha, target, Time.deltaTime / time);
                yield return null;
            }
        }
    }
}