// Copyright (c) Guillem Serra. All Rights Reserved.

using UnityEngine;
using UnityEngine.Events;

namespace Scanner
{
    public class ScanArea: MonoBehaviour
    {
        public UnityAction OnMouseEnterAction;
        public UnityAction OnMouseExitAction;

        private void OnMouseEnter()
        {
            OnMouseEnterAction?.Invoke();
        }

        private void OnMouseExit()
        {
            OnMouseExitAction?.Invoke();
        }

    }
}