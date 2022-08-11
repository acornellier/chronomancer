using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] AudioSource jumpSource;
    [SerializeField] AudioClip jumpClip;

    [SerializeField] AudioSource landSource;
    [SerializeField] AudioClip landClip;

    [SerializeField] AudioSource footstepSource;
    [SerializeField] AudioClip[] footstepClips;

    public void Jump()
    {
        jumpSource.PlayOneShot(jumpClip);
    }

    public void Land()
    {
        landSource.PlayOneShot(landClip);
    }

    public void Footstep()
    {
        footstepSource.PlayOneShot(footstepClips[Random.Range(0, footstepClips.Length)]);
    }
}