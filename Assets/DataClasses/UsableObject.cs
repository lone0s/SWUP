using System;
using System.Collections.Generic;
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

        // public Material material = new Material(Shader.Find("Standard"));
        public Material material;

        public Planet()
        {
            var myMaterial = Resources.Load<Material>("Materials/Default");
            var newMaterial = new Material(myMaterial);

            // Create a new asset file for the material
            var path = "Assets/Resources/Out/" + name.Replace(" ", "_") + ".mat";
            AssetDatabase.CreateAsset(newMaterial, path);

            // Save changes to the asset database
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            var newMaterialInstance = Resources.Load<Material>("Out/" + name.Replace(" ", "_"));
            Debug.Log(newMaterialInstance);
            // Create a new material based on the Standard shader

            // Load a new texture

            // Set the texture of the new material to the new texture
                        //myMaterial.SetTexture("_MainTex", Texture2D.whiteTexture);

            // Set any other properties of the new material as needed
                        //myMaterial.color = Color.red;

            // Assign the new material to a Renderer component
            //GetComponent<Renderer>().material = myMaterial;
            material = newMaterialInstance;
        }
    }

    

    [Serializable]
    public class SolarSystem {
        public Planet[] planets;
    }
}
