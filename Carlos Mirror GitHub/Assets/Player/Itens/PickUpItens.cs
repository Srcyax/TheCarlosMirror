using Mirror;
using System.Collections;
using UnityEngine;

public class PickUpItens : NetworkBehaviour
{
    [SerializeField] public GameObject itemPicktureInventory;
    bool canGet = false;

    Outline outline => GetComponent<Outline>();

    private void Start()
    {
        StartCoroutine(canGetItem());
    }

    private void Update()
    {
        if (transform.position.y < -4)
            transform.position = new Vector3(transform.position.x, 3, transform.position.z);

        outline.enabled = Vector3.Distance(transform.position, Camera.main.transform.position) < 8;
    }

    IEnumerator canGetItem()
    {
        yield return new WaitForSeconds(3f);
        canGet = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && canGet)
        {
            Transform t = other.gameObject.transform.GetChild(3).GetChild(0).GetChild(0).GetChild(1);

            for (int i = 0; i < t.childCount; i++)
            {
                if (!t.GetChild(i).gameObject.CompareTag("WeightInventory"))
                    continue;

                if (t.GetChild(i).gameObject.GetComponent<WeightInventory>().weightInventory >= t.GetChild(i).gameObject.GetComponent<WeightInventory>().weightSlider.maxValue)
                    continue;

                t.GetChild(i).gameObject.GetComponent<WeightInventory>().weightInventory += itemPicktureInventory.GetComponent<ItemInfo>().weight;

                for (int j = 0; j < t.childCount; j++)
                {
                    if (!t.GetChild(j).gameObject.CompareTag("SlotInventory"))
                        continue;

                    if (t.GetChild(j).transform.GetChild(0).childCount > 0 && !t.GetChild(j).transform.GetChild(0).transform.GetChild(0).gameObject.CompareTag(itemPicktureInventory.tag))
                        continue;

                    if (t.GetChild(j).transform.GetChild(0).childCount > 0 && !(t.GetChild(j).transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<ItemInfo>().itemType == ItemType.Consumable))
                        continue;

                    if (Instantiate(itemPicktureInventory, t.GetChild(j).transform.GetChild(0)))
                    {
                        NetworkServer.Destroy(gameObject);
                        break;
                    }
                }
            }         
        }
    }
}