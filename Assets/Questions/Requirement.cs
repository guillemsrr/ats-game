// Copyright (c) Guillem Serra. All Rights Reserved.

using Resume.Base;
using UnityEngine;

namespace Questions
{
    public class Requirement: MonoBehaviour
    {
        [SerializeField] private TextHandler _textHandler;

        public TextHandler Text => _textHandler;
    }
}