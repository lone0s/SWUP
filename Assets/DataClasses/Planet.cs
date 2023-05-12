using System;
using UnityEngine;

namespace Assets.DataClasses
{
    [Serializable]
    public class Planet : UsableObject
    {
        public float radius = 0.25F;
        public float period = 365F;
        public float rotation_days = 1;
        public Vector3 position = new (0,0,0);
        private Vector3 _position = new(0, 0, 0);
        
        // [JsonIgnore]
        public Material material = Resources.Load<Material>("Materials/Default");

        public Planet()
        {
            material.mainTexture = Texture2D.whiteTexture;
        }
        
        public Vector3 GetOldPosition()
        {
            return _position;
        }
        public void UpdateOldPosition()
        {
            _position = position;
        }
    }

    

    [Serializable]
    public class SolarSystem {
        public Planet[] planets;
    }
}