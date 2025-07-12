// Copyright (c) Guillem Serra. All Rights Reserved.

using Level;
using TMPro;
using UnityEngine;

namespace HUD
{
    public class HUDController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _seedText;

        private void Start()
        {
            _seedText.text = GameManager.Instance.Seed.ToString();
        }
    }
}