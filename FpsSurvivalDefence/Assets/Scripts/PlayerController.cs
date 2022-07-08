using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //���ǵ� ���� ����
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float crouchSpeed;

    private float applySpeed;

    [SerializeField]
    private float jumpForce;

    //���º���
    private bool isRun = false;
    private bool isCrouch = false;
    private bool isGround = true;

    //�ɾ��� �� �󸶳� ������ �����ϴ� ����
    [SerializeField]
    private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;

    private CapsuleCollider capsuleCollider;

    //ī�޶� �ΰ���
    [SerializeField]
    private float lookSensitivity;

    //ī�޶� �Ѱ� ����
    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotationX = 0;

    //�ʿ��� ������Ʈ
    [SerializeField]
    private Camera theCamera;

    private Rigidbody myRigid;

    // Start is called before the first frame update
    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        myRigid = GetComponent<Rigidbody>();
        applySpeed = walkSpeed;
        originPosY = theCamera.transform.localPosition.y;
        applyCrouchPosY = originPosY;
    }

    // Update is called once per frame
    void Update()
    {
        IsGround();
        TryJump();
        TryRun();
        TryCrouch();
        Move();
        CameraRotation();
        CharacterRotation();
    }

    //������ ����
    private void Move() 
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        // ������1 ���� -1 �ȴ����� 0
        float _moveDirZ = Input.GetAxisRaw("Vertical");
        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;
        //(1,0,0)(0,0,1)
        //(1,0,1) = 2
        // (0.5,0,0.5) = 1   <- normalized �ϸ� �̷��� �ٲ�
        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);
        
    }

    //���� ī�޶� ȸ��
    private void CameraRotation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    //�¿� ĳ���� ȸ��
    private void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
    }

    //�޸��� �õ�
    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Running();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            RunningCancel();
        }
    }

    //�޸��� ����
    private void Running()
    {
        //���� ���¿��� �޸��� ����� �޸��� ���
        if (isCrouch)
            Crouch();
        
        isRun = true;
        applySpeed = runSpeed;
    }

    //�޸��� ���
    private void RunningCancel()
    {
        isRun = false;
        applySpeed = walkSpeed;
    }

    //�����õ�
    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump();
        }
    }

    //����
    private void Jump()
    {
        //���� ���¿��� ������ �������� ����
        if (isCrouch)
            Crouch();

        myRigid.velocity = transform.up * jumpForce;
    }

    //���� üũ
    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
    }

    //�ɱ� �õ�
    private void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }

    //�ɱ� �Լ�
    private void Crouch()
    {
        isCrouch = !isCrouch;

        if (isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = originPosY;
        }
        StartCoroutine(CrouchCoroutine());
    }

    //�ε巯�� �ɱ� ����
    IEnumerator CrouchCoroutine()
    {
        float _posY = theCamera.transform.localPosition.y;
        int count = 0;


        while(_posY != applyCrouchPosY)
        {
            count++;
            _posY = Mathf.Lerp(_posY, applyCrouchPosY, 0.025f);
            theCamera.transform.localPosition = new Vector3(0, _posY, 0);
            if (count > 70)
                break;
            yield return null;
        }
        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0f);
    }
}
