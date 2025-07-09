using System;
using UnityEngine;
using UnityEngine.Events;

namespace FogOfWar
{
    public class RevealTransparencyHandler : MonoBehaviour
    {
        [SerializeField] private RenderTexture _maskRenderTexture;
        public float PaintSquareSize { get; set; } = 7.5f;
        public float PaintSquareSizeMultiplier { get; set; } = 1f;

        private Texture2D _whiteTexture;

        private Vector3? _lastRevealPoint;
        private Ray? _lastRay;
        private RaycastHit? _lastHit;

        public UnityAction OnMouseEnterAction;
        public UnityAction OnMouseExitAction;

        private void Awake()
        {
            _whiteTexture = new Texture2D(1, 1);
            _whiteTexture.SetPixel(0, 0, Color.white);
            _whiteTexture.Apply();
        }

        private void OnMouseEnter()
        {
            OnMouseEnterAction?.Invoke();
        }

        private void OnMouseExit()
        {
            OnMouseExitAction?.Invoke();
        }

        private void OnDrawGizmos()
        {
            if (_lastRevealPoint.HasValue)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(_lastRevealPoint.Value, 0.5f);
            }

            if (_lastRay.HasValue)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(_lastRay.Value.origin, _lastRay.Value.direction * 10f);
            }

            if (_lastHit.HasValue)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(_lastHit.Value.point, 0.3f);
            }
        }

        public void Initialize()
        {
            ClearRenderTextureToBlack();
        }

        public void ClearRenderTextureToBlack()
        {
            RenderTexture.active = _maskRenderTexture;
            GL.Clear(true, true, Color.black);
            RenderTexture.active = null;
        }

        public void RevealAtPoint(Vector3 worldPos)
        {
            _lastRevealPoint = worldPos;
            Ray ray = new Ray(worldPos + Vector3.up * 5f, Vector3.down); // Cast downward
            _lastRay = ray;
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                _lastHit = hit;
                RevealAtUV(hit.textureCoord);
            }
        }

        public void RevealAtUV(Vector2 uv)
        {
            int px = Mathf.FloorToInt(uv.x * _maskRenderTexture.width);
            int py = Mathf.FloorToInt((1f - uv.y) * _maskRenderTexture.height);

            float pixelSize = PaintSquareSize * PaintSquareSizeMultiplier / 2f;
            Rect rect = new Rect(px - pixelSize, py - pixelSize, PaintSquareSize * PaintSquareSizeMultiplier,
                PaintSquareSize * PaintSquareSizeMultiplier);

            RenderTexture.active = _maskRenderTexture;

            GL.PushMatrix();
            GL.LoadPixelMatrix(0, _maskRenderTexture.width, _maskRenderTexture.height, 0);
            Graphics.DrawTexture(rect, _whiteTexture);
            GL.PopMatrix();

            RenderTexture.active = null;
        }
    }
}