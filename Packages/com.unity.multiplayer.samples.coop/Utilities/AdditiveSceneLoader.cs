using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdditiveSceneLoader : NetworkBehaviour
{
    const float k_CooldownDuration = 5.0f;

    [SerializeField]
    string sceneName;

    int m_PlayersInTrigger;
    bool m_IsLoaded;
    bool m_IsCooldown;

    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            enabled = false;
        }

    }

    void Update()
    {
        if (!m_IsCooldown)
        {
            if (m_IsLoaded && m_PlayersInTrigger == 0)
            {
                NetworkManager.SceneManager.UnloadScene(SceneManager.GetSceneByName(sceneName));
                m_IsLoaded = false;
            }
            else if (!m_IsLoaded && m_PlayersInTrigger > 0)
            {
                NetworkManager.SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
                m_IsLoaded = true;
                // Add this delay to prevent players entering and leaving the collider repeatedly from continually load/unloading the scene
                StartCoroutine(Cooldown());
            }
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_PlayersInTrigger++;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_PlayersInTrigger--;
        }
    }

    IEnumerator Cooldown()
    {
        m_IsCooldown = true;
        yield return new WaitForSeconds(k_CooldownDuration);
        m_IsCooldown = false;
    }
}