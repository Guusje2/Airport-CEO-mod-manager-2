using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ACEOMM2
{

    [Serializable]
    public class Product
    {
        //product type/name (for example: book)
        public string productType;
        public int pricePerUnit;
        public string imagePath;
        public ColorRGBA[] availableColors;
        public bool isCustom = true;
        public bool canHaveAnyColor = false;
        [NonSerialized]
        public string author;
        [NonSerialized]
        public string imgUrl;
        
        public Product (string _productType, string _pricePerUnit, string _imgUrl, string _color1Id, string _color2Id, string _color3Id, string _color4Id, string _color5Id, string _author)
        {
            productType = _productType;
            pricePerUnit = int.Parse(_pricePerUnit);
            imgUrl = _imgUrl;
            //defaulting the colors to white. If parse fails we will at least have a colour
            availableColors = new ColorRGBA[]{ new ColorRGBA(Color.white), new ColorRGBA(Color.white), new ColorRGBA(Color.white), new ColorRGBA(Color.white), new ColorRGBA(Color.white) };
            
            //parsing the colors from the input
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

        //writes the image file to a path
        public void installProduct (string _productsPath)
        {
            System.IO.Directory.CreateDirectory(_productsPath);
            Business.DownloadFile(Path.Combine(_productsPath, productType + ".png"), imgUrl);
            imagePath = productType + ".png";
        }

    }

    /// <summary>
    /// class to easily serialize colors in the ACEO format
    /// </summary>
    [Serializable]
    public class ColorRGBA
    {
        public double r;
        public double g;
        public double b;
        public double a;

        public ColorRGBA(Color color)
        {
            // times 255 due to aceo using 0-255 and unity using 0-1
            r = color.r*255;
            g = color.g*255;
            b = color.b*255;
            a = color.a*255;
        }
    }

    /// <summary>
    /// class to hold all of the products and easily serialize them
    /// </summary>
    [Serializable]
    public class ProductsContainer
    {
        public List<Product> array;

        public ProductsContainer ()
        {
            array = new List<Product>();
        }

        public void serializeProducts (string installLocation)
        {
            Debug.Log("Installing Products");
            Directory.CreateDirectory(Path.Combine(installLocation, "Products"));
            foreach (Product product in array)
            {
                product.installProduct(Path.Combine(installLocation, "Products"));
            }
            using (StreamWriter file = new StreamWriter(Path.Combine(installLocation, "Products", "ShopProducts.json")))
            {
                file.Write(Newtonsoft.Json.JsonConvert.SerializeObject(this, new Newtonsoft.Json.JsonSerializerSettings { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore }));
            }
        }
    }
}
