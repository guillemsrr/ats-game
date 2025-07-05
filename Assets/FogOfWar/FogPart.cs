// Copyright (c) Guillem Serra. All Rights Reserved.

using System;
using UnityEngine;

namespace FogOfWar
{
    public class FogPart : MonoBehaviour
    {
        [SerializeField] private Collider _collider;
        [SerializeField] private ParticleSystem _fogVfx;
        [SerializeField] private GameObject _quad;

        private void Start()
        {
            _fogVfx.Stop();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player"))
            {
                return;
            }

            _collider.enabled = false;
            _quad.SetActive(false);
            ActivateFogVfx();
        }

        private void ActivateFogVfx()
        {
            if (!_fogVfx)
            {
                return;
            }

            //_fogVfx.Play();
        }
    }
}