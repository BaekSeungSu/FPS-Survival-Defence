using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CloseWeaponController : MonoBehaviour
{
    // �̿ϼ� Ŭ���� = �߻� Ŭ����.


    // ���� ������ Hand�� Ÿ�� ����.
    [SerializeField]
    protected CloseWeapon currentCloseWeapon;

    // ������??
    protected bool isAttack = false;
    protected bool isSwing = false;

    protected RaycastHit hitInfo;


    protected void TryAttack()
    {
        if (!Inventory.inventoryActivated) 
        {
            if (Input.GetButton("Fire1"))
            {
                if (!isAttack)
                {
                    StartCoroutine(AttackCoroutine());
                }
            }
        }
    }

    protected IEnumerator AttackCoroutine()
    {
        isAttack = true;
        currentCloseWeapon.anim.SetTrigger("Attack");

        yield return new WaitForSeconds(currentCloseWeapon.attackDelayA);
        isSwing = true;

        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(currentCloseWeapon.attackDelayB);
        isSwing = false;


        yield return new WaitForSeconds(currentCloseWeapon.attackDelay - currentCloseWeapon.attackDelayA - currentCloseWeapon.attackDelayB);
        isAttack = false;
    }


    // �̿ϼ� = �߻� �ڷ�ƾ.
    protected abstract IEnumerator HitCoroutine();


    protected bool CheckObject()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentCloseWeapon.range))
        {
            return true;
        }
        return false;
    }

    // �ϼ� �Լ�������, �߰� ������ �Լ�.
    public virtual void CloseWeaponChange(CloseWeapon _closeWeapon)
    {
        if (WeaponManager.currentWeapon != null)
            WeaponManager.currentWeapon.gameObject.SetActive(false);

        currentCloseWeapon = _closeWeapon;
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;

        currentCloseWeapon.transform.localPosition = Vector3.zero;
        currentCloseWeapon.gameObject.SetActive(true);
    }
}
