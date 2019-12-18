using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator ArmAnimController;

    public void Shoot()
    {
        ArmAnimController.SetTrigger("shoot");
    }

    public void Reload()
    {
        ArmAnimController.SetTrigger("reload");
    }
}
