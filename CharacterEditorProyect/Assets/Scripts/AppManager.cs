using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AppManager : MonoBehaviour
{
    public enum Type { TEXTURE, ACCESORIES }

    public Transform GridCategory;
    public Transform GridElements;
    public GameObject BaseElements;

    public List<GameObject> Bones;

    GameObject[] SaveObj = new GameObject[4];

    int[] SaveTextures;
    int[] SaveAccessories;



    void Start()
    {
        PlayerPrefs.DeleteAll();
        PrintCategories();
    }



    void PrintCategories()
    {
        //!--- Cargo sprites de carpeta Categories ---!\\
        int CountCategories = Resources.LoadAll<Sprite>("Categories/").Length;

        //!--- Instancio objeto en grid, asigno imagenes y función al boton ---!\\
        for (int i = 0; i < CountCategories; i++)
        {
            GameObject Category = Instantiate(BaseElements, GridCategory);
            Category.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Categories/" + i);
            int tmpInt = i;
            Type _type = Type.TEXTURE;
            Category.GetComponent<Button>().onClick.AddListener(delegate { PrintElements(tmpInt, _type); });
        }

        //!--- Cargo sprites de carpeta CatAccesories ---!\\
        int CountCatAccesories = Resources.LoadAll<Sprite>("CatAccesories/").Length;

        //!--- Instancio objeto en grid, asigno imagenes y función al boton ---!\\
        for (int i = 0; i < CountCatAccesories; i++)
        {
            GameObject Accesory = Instantiate(BaseElements, GridCategory);
            Accesory.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("CatAccesories/" + i);
            int tmpInt = i;
            Type _type = Type.ACCESORIES;
            Accesory.GetComponent<Button>().onClick.AddListener(delegate { PrintElements(tmpInt, _type); });
        }

        SaveTextures = new int[CountCategories];
        SaveAccessories = new int[CountCatAccesories];
        for (int i = 0; i < SaveAccessories.Length; i++)
        {
            SaveAccessories[i] = -1;
        }
        LoadData();
    }

    void PrintElements(int i, Type _type)
    {
        //!--- Destruyo elementos para cargar nuevos ---!\\
        for (int g = GridElements.childCount - 1; g >= 0; g--)
        {
            Destroy(GridElements.GetChild(g).gameObject);
        }

        if (_type == Type.TEXTURE)
        {
            //!--- Cargo sprites de carpeta Elements ---!\\
            int CountElements = Resources.LoadAll<Sprite>("Elements/" + i).Length;

            //!--- Instancio objeto en grid, asigno imagenes y función al boton ---!\\
            for (int j = 0; j < CountElements; j++)
            {
                GameObject Element = Instantiate(BaseElements, GridElements);
                Element.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Elements/" + i + "/" + j);
                int tmpi = i;
                int tmpj = j;
                Type tmpType = _type;
                Element.GetComponent<Button>().onClick.AddListener(delegate { PrintTexture(tmpi, tmpj, tmpType); });
            }
        }
        else if (_type == Type.ACCESORIES)
        {
            //!--- Instancio botón empty para borrar accesorio ---!\\
            GameObject EmptyAccesory = Instantiate(BaseElements, GridElements);
            int tmpEmpty = i;
            EmptyAccesory.GetComponent<Button>().onClick.AddListener(delegate { PrintTexture(tmpEmpty, -1, Type.ACCESORIES); });

            //!--- Cargo sprites de carpeta ElemAccesories ---!\\
            int CountElements = Resources.LoadAll<Sprite>("ElemAccesories/" + i).Length;

            //!--- Instancio objeto en grid, asigno imagenes y función al boton ---!\\
            for (int j = 0; j < CountElements; j++)
            {
                GameObject Element = Instantiate(BaseElements, GridElements);
                Element.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("ElemAccesories/" + i + "/" + j);
                int tmpi = i;
                int tmpj = j;
                Type tmpType = _type;
                Element.GetComponent<Button>().onClick.AddListener(delegate { PrintTexture(tmpi, tmpj, tmpType); });
            }

        }
    }

    void PrintTexture(int i, int j, Type _type, bool Loading = false)
    {
        if (_type == Type.TEXTURE)
        {
            //!--- Cargo Material ---!\\
            Material NewMat = Resources.Load<Material>("Materials/" + i);
            //!--- Cargo Textura ---!\\
            Texture2D NewText = Resources.Load<Texture2D>("Textures/" + j);
            //!--- Cargo NormalMap ---!\\
            Texture2D NewNormal = Resources.Load<Texture2D>("Textures/" + j + "/Normal");

            //!--- Asigno Textura al Material---!\\
            NewMat.SetTexture("_MainTex", NewText);
            //!--- Asigno Textura al Material---!\\
            NewMat.SetTexture("_BumpMap", NewNormal);
            //print("Categoria " + i + " Elemento " + j + " Tipo" + _type);

            SaveTextures[i] = j;
        }
        else if (_type == Type.ACCESORIES)
        {
            Destroy(SaveObj[i]);
            if (j != -1)
            {
                //!--- Cargo FBXs ---!\\
                GameObject NewObj = Resources.Load<GameObject>("Accesories/" + i + "/" + j);
                print(NewObj);
                //!--- Instancio Accesorio ---!\\
                GameObject NewInstance = Instantiate(NewObj, Bones[i].transform);
                SaveObj[i] = NewInstance;
            }
            SaveAccessories[i] = j;
        }

        if (Loading == false)
        {
            SaveData();
        }
    }

    void SaveData()
    {
        PlayerPrefsX.SetIntArray("Textures", SaveTextures);
        PlayerPrefsX.SetIntArray("Accessories", SaveAccessories);
    }

    void LoadData()
    {
        if (PlayerPrefs.HasKey("Textures"))
        {
            SaveTextures = PlayerPrefsX.GetIntArray("Textures");

            for (int i = 0; i < SaveTextures.Length; i++)
                PrintTexture(i, SaveTextures[i], Type.TEXTURE, true);
        }

        if (PlayerPrefs.HasKey("Accessories"))
        {
            SaveAccessories = PlayerPrefsX.GetIntArray("Accessories");

            for (int i = 0; i < SaveAccessories.Length; i++)
                PrintTexture(i, SaveAccessories[i], Type.ACCESORIES, true);
        }
    }
}
