using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class Bunny : MonoBehaviour
{
    [SerializeField] private GameObject _egg;
    [SerializeField] private float _jumpForce = 10f, _jumpDelay = 2f;
    [SerializeField] private Color[] _colors;
    [SerializeField] private Transform _eggTransform;

    private Animator _anim;
    private bool _isGrounded = true;
    private int _eggsLaid = 0;
    private int _hopHash = Animator.StringToHash("HopStart");
    private int _landHash = Animator.StringToHash("HopLand");
    private Rigidbody _rigidbody;
    private WaitForSeconds _jumpWait;
    
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _jumpWait = new WaitForSeconds(_jumpDelay);

        StartCoroutine(JumpRoutine());
    }

    private void JumpAnim()
    {
        _anim.SetTrigger(_hopHash);
    }

    private IEnumerator JumpRoutine()
    {
        while (_eggsLaid < _colors.Length - 1)
        {
            yield return _jumpWait;
            JumpAnim();
        }
    }

    public void Jump()
    {
        _rigidbody.AddForce(0,_jumpForce,-_jumpForce/2, ForceMode.Impulse);
        _isGrounded = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Ground"))
        {
            if (!_isGrounded)
            {
                _anim.SetTrigger(_landHash);
                _isGrounded = true;
            }
        }
    }

    public void LayEgg()
    {
        // instantiate egg
        GameObject egg = Instantiate(_egg, _eggTransform.position, Quaternion.identity);
        // material.color = _colors[_eggsLaid];
        egg.GetComponent<MeshRenderer>().material.color = _colors[_eggsLaid];
        _eggsLaid++;
    }
}
