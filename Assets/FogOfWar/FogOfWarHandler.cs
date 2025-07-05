// Copyright (c) Guillem Serra. All Rights Reserved.

using System;
using UnityEngine;

namespace FogOfWar
{
    public class FogOfWarHandler : MonoBehaviour
    {
        [SerializeField] private int _columns = 10;
        [SerializeField] private int _rows = 50;
        [SerializeField] private FogPart _fogPartPrefab;
        [SerializeField] private float _space = 2f;

        private void Start()
        {
            for (int column = 0; column < _columns; column++)
            {
                for (int row = 0; row < _rows; row++)
                {
                    Vector3 position = new Vector3(column, 0, -row);
                    position *= _space;
                    position += transform.position;

                    var fogPart = Instantiate(_fogPartPrefab, position, Quaternion.identity);
                    fogPart.transform.SetParent(transform);
                }
            }
        }
    }
}