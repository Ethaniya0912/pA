using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    protected CharacterController characterController;

    protected virtual void Awake()
    {
        // ���� �ε�ɶ� ������Ʈ�� ��������ʰ�.
        DontDestroyOnLoad(this);

        // ��Ʈ�ѷ� ������Ʈ ��������.
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
}
