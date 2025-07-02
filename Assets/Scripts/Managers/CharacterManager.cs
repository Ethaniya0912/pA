using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    protected CharacterController characterController;

    protected virtual void Awake()
    {
        // 씬이 로드될때 오브젝트가 사라지지않게.
        DontDestroyOnLoad(this);

        // 컨트롤러 컴포넌트 가져오기.
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
}
