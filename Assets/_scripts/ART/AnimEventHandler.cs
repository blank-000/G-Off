using UnityEngine;

public class AnimEventHandler : MonoBehaviour
{

    public void OnFire()
    {
        if (PlayerAnimationRelay.I._fire != null)
        {
            PlayerAnimationRelay.I._fire.OnFire();
        }
    }
}
