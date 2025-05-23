using System.Diagnostics;
using UnityEngine;

public class ApplicationQuit : MonoBehaviour
{
    public void Quit ()
    {
        Application.Quit();
        Process.GetCurrentProcess().Kill();
    }
}
