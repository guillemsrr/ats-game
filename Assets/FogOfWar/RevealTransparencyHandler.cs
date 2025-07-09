using System;
using UnityEngine;

namespace FogOfWar
{
    public class RevealTransparencyHandler : MonoBehaviour
    {
        [SerializeField] private RenderTexture _maskRenderTexture;
        [SerializeField] private int _paintSquareSize = 10;
        
        private Texture2D _whiteTexture;

        private void Awake()
        {
            _whiteTexture = new Texture2D(1, 1);
            _whiteTexture.SetPixel(0, 0, Color.white);
            _whiteTexture.Apply();
        }

        public void Initialize()
        {
            ClearRenderTextureToBlack();
        }

        private void ClearRenderTextureToBlack()
        {
            RenderTexture.active = _maskRenderTexture;
            GL.Clear(true, true, Color.black);
            RenderTexture.active = null;
        }

        public void RevealAtPoint(Vector3 worldPos)
        {
            Ray ray = new Ray(worldPos + Vector3.up * 5f, Vector3.down); // Cast downward
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                RevealAtUV(hit.textureCoord);
            }
        }

        public void RevealAtUV(Vector2 uv)
        {
            if (_maskRenderTexture == null || _whiteTexture == null) return;

            // Flip Y because RenderTextures have bottom-left origin
            int px = Mathf.FloorToInt(uv.x * _maskRenderTexture.width);
            int py = Mathf.FloorToInt((1f - uv.y) * _maskRenderTexture.height);

            Rect rect = new Rect(px - _paintSquareSize / 2, py - _paintSquareSize / 2, _paintSquareSize,
                _paintSquareSize);

            RenderTexture.active = _maskRenderTexture;

            GL.PushMatrix();
            GL.LoadPixelMatrix(0, _maskRenderTexture.width, _maskRenderTexture.height, 0);
            Graphics.DrawTexture(rect, _whiteTexture);
            GL.PopMatrix();

            RenderTexture.active = null;
        }

        private void OnMouseEnter()
        {
            Debug.Log("On mouse enter");
        }

        private void OnMouseExit()
        {
            Debug.Log("On mouse exit");
        }
    }
}