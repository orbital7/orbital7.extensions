using System.Collections;

namespace System;

public static class ListExtensions
{
    public static DateTime Average(
        this IList<DateTime> dates)
    {
        int count = dates.Count;
        double temp = 0;

        for (int i = 0; i < count; i++)
            temp += dates[i].Ticks / (double)count;

        return new DateTime((long)temp);
    }

    public static string ToCommaAndString(
        this IList list, 
        string nullValue = "")
    {
        if (list != null || list.Count == 0)
        {
            if (list.Count == 1)
            {
                return GetItemValue(list[0], nullValue);
            }
            else if (list.Count == 2)
            {
                return string.Format("{0} and {1}",
                    GetItemValue(list[0], nullValue),
                    GetItemValue(list[1], nullValue));
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                int index = 0;

                foreach (object item in list)
                {
                    if (index == list.Count - 2)
                        sb.Append(", and ");
                    else if (index > 0)
                        sb.Append(", ");

                    sb.Append(GetItemValue(item, nullValue));
                    index++;
                }

                return sb.ToString();
            }
        }
        else
        {
            return null;
        }
    }

    public static string ToString(
        this IList list, 
        string delim, 
        bool encloseInQuotes = false, 
        string nullValue = "", 
        bool addDelimToEnd = false)
    {
        StringBuilder sb = new StringBuilder();
        int index = 0;

        if (list != null)
        {
            foreach (object item in list)
            {
                // Append the delimiter if not first.
                if (index > 0)
                    sb.Append(delim);

                // Get the item text.
                string value = GetItemValue(item, nullValue);
                if (encloseInQuotes)
                    value = value.EncloseInQuotes();

                // Append the item, and not matter what we're no longer first.
                sb.Append(value);
                index++;
            }

            if (addDelimToEnd)
                sb.Append(delim);
        }

        return sb.ToString();
    }

    private static string GetItemValue(
        object listItem, 
        string nullValue)
    {
        string value = nullValue;
        if (listItem != null)
            value = listItem.ToString();

        return value;
    }

    public static List<T>[] SplitEvenly<T>(
        this IList<T> list, 
        int segments)
    {
        // Initialize.
        List<T>[] lists = new List<T>[segments];
        for (int i = 0; i < segments; i++)
            lists[i] = new List<T>();
        
        // Distribute.
        int index = 0;
        int maxIndex = segments - 1;
        for (int i = 0; i < list.Count; i++)
        {
            lists[index].Add(list[i]);
            index++;
            if (index > maxIndex) index = 0;
        }

        return lists;
    }

    public static List<List<T>> SplitBySize<T>(
        this IList<T> list, int maxSize)
    {
        List<List<T>> lists = new List<List<T>>();

        // Loop.
        List<T> currentList = null;
        for (int i = 0; i < list.Count; i++)
        {
            if ((currentList == null) || (currentList.Count == maxSize))
            {
                currentList = new List<T>();
                lists.Add(currentList);
            }
            currentList.Add(list[i]);
        }

        return lists;
    }

    public static List<T> Randomize<T>(
        this IList<T> inputList)
    {
        List<T> randomList = new List<T>();

        Random r = new Random();

        int randomIndex = 0;
        while (inputList.Count > 0)
        {
            randomIndex = r.Next(0, inputList.Count); // Choose a random object in the list
            randomList.Add(inputList[randomIndex]);   // add it to the new, random list
            inputList.RemoveAt(randomIndex);          // remove to avoid duplicates
        }

        return randomList;
    }

    public static int? GetItemIndex(
        this IList list, 
        object listItem)
    {
        int? index = null;

        if ((list != null) && (listItem != null))
        {
            int i = 0;
            foreach (object item in list)
            {
                if ((listItem == null && item == null) || (listItem != null && listItem.Equals(item)))
                {
                    index = i;
                    break;
                }
                i++;
            }
        }

        return index;
    }
    public static bool CanMoveItemUp(
        this IList list, 
        object listItem)
    {
        return CanMoveItemUp(list, listItem, list.GetItemIndex(listItem));
    }

    public static bool CanMoveItemUp(
        this IList list, 
        object listItem, 
        int? listItemIndex)
    {
        return (listItem != null) && (listItemIndex != null) && (listItemIndex > 0);
    }

    public static bool CanMoveItemDown(
        this IList list, 
        object listItem)
    {
        return CanMoveItemDown(list, listItem, list.GetItemIndex(listItem));
    }

    public static bool CanMoveItemDown(
        this IList list, 
        object listItem, 
        int? listItemIndex)
    {
        return (listItem != null) && (listItemIndex != null) && (listItemIndex < list.Count - 1);
    }

    public static bool MoveItemUp(
        this IList list, 
        object listItem)
    {
        return MoveItemUp(list, listItem, list.GetItemIndex(listItem));
    }

    public static bool MoveItemUp(
        this IList list, 
        object listItem, 
        int? listItemIndex)
    {
        bool success = false;

        if (CanMoveItemUp(list, listItem, listItemIndex))
        {
            int insertIndex = (int)listItemIndex - 1;
            MoveItem(list, listItem, insertIndex);
            success = true;
        }

        return success;
    }

    public static bool MoveItemDown(
        this IList list, 
        object listItem)
    {
        return MoveItemDown(list, listItem, list.GetItemIndex(listItem));
    }

    public static bool MoveItemDown(
        this IList list, 
        object listItem, 
        int? listItemIndex)
    {
        bool success = false;

        if (CanMoveItemDown(list, listItem, listItemIndex))
        {
            int insertIndex = (int)listItemIndex + 1;
            MoveItem(list, listItem, insertIndex);
            success = true;
        }

        return success;
    }

    private static void MoveItem(
        this IList list,
        object listItem, 
        int insertIndex)
    {
        list.Remove(listItem);
        list.Insert(insertIndex, listItem);
    }
}
