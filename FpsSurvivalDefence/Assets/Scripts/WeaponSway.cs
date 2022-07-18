using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{

    // ���� ��ġ.
    private Vector3 originPos;

    // ���� ��ġ.
    private Vector3 currentPos;

    // sway �Ѱ�.
    [SerializeField]
    private Vector3 limitPos;

    // ������ sway �Ѱ�.
    [SerializeField]
    private Vector3 fineSightLimitPos;


    // �ε巯�� ������ ����.
    [SerializeField]
    private Vector3 smoothSway;


    // �ʿ��� ������Ʈ/
    [SerializeField]
    private GunController theGunController;


    // Use this for initialization
    void Start()
    {
        originPos = this.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        TrySway();
    }


    private void TrySway()
    {
        if (Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0)
            Swaying();
        else
            BackToOriginPos();
    }

    private void Swaying()
    {
        float _moveX = Input.GetAxisRaw("Mouse X");
        float _moveY = Input.GetAxisRaw("Mouse Y");

        if (!theGunController.isFineSightMode)
        {
            currentPos.Set(Mathf.Clamp(Mathf.Lerp(currentPos.x, -_moveX, smoothSway.x), -limitPos.x, limitPos.x),
               Mathf.Clamp(Mathf.Lerp(currentPos.y, -_moveY, smoothSway.x), -limitPos.y, limitPos.y),
               originPos.z);
        }
        else
        {
            currentPos.Set(Mathf.Clamp(Mathf.Lerp(currentPos.x, -_moveX, smoothSway.y), -fineSightLimitPos.x, fineSightLimitPos.x),
               Mathf.Clamp(Mathf.Lerp(currentPos.y, -_moveY, smoothSway.y), -fineSightLimitPos.y, fineSightLimitPos.y),
               originPos.z);
        }
        transform.localPosition = currentPos;
    }

    private void BackToOriginPos()
    {
        currentPos = Vector3.Lerp(currentPos, originPos, smoothSway.x);
        transform.localPosition = currentPos;
    }
}
