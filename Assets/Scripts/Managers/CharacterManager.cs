using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [HideInInspector] public CharacterController characterController;
    [HideInInspector] public Animator animator;
    [HideInInspector] public StatsManager statsManager;

    protected virtual void Awake()
    {
        // ���� �ε�ɶ� ������Ʈ�� ��������ʰ�.
        DontDestroyOnLoad(this);

        // ��Ʈ�ѷ� ������Ʈ ��������.
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        statsManager = GetComponent<StatsManager>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
}
