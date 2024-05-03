using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEffectsManager : MonoBehaviour
{
	[SerializeField] private AudioClip cardsMatchedSfx;
	[SerializeField] private AudioClip cardsMismatchedSfx;
	[SerializeField] private AudioClip flipCardEffect;

	[SerializeField] private float pitch;
	[SerializeField] private float volume;
	
	private AudioSource audioSource;

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
		AdjustAudio();
	}

	public void CardMatchedSfx()
	{
		audioSource.PlayOneShot(cardsMatchedSfx);
	}

	public void CardMismatchSfx()
	{
		audioSource.PlayOneShot(cardsMismatchedSfx);
	}

	public void CardFlipSfx()
	{
		audioSource.PlayOneShot(flipCardEffect);
	}

	private void AdjustAudio()
	{
		audioSource.pitch  = pitch;
		audioSource.volume = volume;
	}
}
