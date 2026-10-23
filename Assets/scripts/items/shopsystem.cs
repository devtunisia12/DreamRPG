using UnityEngine;

public class shopsystem : MonoBehaviour
{
    public int Typeitem;

    public void BuyItem(int Typeitem)
    {
        if (Typeitem == 0)
        {
            movement.healthmedkit += 1;
        }

        if (Typeitem == 1)
        {
            movement.powerItem += 1;
        }

        if (Typeitem == 2)
        {
            itemmenusystem.potionC += 1;
        }

        if (Typeitem == 3)
        {
            itemmenusystem.keyC += 1;
        }
    }
}
