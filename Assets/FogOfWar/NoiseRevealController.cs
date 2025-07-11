// Copyright (c) Guillem Serra. All Rights Reserved.

namespace FogOfWar
{
    using UnityEngine;

    public class NoiseRevealController : MonoBehaviour
    {
        [SerializeField] private RevealTransparencyHandler _revealHandler;
        [SerializeField] private int _gridResolution = 32;
        [SerializeField] private float _delayBetweenReveals = 0.0025f;

        private float _noiseScale;
        private float _threshold;

        private float _seedOffset;

        private Coroutine _noiseReavealCoroutine;

        private void Start()
        {
            _seedOffset = Random.Range(1, 5) / 1000f;
        }

        public void StartReveal(float noiseScale = 5f, float threshold = 0.5f)
        {
            _noiseScale = noiseScale;
            _threshold = threshold;

            if (_noiseReavealCoroutine != null)
            {
                StopCoroutine(_noiseReavealCoroutine);
            }

            _noiseReavealCoroutine = StartCoroutine(RevealNoisePattern());
        }

        private System.Collections.IEnumerator RevealNoisePattern()
        {
            yield return new WaitForSeconds(0.5f);

            for (int y = 0; y < _gridResolution; y++)
            {
                for (int x = 0; x < _gridResolution; x++)
                {
                    float u = x / (float) (_gridResolution - 1);
                    float v = y / (float) (_gridResolution - 1);

                    float noise = Mathf.PerlinNoise(u * _noiseScale * GetRandomNoiseValue() + _seedOffset, v *
                        _noiseScale *
                        GetRandomNoiseValue() + _seedOffset);
                    if (noise > _threshold)
                    {
                        _revealHandler.RevealAtUV(new Vector2(u, v));
                        yield return new WaitForSeconds(_delayBetweenReveals);
                    }
                }
            }
        }

        float GetRandomNoiseValue()
        {
            return Random.Range(0.98f, 1.1f);
        }

        public void FullReveal()
        {
            _revealHandler.PaintSquareSize = 15f;

            if (_noiseReavealCoroutine != null)
            {
                StopCoroutine(_noiseReavealCoroutine);
            }

            _noiseReavealCoroutine = StartCoroutine(RevealFullPattern());
        }

        private System.Collections.IEnumerator RevealFullPattern()
        {
            for (int y = 0; y < _gridResolution; y++)
            {
                for (int x = 0; x < _gridResolution; x++)
                {
                    float u = x / (float) (_gridResolution - 1);
                    float v = y / (float) (_gridResolution - 1);

                    _revealHandler.RevealAtUV(new Vector2(u, v));
                    yield return new WaitForSeconds(_delayBetweenReveals);
                }
            }
        }
    }
}