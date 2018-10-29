using Orbital7.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public static class ListExtensions
    {
        public static DateTime Average(this List<DateTime> dates)
        {
            int count = dates.Count;
            double temp = 0;

            for (int i = 0; i < count; i++)
                temp += dates[i].Ticks / (double)count;

            return new DateTime((long)temp);
        }

        public static string FormatAsCommaAndString(
            this IList list, 
            string nullValue = "")
        {
            if (list != null || list.Count == 0)
            {
                if (list.Count == 1)
                {
                    return GetListItemValue(list[0], nullValue);
                }
                else if (list.Count == 2)
                {
                    return string.Format("{0} and {1}",
                        GetListItemValue(list[0], nullValue),
                        GetListItemValue(list[1], nullValue));
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

                        sb.Append(GetListItemValue(item, nullValue));
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
                    string value = GetListItemValue(item, nullValue);
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

        private static string GetListItemValue(
            object listItem, 
            string nullValue)
        {
            string value = nullValue;
            if (listItem != null)
                value = listItem.ToString();

            return value;
        }

        public static List<T>[] SplitEvenly<T>(this IList<T> list, int segments)
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

        public static List<List<T>> SplitBySize<T>(this IList<T> list, int maxSize)
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

        public static List<T> Randomize<T>(this IList<T> inputList)
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

        public static int? GetListItemIndex(this IList list, object listItem)
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
        public static bool CanMoveListItemUp(this IList list, object listItem)
        {
            return CanMoveListItemUp(list, listItem, list.GetListItemIndex(listItem));
        }

        public static bool CanMoveListItemUp(this IList list, object listItem, int? listItemIndex)
        {
            return (listItem != null) && (listItemIndex != null) && (listItemIndex > 0);
        }

        public static bool CanMoveListItemDown(this IList list, object listItem)
        {
            return CanMoveListItemDown(list, listItem, list.GetListItemIndex(listItem));
        }

        public static bool CanMoveListItemDown(this IList list, object listItem, int? listItemIndex)
        {
            return (listItem != null) && (listItemIndex != null) && (listItemIndex < list.Count - 1);
        }

        public static bool MoveListItemUp(this IList list, object listItem)
        {
            return MoveListItemUp(list, listItem, list.GetListItemIndex(listItem));
        }

        public static bool MoveListItemUp(this IList list, object listItem, int? listItemIndex)
        {
            bool success = false;

            if (CanMoveListItemUp(list, listItem, listItemIndex))
            {
                int insertIndex = (int)listItemIndex - 1;
                MoveListItem(list, listItem, insertIndex);
                success = true;
            }

            return success;
        }

        public static bool MoveListItemDown(this IList list, object listItem)
        {
            return MoveListItemDown(list, listItem, list.GetListItemIndex(listItem));
        }

        public static bool MoveListItemDown(this IList list, object listItem, int? listItemIndex)
        {
            bool success = false;

            if (CanMoveListItemDown(list, listItem, listItemIndex))
            {
                int insertIndex = (int)listItemIndex + 1;
                MoveListItem(list, listItem, insertIndex);
                success = true;
            }

            return success;
        }

        private static void MoveListItem(this IList list, object listItem, int insertIndex)
        {
            list.Remove(listItem);
            list.Insert(insertIndex, listItem);
        }
    }
}
