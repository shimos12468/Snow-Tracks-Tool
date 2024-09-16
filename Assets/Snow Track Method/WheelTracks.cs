using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelTracks : MonoBehaviour
{

    public Shader drawShader;
    public GameObject terrain;
    
    public Transform[] wheel;
    
    
    private Material drawMaterial;
    private Material snowMaterial;
    private RenderTexture splatMap;

    [Range(1, 500)]
    public float brushSize = 300;
    [Range(0, 1)]
    public float brushStrength = 1;

    public RaycastHit groundHit;
    int layerMask;
    // Start is called before the first frame update
    void Start()
    {
        layerMask = LayerMask.GetMask("Ground");
        drawMaterial = new Material(drawShader);

        //drawMaterial.SetVector("_Color", Color.red);

        snowMaterial = terrain.GetComponent<MeshRenderer>().material;
        snowMaterial.SetTexture("_SplatTex", splatMap = new RenderTexture(1024,1024,0,RenderTextureFormat.ARGBFloat));
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < wheel.Length; i++)
        {
            if (Physics.Raycast(wheel[i].position,Vector3.down, out groundHit,1,layerMask))
            {

                drawMaterial.SetVector("_Coordinate", new Vector4(groundHit.textureCoord.x, groundHit.textureCoord.y, 0, 0));
                drawMaterial.SetFloat("_Strength", brushStrength);
                drawMaterial.SetFloat("_Size", brushSize);
                RenderTexture temp = RenderTexture.GetTemporary(splatMap.width, splatMap.height, 0, RenderTextureFormat.ARGBFloat);
                Graphics.Blit(splatMap, temp);
                Graphics.Blit(temp, splatMap, drawMaterial);
                RenderTexture.ReleaseTemporary(temp);



            }
        }
    }
}
