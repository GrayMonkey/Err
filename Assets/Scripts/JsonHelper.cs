using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class JsonHelper
{
	public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.dataset;
    }

    public static List<T> FromJsonList<T>(string json)
    {
    	WrapperList<T> wrapperList = JsonUtility.FromJson<WrapperList<T>>(json);
    	return wrapperList.dataset;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] dataset;
    }

    [System.Serializable]
    private class WrapperList<T>
    {
    	public List<T> dataset;
    }
}