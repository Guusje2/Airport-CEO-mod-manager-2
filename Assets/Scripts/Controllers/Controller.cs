using ACEOMM2;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace ACEOMM2 {
    public class Controller : MonoBehaviour
    {
        public static Controller instance;
        public List<ModPack> modPacks;
        /// <summary>
        /// folder in which the businesses will be installed
        /// </summary>
        public string defaultInstallFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "//Apoapsis Studios//Airport CEO//Mods";


        /// <summary>
        /// will be removed for a system allowing multiple mods
        /// </summary>
        public Mod currentmod;
        public void Start()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }
            modPacks = new List<ModPack>();
            currentmod = new Mod("ACEOMM", "AceoMM mod test", "Guusje2", "0.0.1");
            Application.targetFrameRate = 60;
        }
            #if UNITY_STANDALONE_WIN
        public string productsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),"Steam","steamapps","common","Airport CEO","Airport CEO_Data","DataFiles","Products");
#endif

#if UNITY_STANDALONE_OSX
        public string productsFolder = "~/Library/Application Support/Steam/steamapps/common/Airport CEO/Airport CEO/Content/DataFiles/Products";
#endif  



        public void InstallMod()
        {
            currentmod.installMod(defaultInstallFolder);
            modPacks[0].GetAllBusinessData();
        }

    }
}