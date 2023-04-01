using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] private List<AudioClip> Clips;
    
    [SerializeField] private AudioSource Player;
    public void PlaySound(string name)
    {
        AudioClip clip = Clips.Find(x => x.name == name);
        if (clip != null)
        {
            Player.PlayOneShot(clip);
        }
    }
}
