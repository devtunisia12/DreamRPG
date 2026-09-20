using UnityEngine;
using UnityEngine.UI;

public class ScrollingUIImage : MonoBehaviour
{
    private Image image;
    private Material mat;

    [Range(0f, 0.5f)]
    public float speed = 0.2f;

    private float distance;

    void Start()
    {
        image = GetComponent<Image>();

        mat = new Material(image.material);
        image.material = mat;
    }

    void Update()
    {
        distance += Time.deltaTime * speed;

        mat.SetTextureOffset("_MainTex", Vector2.right * distance);
    }

    void OnDestroy()
    {
        if (mat != null)
            Destroy(mat);
    }
}