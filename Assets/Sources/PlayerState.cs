using UnityEngine;

public abstract class PlayerState
{
    protected PlayerController _playerController;



    public virtual void Start(PlayerController playerController)
    {
        if (_playerController == null)
            _playerController = playerController;
    }


    public virtual void End()
    {

    }
}
