using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ACEOMM2
{

    public enum ProductType{Shop, Food };
    [Serializable]
    public class Product
    {
        public string productType;
        public int pricePerUnit;
        public string imagePath;
        public Color[] availableColors;
        [NonSerialized]
        public string author;
        
        public Product (string _productType, string _pricePerUnit, string _imagePath, string _color1Id, string _color2Id, string _color3Id, string _author)
        {
            productType = _productType;
            pricePerUnit = int.Parse(_pricePerUnit);
            imagePath = _imagePath;
            availableColors = new Color[2];
            ColorUtility.TryParseHtmlString(_color1Id, out availableColors[0]);
            ColorUtility.TryParseHtmlString(_color2Id, out availableColors[1]);
            ColorUtility.TryParseHtmlString(_color3Id, out availableColors[2]);
            author = _author;
        }

        public Product ()
        {

        }

        public void InstallProduct (string _productsPath)
        {
            System.IO.Directory.CreateDirectory(_productsPath + "");
        }

    }
}
