using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class DropdownMenuScript : MonoBehaviour
    {
        private MenusScript script;
        public Action<string> function;

        void Start()
        {
            /*script = GetComponentInParent<MenusScript>();
            function = script.Function;*/
        }

        public void OnClickItem()
        {
            var objName = gameObject.GetComponent<Text>().text;
            if (function != null && objName != null)
            {
                function.Invoke(objName);
            }
            else
            {
                Debug.LogError("pas bind");
            }

        }

        void Update()
        {

        }
    }
}
