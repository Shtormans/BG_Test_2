using MainGame;
using UnityEngine;

public class MultisceneItemsTransfer : MonoBehaviour
{
    [SerializeField] private AnimatedSkin _defaultSkin;

    private MultisceneItems _roomItems;

    private void Awake()
    {
        _roomItems = new MultisceneItems()
        {
            Skin = _defaultSkin
        };
    }

    public MultisceneItems GetMultisceneItems()
    {
        return _roomItems;
    }

    public void ChangeMultisceneItems(MultisceneItems roomItems)
    {
        _roomItems = roomItems;
    }
}
