using System.Collections.Generic;
using UnityEngine;

namespace RandomName {
    public class NameGenerator : MonoBehaviour {
        
        public TextAsset[] fileList;
        protected Dictionary<string, string[]> nameLibrary;

        void Awake () {
            nameLibrary = new Dictionary<string, string[]> ();
            foreach(TextAsset f in fileList){
                f.text.Replace(" ","");
                string[] names = f.text.Split('\n');
                nameLibrary[f.name] = names;
            }
        }

        public string[] getName(){
            return getName(Random.value > 0.5f);
        }

        public string[] getName(bool isMale){
            string fname = isMale ? randomName("Male") : randomName("Female");
            string lname = randomName("Lastname");
            return new string[] {fname, lname};
        }

        protected string randomName(string filename){
            string[] list = nameLibrary[filename];
            return list[Random.Range(0,list.Length)];
        }

    }
}