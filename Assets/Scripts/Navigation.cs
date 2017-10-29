using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigation : MonoBehaviour
{
    private GameObject initialActiveGameObject;
    private AudioClip resetAudio;

    #region - "Singleton" Instance -
    private static Navigation instance;

    public static Navigation Instance
    {
        get
        {
            if (instance == null)
            {
                throw new UnityException("Someone is calling Navigation.Instance before it is set! Change script execution order.");
            }

            return instance;
        }
    }

    private void InitializeSingleton()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    public bool CanGoBackToBedroom
    {
        get
        {
            return initialActiveGameObject.activeSelf;
        }
    }
    public AudioClip ResetAudio
    {
        get
        {
            return resetAudio;
        }
    }

    private void Awake()
    {
        InitializeSingleton();
    }
}
