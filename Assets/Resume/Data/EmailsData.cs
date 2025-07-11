// Copyright (c) Guillem Serra. All Rights Reserved.

using UnityEngine;

namespace Resume.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Resume/Emails")]
    public class EmailsData : ScriptableObject
    {
        public string[] Emails;
    }
}