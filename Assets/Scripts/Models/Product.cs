using System;
using System.Collections.Generic;
using System.IO;
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
        public ColorRGBA[] availableColors;
        public bool isCustom = true;
        public bool canHaveAnyColor = false;
        [NonSerialized]
        public string author;
        
        public Product (string _productType, string _pricePerUnit, string _imagePath, string _color1Id, string _color2Id, string _color3Id, string _color4Id, string _color5Id, string _author)
        {
            productType = _productType;
            pricePerUnit = int.Parse(_pricePerUnit);
            imagePath = _imagePath;
            availableColors = new ColorRGBA[]{ new ColorRGBA(Color.white), new ColorRGBA(Color.white), new ColorRGBA(Color.white), new ColorRGBA(Color.white), new ColorRGBA(Color.white) };
            
            ColorUtility.TryParseHtmlString(_color1Id, out Color a);
            availableColors[0] = new ColorRGBA(a);
            //Debug.Log("Color 1 for " + productType + " is: " + a);
            ColorUtility.TryParseHtmlString(_color2Id, out Color b);
            availableColors[1] = new ColorRGBA(b);
            ColorUtility.TryParseHtmlString(_color3Id, out Color c);
            availableColors[2] = new ColorRGBA(c);
            ColorUtility.TryParseHtmlString(_color4Id, out Color d);
            availableColors[3] = new ColorRGBA(d);
            ColorUtility.TryParseHtmlString(_color5Id, out Color e);
            availableColors[4] = new ColorRGBA(e);

            author = _author;
            
        }

        public Product ()
        {

        }

        public void InstallProduct (string _productsPath)
        {
            System.IO.Directory.CreateDirectory(_productsPath);
            Business.DownloadFile(Path.Combine(_productsPath, productType + ".png"), imagePath);
            imagePath = productType + ".png";
        }

    }

    public class ColorRGBA
    {
        public double r;
        public double g;
        public double b;
        public double a;

        public ColorRGBA(Color color)
        {
            
            r = color.r;
            g = color.g;
            b = color.b;
            a = color.a;
        }
    }
}
