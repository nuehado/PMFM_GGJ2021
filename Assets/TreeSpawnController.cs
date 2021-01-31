using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawnController : MonoBehaviour
{
    [SerializeField] private List<TreeLocation> allTrees = new List<TreeLocation>();
    [SerializeField] private List<HarvestableTree> disabledTrees = new List<HarvestableTree>();
    [SerializeField] private int maxTrees;
    [SerializeField] private int currentActiveHarvestables;

    Random rnd = new Random();

    private void Start()
    {
        foreach(TreeLocation location in GetComponentsInChildren<TreeLocation>())
        {
            allTrees.Add(location);
        }
        maxTrees = allTrees.Count;
        currentActiveHarvestables = maxTrees;
    }

    void OnDrawGizmos()
    {
        foreach(TreeLocation treeLocation in allTrees)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(treeLocation.transform.position, 1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentActiveHarvestables = 0;
        for (int i = 0; i < maxTrees; i++)
        {
            if (allTrees[i].GetComponentInChildren<HarvestableTree>(true).isActiveAndEnabled)
            {
                currentActiveHarvestables++;
            }
            else if (allTrees[i].GetComponentInChildren<HarvestableStump>(true).isActiveAndEnabled)
            {
                currentActiveHarvestables++;
            }
            else
            {
                HarvestableTree treeSource = null;
                foreach (KindlingSource source in allTrees[i].GetComponentsInChildren<KindlingSource>(true))
                {
                    if (source is HarvestableTree tree && disabledTrees.Contains(tree) == false)
                    {
                        treeSource = tree;
                    }
                    else if (source is HarvestableStump stump && stump.isActiveAndEnabled)
                    {
                        return;
                    }
                }
                if (treeSource != null)
                {
                    disabledTrees.Add(treeSource);
                }
                
            }
        }

        if (disabledTrees.Count >= maxTrees / 2)
        {
            RespawnRandomTree();
        }
    }

    private void RespawnRandomTree()
    {
        int respawnedTreeIndex = Random.Range(0, disabledTrees.Count - 1);
        disabledTrees[respawnedTreeIndex].gameObject.SetActive(true);
        disabledTrees.RemoveAt(respawnedTreeIndex);
    }
}
