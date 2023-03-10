using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionTracker : MonoBehaviour
{
    //Credit to https://www.youtube.com/watch?v=OL1QgwaDsqo&ab_channel=Seabass for providing the framework for this function
    public Dictionary<int, Unit> selectedDict = new Dictionary<int, Unit>();

    public void addSelected(Unit item)
    {
        int id = item.GetInstanceID();
        if(!(selectedDict.ContainsKey(id)))
        {
            selectedDict.Add(id, item);
            selectedDict[id].Select();
        }
    }

    public void deselect(int id)
    {
        selectedDict[id].Deselect();
    }

    public void deselectAll()
    {
        foreach(KeyValuePair<int, Unit> pair in selectedDict)
        {
            if(pair.Value != null)
            {
                deselect(pair.Key);
                //pair.Value.gameObject.Deselect();
            }
        }
        selectedDict.Clear();
    }
}
