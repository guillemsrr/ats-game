// Copyright (c) Guillem Serra. All Rights Reserved.

using GameJamBase.UI.View;
using Menu;
using UnityEngine;

namespace Questions
{
    public class Requirement: MonoBehaviour
    {
        [SerializeField] private TextHandler _textHandler;

        public TextHandler Text => _textHandler;
    }
}