using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{

    [SerializeField]
    private int hp; // ������ ü��.

    [SerializeField]
    private float destroyTime; // ���� ���� �ð�

    [SerializeField]
    private SphereCollider col; // ��ü �ö��̴�.


    // �ʿ��� ���� ������Ʈ.
    [SerializeField]
    private GameObject go_rock; // �Ϲ� ����.
    [SerializeField]
    private GameObject go_debris; // ���� ����.
    [SerializeField]
    private GameObject go_effect_prefabs; // ä�� ����Ʈ.
    [SerializeField]
    private GameObject go_rock_item_prefab; //������ ������

    //������ ������ ���� ����
    [SerializeField]
    private int count;


    // �ʿ��� ���� �̸�.
    [SerializeField]
    private string strike_Sound;
    [SerializeField]
    private string destroy_Sound;


    public void Mining()
    {
        SoundManager.instance.PlaySE(strike_Sound);

        var clone = Instantiate(go_effect_prefabs, col.bounds.center, Quaternion.identity);
        Destroy(clone, destroyTime);

        hp--;

        if (hp <= 0)
            Destruction();
    }

    private void Destruction()
    {
        SoundManager.instance.PlaySE(destroy_Sound);

        col.enabled = false;
        for (int i = 0; i <= count; i++)
        {
            Instantiate(go_rock_item_prefab, go_rock.transform.position, Quaternion.identity);
        }
        Destroy(go_rock);

        go_debris.SetActive(true);
        Destroy(go_debris, destroyTime);
    }

}
