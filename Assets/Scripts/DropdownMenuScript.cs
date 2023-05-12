using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class DropdownMenuScript : MonoBehaviour
    {
        private MenusScript script;
        private Action<string,bool> function;

        void Start()
        {
            script = GetComponentInParent<MenusScript>();
            function = script.Function;
        }

        public void OnClickItem()
        {
            var objName = gameObject.GetComponent<Text>().text;
            if (function != null && objName != null)
            {
                function.Invoke(objName,true);
            }

        }

        void Update()
        {

        }
    }
}
