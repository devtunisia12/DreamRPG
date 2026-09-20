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
            gameObject.SetActive(false);
        }
    }
}
