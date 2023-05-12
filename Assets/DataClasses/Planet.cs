using System;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
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

        [JsonIgnore] public Material material;

        [JsonProperty("materialPath")]
        private string _materialPath;

        public Planet()
        {
            material = Resources.Load<Material>("Materials/Default");
            material.mainTexture = Texture2D.whiteTexture;
        }

        public void InitMaterial()
        {
            if (_materialPath != null)
            {
                material = Resources.Load<Material>(_materialPath);
            }
        }

        public void UpdateMaterialPath()
        {
            var materialPath = AssetDatabase.GetAssetPath(material);
            if (materialPath.StartsWith("Assets/Resources/"))
            {
                materialPath = materialPath["Assets/Resources/".Length..];
            }
            _materialPath = Path.ChangeExtension(materialPath, null);
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