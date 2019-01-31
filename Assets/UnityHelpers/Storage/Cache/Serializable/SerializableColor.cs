using UnityEngine;
using System;

namespace UnityHelpers.Storage
{
    [System.Serializable]
    public struct SerializableColor
    {
        public float r;
        public float g;
        public float b;
        public float a;

        public SerializableColor(float r, float g, float b, float a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public override string ToString()
        {
            return ((Color)this).ToString();
        }

        public static implicit operator Color(SerializableColor rValue)
        {
            return new Color(rValue.r, rValue.g, rValue.b, rValue.a);
        }

        public static implicit operator SerializableColor(Color rValue)
        {
            return new SerializableColor(rValue.r, rValue.g, rValue.b, rValue.a);
        }
    }
}