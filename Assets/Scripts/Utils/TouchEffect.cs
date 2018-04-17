using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchEffect : MonoBehaviour {

    // タップエフェクト
    [SerializeField]
    ParticleSystem _tapEffect;

    void Update()
    {
       if(CanMakeEffect())
        {
            // エフェクトが作れるとき作成
            CreateeEffect();
        }
    }

    /// <summary>
    /// エフェクトが作れるか？
    /// </summary>
    /// <returns></returns>
    private bool CanMakeEffect()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (GetTouchUIObjectHaveComponent<UnityEngine.UI.Button>())
            {
                return false;
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// エフェクトの作成
    /// </summary>
    private void CreateeEffect()
    {
        var camera = Camera.main;
        // マウスのワールド座標までパーティクルを移動し、パーティクルエフェクトを1つ生成する
        var pos = camera.ScreenToWorldPoint(Input.mousePosition + camera.transform.forward * 10);
        GameObject obj = GameObject.Instantiate(_tapEffect.gameObject, pos, Quaternion.identity);
        var particle = obj.GetComponent<ParticleSystem>();
        particle.Emit(1);
        Destroy(obj, particle.main.duration);
    }

    /// <summary>
    /// タッチした箇所のUIとのraycastを取得
    /// </summary>
    /// <returns></returns>
    private List<RaycastResult> GetTouchUIOverRayCasts()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results;
    }

    /// <summary>
    /// タッチした箇所にあるUIがコンポーネントを持っているか?
    /// </summary>
    /// <returns></returns>
    private bool GetTouchUIObjectHaveComponent<T>()
    {
        var rays = GetTouchUIOverRayCasts();

        foreach (var ray in rays)
        {
            var obj = ray.gameObject.GetComponent<T>();
            if(obj!=null)
            {
                return true;
            }
        }

        return false;
    }
}
