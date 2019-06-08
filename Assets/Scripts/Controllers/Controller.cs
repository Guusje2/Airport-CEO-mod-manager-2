using ACEOMM2;
using System.Collections.Generic;
using UnityEngine;


namespace ACEOMM2 {
    public class Controller : MonoBehaviour
    {
        public List<ModPack> modPacks;
        public string InstallFolder = " C:\\My Documents\\ACEO tests\\";
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