using System;
using System.Collections.Generic;


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
}
