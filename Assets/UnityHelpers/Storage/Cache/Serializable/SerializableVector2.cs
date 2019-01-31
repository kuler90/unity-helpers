using UnityEngine;
using System;

namespace UnityHelpers.Storage
{
    [System.Serializable]
    public struct SerializableVector2
    {
        public float x;
        public float y;

        public SerializableVector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return ((Vector2)this).ToString();
        }

        public static implicit operator Vector2(SerializableVector2 rValue)
        {
            return new Vector2(rValue.x, rValue.y);
        }

        public static implicit operator SerializableVector2(Vector2 rValue)
        {
            return new SerializableVector2(rValue.x, rValue.y);
        }
    }
}