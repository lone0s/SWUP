using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;


namespace Assets.DataClasses
{
    [Serializable]
    public class UsableObject
    {
        private static int _nbObject = 1;
        
        public string name = "Object " + _nbObject++;
        private string _name;
        
        public Dictionary<string, UsableObject> Children = new ();

        public string GetOldName()
        {
            return _name;
        }
        public void UpdateOldName()
        {
            _name = name;
        }
    }

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
