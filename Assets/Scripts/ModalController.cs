using System.Collections;
using System.Collections.Generic;
using Contexts;
using UnityEngine;
using UnityEngine.UI;

public class ModalController : ContextProvider<ModalController>
{
    [SerializeField]
    private ModalBase[] _modals;
    [SerializeField]
    private Image _backgroundImage;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private InputController _inputController;

    public void Open(string modalId)
    {
        ForceClose();
        _backgroundImage.gameObject.SetActive(true);
        _animator.SetBool("open", true);
        _inputController.Interactable = false;
        _activeModal = FindModalById(modalId);
        if (_activeModal)
        {
            _activeModal.gameObject.SetActive(true);
            _activeModal.Open(Close);
            UISoundController.Instance.PlayOpenMenu();
        }
        else
        {
            Debug.LogError("No modal found for ModalId: " + modalId);
        }
    }

    public void ForceClose()
    {
        if (_activeModal)
        {
            _activeModal.Close();
        }
    }

    private void Close(string modalId)
    {
        _animator.SetBool("open", false);
        _inputController.Interactable = true;
        UISoundController.Instance.PlayCloseMenu();
    }

    private ModalBase FindModalById(string modalId)
    {
        foreach (ModalBase modal in _modals)
        {
            if (modal.ModalId.Equals(modalId))
            {
                return modal;
            }
        }

        return null;
    }

    private void Disable_Background()
    {
        _backgroundImage.gameObject.SetActive(false);
    }

    private ModalBase _activeModal;
}
