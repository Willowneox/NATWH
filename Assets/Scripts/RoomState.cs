using System;
using System.Collections.Generic;

[Serializable]
public class RoomState
{
    public int trashDay = -1;
    public List<int> cleanedTrashIDs = new List<int>();

    public void ValidateForDay(int currentDay)
    {
        if (trashDay != currentDay)
        {
            cleanedTrashIDs.Clear();
            trashDay = currentDay;
        }
    }

    public void CleanTrash(int id)
    {
        if (!cleanedTrashIDs.Contains(id))
        {
            cleanedTrashIDs.Add(id);
        }
    }

    public bool IsTrashCleaned(int id) => cleanedTrashIDs.Contains(id);

    public float GetCleanPercent(int totalSpawnedToday)
    {
        if (totalSpawnedToday == 0) return 1f;
        return (float)cleanedTrashIDs.Count/totalSpawnedToday;
    }
}
