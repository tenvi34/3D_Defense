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

    private void OnMouseDown()
    {
        Debug.Log("마우스 클릭");
        MyPlayerController.Instance.GetNewCharacter(transform.position + Vector3.up * 2, Quaternion.identity);
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
    
    // 원본
    //
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