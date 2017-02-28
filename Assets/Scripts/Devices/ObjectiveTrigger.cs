using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ObjectiveTrigger : MonoBehaviour
{
    private AudioSource _audioSource;

    void Start()
    {
        _audioSource = this.GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        PlaySound();
        Managers.Mission.ReachObjective();
    }

    private void PlaySound()
    {
        _audioSource.Play();
    }
}
