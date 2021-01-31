using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterInventory : MonoBehaviour
{
    public CharacterInventory_Properties properties;
    public List<Interactable_Source> inventory;
    private InventoryUI ui;
    private Fire selfFire;
    public bool onFire = false;
    float health = 1;

    //navmeshagent stuff
    private NavMeshAgent agent;
    private float maxSpeed;
    private float maxAngularSpeed;
    private float maxAcceleration;
    private SpriteRenderer sprite;

    private void Start()
    {
        inventory = new List<Interactable_Source>();
        ui = transform.GetComponentInChildren<InventoryUI>();

        agent = GetComponent<NavMeshAgent>();
        maxSpeed = agent.speed;
        maxAngularSpeed = agent.angularSpeed;
        maxAcceleration = agent.acceleration;
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    public void AddToInventory(Interactable_Source newObject)
    {
        if (inventory.Count >= properties.size || 
            inventory.Contains(newObject) ||
            !newObject.IsEquippable()) //inventory full or duplicate or not equippable
            return;

        inventory.Add(newObject);
        newObject.owner = this;
        newObject.GetComponent<SpriteRenderer>().enabled = false;
        newObject.GetComponent<Collider>().enabled = false;
        ui.RefreshUI();
    }

    public void AddKindlingToFire(int index, Fire fire)
    {
        if (0 > index || index >= inventory.Count)
        {
            Debug.LogError($"TRIED TO REMOVE ITEM #{index} FROM {gameObject}'S INVENTORY, " +
                $"BUT ONLY {inventory.Count} OBJECTS ARE IN THE INVENTORY");
            return;
        }

        if (!fire.IsRefuelable())
            return;

        if (!(inventory[index] is Kindling)) //make sure that object is kindling
            return;

        Kindling currKindling = inventory[index] as Kindling;
        fire.AddFuel(currKindling.properties.fuelValue);
        RemoveFromInventory(inventory[index]);
    }

    public void RemoveFromInventory(Interactable_Source obj, bool dropped = false)
    {
        inventory.Remove(obj);
        if (dropped) //object discarded, leave it on the ground
        {
            Vector3 spawnLocation = new Vector3(transform.position.x + Random.Range(-1.5f, 1.5f), 
                transform.position.y - 0.5f, transform.position.z + Random.Range(-1.5f, 1.5f));
            obj.transform.position = spawnLocation;
            obj.owner = null;
            obj.GetComponent<SpriteRenderer>().enabled = true;
            obj.GetComponent<Collider>().enabled = true;
        }
        else //object used, destroy it
        {
            Destroy(obj.gameObject);
        }
        ui.RefreshUI();
    }

    public void AddAllKindlingToFire(Fire fire)
    {
        int nonKindlingItems = 0;
        if (!fire.IsRefuelable())
            return;

        while (inventory.Count > nonKindlingItems)
        {
            if (inventory[nonKindlingItems] is Kindling)
                AddKindlingToFire(nonKindlingItems, fire);
            else
                nonKindlingItems++;
        }
    }

    public void BurstIntoFlames()
    {
        if (onFire)
            return;

        Debug.Log("I'm on fire!");
        GetComponentInChildren<AudioSource>().PlayOneShot(properties.wilhelm);
        onFire = true;
        EnableFlames(true);
        if (selfFire == null)
            selfFire = GetComponent<Fire>();
        selfFire.enabled = true;
        StartCoroutine(Burn());
        StartCoroutine(BurnColors());
    }

    private void EnableFlames(bool toggle)
    {
        GetComponentInChildren<InventoryUI>().transform.GetChild(3).gameObject.SetActive(toggle);
    }

    private IEnumerator Burn()
    {
        Debug.Log("I'm burning!");
        yield return null;
        float startFuel = health * selfFire.properties.startingFuel;

        if (selfFire.GetFireLife() == 0)
            selfFire.AddFuel(startFuel, true);

        while (selfFire.GetFireLife() > 0)
        {
            SetHealth(selfFire.GetFireLife() / selfFire.properties.startingFuel);
            yield return null;
        }

        Debug.Log("I'm not burning anymore!");
        SetHealth(0);
        StopBeingOnFire();
        yield return null;
    }

    private IEnumerator BurnColors()
    {
        float timeBetweenColorChanges = 1f / 6f;
        Color red = new Color(.733f, .506f, .525f);
        Color yellow = new Color(.827f, .6745f, .647f);

        while (true)
        {
            sprite.color = red;
            yield return new WaitForSeconds(timeBetweenColorChanges);

            if (!onFire)
                break;
            sprite.color = yellow;
            yield return new WaitForSeconds(timeBetweenColorChanges);

            if (!onFire)
                break;
        }

        if (ui.gameObject.activeSelf)
            sprite.color = Color.green;
        else
            sprite.color = Color.white;

        yield return null;
    }

    private void StopBeingOnFire()
    {
        onFire = false;
        EnableFlames(false);
        selfFire.enabled = false;
        StartCoroutine(Recuperate());
    }

    private IEnumerator Recuperate()
    {
        Debug.Log("I'm getting better!");
        float timeToBurnCompletely = selfFire.properties.startingFuel / selfFire.properties.fuelUsageRate;
        float recuperationTime = timeToBurnCompletely / properties.burnRecuperationSpeed;
        float recuperationPerSecond = 1f / recuperationTime;

        while(health < 1)
        {
            if (onFire)
                yield break;

            float currHealth = health + Time.deltaTime * recuperationPerSecond;
            SetHealth(currHealth);
            yield return null;
        }

        SetHealth(1);
        Debug.Log("I'm healthy!");
        yield return null;
    }

    private void SetHealth(float newHealth)
    {
        health = Mathf.Clamp(newHealth, 0, 1);
        agent.speed = health * maxSpeed;
        agent.angularSpeed = health * maxAngularSpeed;
        agent.acceleration = health * maxAcceleration;
    }
}
