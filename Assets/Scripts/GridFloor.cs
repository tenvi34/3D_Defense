using UnityEngine;

public class GridFloor : MonoBehaviour
{
    private Material originMaterial;
    private Renderer renderer;
    private OutlineScript outlineScript;

    void Awake()
    {
        renderer = GetComponent<Renderer>();
        originMaterial = renderer.material;
        outlineScript = GetComponent<OutlineScript>();  // 동일 오브젝트의 OutlineScript 참조
    }

    private void OnMouseEnter()
    {
        outlineScript.EnableOutline();  // 윤곽선 활성화
    }
    
    private void OnMouseExit()
    {
        outlineScript.DisableOutline();  // 윤곽선 비활성화
        renderer.material = originMaterial;
    }
    
    // private Material originMaterial;
    // private Renderer renderer;
    //
    // void Awake()
    // {
    //     renderer = GetComponent<Renderer>();
    //     originMaterial = renderer.material;
    // }
    //
    // private void OnMouseEnter()
    // {
    //     OutlineScript.Instance.EnableOutline();
    // }
    //
    // private void OnMouseExit()
    // {
    //     OutlineScript.Instance.DisableOutline();
    //     renderer.material = originMaterial;
    // }
}