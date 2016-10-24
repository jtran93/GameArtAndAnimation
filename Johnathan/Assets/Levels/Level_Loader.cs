using UnityEngine;
using System.Collections;

[System.Serializable]
public class ColorToPrefab
{
    public Color32 color;
    public GameObject prefab;
}

public class Level_Loader : MonoBehaviour
{
    public Texture2D levelMap;

    public ColorToPrefab[] colorToPrefab;


    void Start()
    {
        LoadMap();
    }

    void EmptyMap()
    {
        while (transform.childCount > 0)
        {
            Transform c = transform.GetChild(0);
            c.SetParent(null);
            Destroy(c.gameObject);
        }
    }

    void LoadMap()
    {
        EmptyMap();

        Color32[] allPixels = levelMap.GetPixels32();
        int width = levelMap.width;
        int height = levelMap.height;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {

                TileSpawner(allPixels[(j * width) + i], i, j);

            }
        }
    }

    void TileSpawner(Color32 c, int i, int j)
    {

        if (c.a <= 0)
        {
            return;
        }

        foreach (ColorToPrefab ctp in colorToPrefab)
        {

            if (c.Equals(ctp.color))
            {
                GameObject go = (GameObject)Instantiate(ctp.prefab, new Vector3(i, j, 0), Quaternion.identity);
                go.transform.SetParent(this.transform);
                return;
            }
        }

        Debug.LogError("No color to prefab found for: " + c.ToString());

    }

}