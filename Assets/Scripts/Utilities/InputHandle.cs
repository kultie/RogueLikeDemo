
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputHandleUtilities
{

    static List<KeyCode> inputQueue = new List<KeyCode>();

    public static void UpdateInput()
    {
        HandleKeyDown();
        HandleKeyUp();
    }

    static void HandleKeyDown()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            inputQueue.Add(KeyCode.UpArrow);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            inputQueue.Add(KeyCode.DownArrow);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            inputQueue.Add(KeyCode.LeftArrow);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            inputQueue.Add(KeyCode.RightArrow);
        }
    }

    static void HandleKeyUp()
    {
        if (inputQueue.Count != 0)
        {
            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                ReleaseKey(KeyCode.UpArrow);
            }
            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                ReleaseKey(KeyCode.DownArrow);
            }
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                ReleaseKey(KeyCode.LeftArrow);
            }
            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                ReleaseKey(KeyCode.RightArrow);
            }
        }
    }

    static void ReleaseKey(KeyCode keyCode)
    {
        inputQueue.RemoveAll(c => c.Equals(keyCode));
    }

    public static KeyCode GetLastInput()
    {
        if (inputQueue.Count != 0)
        {
            return inputQueue[inputQueue.Count - 1];
        }
        return KeyCode.None;
    }

    public static void ClearInputQueue()
    {
        inputQueue.Clear();
    }
}