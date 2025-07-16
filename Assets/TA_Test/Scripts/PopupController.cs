using UnityEngine;

public class PopupController : MonoBehaviour
{
    public GameObject popupRoot;
    public Animator animator;

    public void OpenPopup()
    {
        popupRoot.SetActive(true);
        animator.SetTrigger("ShowPopup");
    }

    public void ClosePopup()
    {
        animator.SetTrigger("HidePopup");
    }

    public void DeactivateAfterHide()
    {
        popupRoot.SetActive(false);
    }
}
