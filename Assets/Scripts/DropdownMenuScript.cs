using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class DropdownMenuScript : MonoBehaviour
    {
        private MenusScript script;
        public Func<string, string> function;

        void Start()
        {
            script = GetComponentInParent<MenusScript>();
            function = script.function;
        }

        public void OnClickItem()
        {
            var objName = gameObject.GetComponent<Text>().text;
            if (function != null && objName != null)
            {
                function.Invoke(objName);
            }

        }

        void Update()
        {

        }
    }
}
