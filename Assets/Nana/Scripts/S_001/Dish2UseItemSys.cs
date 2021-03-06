using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;

public class Dish2UseItemSys : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool _isPushed = false; // マウスが押されているか押されていないか
    private Vector3 _nowMousePosi; // 現在のマウスのワールド座標

    private Vector2 prevPosition;

    //タイムライン
    [SerializeField] PlayableDirector director;

    //ターゲット
    [SerializeField] GameObject targetObj;

    void Update()
    {
        Vector3 nowmouseposi;
        Vector3 diffposi;

        // マウスが押下されている時、オブジェクトを動かす
        if (_isPushed) {
            // 現在のマウスのワールド座標を取得
            nowmouseposi = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // 一つ前のマウス座標との差分を計算して変化量を取得
            diffposi = nowmouseposi - _nowMousePosi;

            //　Y成分のみ変化させる
            //diffposi.x = 0;
            diffposi.z = 0;

            // 開始時のオブジェクトの座標にマウスの変化量を足して新しい座標を設定
            GetComponent<Transform>().position += diffposi;

            // 現在のマウスのワールド座標を更新
            _nowMousePosi = nowmouseposi;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isPushed = true;

        // マウスのワールド座標を保存
        _nowMousePosi = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //追加
        prevPosition = transform.position;

        //Debug.Log("down");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isPushed = false;
        _nowMousePosi = Vector3.zero;

        //Debug.Log("UP");

        bool flg = true;
        var raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);

        foreach (var hit in raycastResults)
        {
            if (hit.gameObject == targetObj.gameObject)
            {
                transform.position = hit.gameObject.transform.position;
                flg = false;

                PlayTimeline(hit.gameObject.name);

                gameObject.SetActive(false);
            }
        }
        if (flg)
        {
            transform.position = prevPosition;
        }
    }

    private void PlayTimeline(string itemName)
    {
        //director.enabled = true;
        director.Play();
    }
}
