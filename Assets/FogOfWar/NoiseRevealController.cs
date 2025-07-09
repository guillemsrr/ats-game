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

        private int _seed;
        private float _seedOffset;

        private Coroutine _noiseReavealCoroutine;

        private void Awake()
        {
            _seed = Random.Range(0, int.MaxValue);
            System.Random rng = new System.Random(_seed);
            _seedOffset = (float) rng.NextDouble(); // use per-cell instead of Perlin
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

                    float noise = Mathf.PerlinNoise(u * _noiseScale + _seedOffset, v * _noiseScale + _seedOffset);
                    if (noise > _threshold)
                    {
                        _revealHandler.RevealAtUV(new Vector2(u, v));
                        yield return new WaitForSeconds(_delayBetweenReveals);
                    }
                }
            }
        }

        public void FullReveal()
        {
            _revealHandler.PaintSquareSize = 10f;

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