using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uvscroll : MonoBehaviour
{
    private Mesh myMesh;

    Renderer r;

    [SerializeField]
    float alpha = 0.75f;
    [SerializeField]
    Vector2 scroolSpeed = new Vector2 ( 0.01f,0f );
    [SerializeField]
    bool RightMove = true;
    void Start()
    {
        // ílïœçXèàóù
        r = gameObject.GetComponent<Renderer>();

        if (RightMove) scroolSpeed.x *= -1;

        myMesh = gameObject.GetComponent<MeshFilter>().mesh;
    }
    void Update()
    {
        Vector2[] nUV = myMesh.uv;
        for (int i = 0; i < myMesh.uv.Length; i++)
        {
            nUV[i].x += Time.deltaTime * scroolSpeed.x;
            nUV[i].y += Time.deltaTime * scroolSpeed.y;
        }
        myMesh.uv = nUV; 
        r.material.color = new Color(r.material.color.r, r.material.color.g, r.material.color.b, alpha);
        //r.material.color = new Color(BaseColor.r, BaseColor.g, BaseColor.b, alpha);
    }
}
