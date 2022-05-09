using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SlashImageController : MonoBehaviour
{
    [SerializeField]
    public BoxCollider2D collider_;

    private Image image;

    [SerializeField] private RectTransform child_rect_transform;

    // Start is called before the first frame update
    void Start()
    {
        var color = GetComponent<Image>().color;
        image = GetComponent<Image>();
        image.color = new Color(color.r, color.g, color.b, 225.0f);
        //Debug.Log(gameObject.GetComponent<BoxCollider2D>().size);

        //collider_ = GetComponent<BoxCollider2D>();
    }
    

    // Update is called once per frame
    void Update()
    {
      
        if (image == null) return;
        
        var color   = image.color;
        if (color.a > 0.1f)
        {
            float alpha = Mathf.Lerp(color.a, 0.0f, 10.0f * Time.deltaTime);
            image.color = new Color(color.r, color.g, color.b, alpha);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public Ray SlashRay()
    {
        return Camera.main.ScreenPointToRay(child_rect_transform.position);
    }

    public float RadianAngle2D()
    {
        return image.rectTransform.eulerAngles.z;
    }

}
