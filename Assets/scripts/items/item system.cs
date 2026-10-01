using UnityEngine;

public class itemsystem : MonoBehaviour
{
    public int typeitem = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (typeitem == 1)
            {
                movement.healthmedkit += 1;
            }
            if (typeitem == 2)
            {
                movement.powerItem += 1;
            }
            if (typeitem == 3)
            {
                movement.NewDefenseItem += 1;
            }
            if (typeitem == 4)
            {
                movement.NewWeapon += 1;
            }
            if (typeitem == 5)
            {
                itemmenusystem.potionC += 1;
            }
            if (typeitem == 6)
            {
                itemmenusystem.reviveC += 1;
            }
            if (typeitem == 7)
            {
                itemmenusystem.ManapotC += 1;
            }
            if (typeitem == 8)
            {
                itemmenusystem.BjinC += 1;
            }
            if (typeitem == 9)
            {
                itemmenusystem.keyC += 1;
            }
            if (typeitem == 10)
            {
                itemmenusystem.OrbC += 1;
            }
            if (typeitem == 11)
            {
                itemmenusystem.BookC += 1;
            }
            if (typeitem == 12)
            {
                itemmenusystem.InkC += 1;
            }
            gameObject.SetActive(false);
        }
    }
}
