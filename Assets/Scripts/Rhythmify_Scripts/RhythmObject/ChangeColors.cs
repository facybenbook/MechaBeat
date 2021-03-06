﻿using UnityEngine;
using System.Collections;

namespace Rhythmify {
    [System.Serializable]
    public class ColorTransition {
        public Color startColor;
        public Color endColor;
        public bool smooth;
    }

    public class ChangeColors : _AbstractRhythmObject {
        public ColorTransition[] colorTransitions;
        public int[] indices;
        public int offset;
        public bool shared;

        override protected void rhythmUpdate() {
            int size = colorTransitions.Length;

            if (size < 0) {
                return;
            }

            int idx = getBeat() + offset;
            if (indices.Length > 0) {
                idx = indices [idx % indices.Length];
            }

            if (idx < 0) {
                return;
            }

            ColorTransition t = colorTransitions[idx % size];

            if (t.smooth) {
                StartCoroutine(changeSmooth(t.startColor, t.endColor, secondsPerBeat));
            }
            else {
                if (shared) {
                    renderer.sharedMaterial.color = t.endColor;
                }
                else {
                    renderer.material.color = t.endColor;
                }
            }
        }

        private IEnumerator changeSmooth(Color startColor, Color endColor, float duration) {
            float startTime = Time.time;
            while (Time.time <= startTime + duration) {
                float lerpPercent = Mathf.Clamp01((Time.time - startTime) / duration);
                if (shared) {
                    renderer.sharedMaterial.color = Color.Lerp(startColor, endColor, lerpPercent);
                }
                else {
                    renderer.material.color = Color.Lerp(startColor, endColor, lerpPercent);
                }
                yield return null;
            }
        }
    }
}
