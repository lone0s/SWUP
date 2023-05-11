using System.Collections.Generic;
using UnityEngine;


namespace Assets.DataClasses
{
    [System.Serializable]
    public class UsableObject
    {
        public string name = "Object";
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

    [System.Serializable]
    public class Planet : UsableObject
    {
        public float radius = 0.25F;
        public float period = 365F;
        public float rotation_days = 1;
        public Vector3 position = new (0,0,0);

        public Material material;
    }


    [System.Serializable]
    public class SolarSystem {
        public Planet[] planets;
    }
}
