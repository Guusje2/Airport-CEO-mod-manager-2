using ACEOMM2;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace ACEOMM2 {
    public class Controller : MonoBehaviour
    {
        public List<ModPack> modPacks;
        public string InstallFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "//Apoapsis Studios//Airport CEO//Mods";
        /// <summary>
        /// will be removed for a system allowing multiple mods
        /// </summary>
        public Mod currentmod;
        public void Start()
        {
            modPacks = new List<ModPack>();
            currentmod = new Mod("ACEOMM", "AceoMM mod test", "Guusje2", "0.0.1");
        }

        public void InstallMod()
        {
            currentmod.CreateModDataJSON(InstallFolder);
        }

    }
}