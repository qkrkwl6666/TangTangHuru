using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VFXRenderManager : MonoBehaviour
{
    public List<ParticleSystem> particles = new ();

    public List<RawImage> rawImages = new ();

    private void Awake()
    {
        AllSetActiveFalse();
    }

    public void PlayAppraisalParticle(float playTime)
    {
        AllSetActiveFalse();

        rawImages[(int)RenderRawImage.ChargeLightTexture].gameObject.SetActive(true);
        particles[(int)Particle.ChargeLight].gameObject.SetActive(true);

        particles[(int)Particle.ChargeLight].Play();

        StartCoroutine(CoParticleStop(particles[(int)Particle.ChargeLight], playTime));
    }

    public void PlayIconWaitParticle()
    {
        rawImages[(int)RenderRawImage.IconWaitTexture].gameObject.SetActive(true);
        particles[(int)Particle.IconWait].gameObject.SetActive(true);

        particles[(int)Particle.IconWait].Play();
    }

    public void PlayTierParticle(ItemTier itemTier)
    {
        rawImages[(int)RenderRawImage.RatingTexture].gameObject.SetActive(true);

        particles[(int)Particle.RareVFX].gameObject.SetActive(false);
        particles[(int)Particle.EpicVFX].gameObject.SetActive(false);
        particles[(int)Particle.UniqueVFX].gameObject.SetActive(false);
        particles[(int)Particle.LengendaryVFX].gameObject.SetActive(false);

        switch (itemTier)
        {
            case ItemTier.Rare:
                particles[(int)Particle.RareVFX].gameObject.SetActive(true);
                particles[(int)Particle.RareVFX].Play();
                break;
            case ItemTier.Epic:
                particles[(int)Particle.EpicVFX].gameObject.SetActive(true);
                particles[(int)Particle.EpicVFX].Play();
                break;
            case ItemTier.Unique:
                particles[(int)Particle.UniqueVFX].gameObject.SetActive(true);
                particles[(int)Particle.UniqueVFX].Play();
                break;
            case ItemTier.Legendary:
                particles[(int)Particle.LengendaryVFX].gameObject.SetActive(true);
                particles[(int)Particle.LengendaryVFX].Play();
                break;
        }
    }

    public IEnumerator CoParticleStop(ParticleSystem ps, float time)
    {
        yield return new WaitForSeconds(time);

        ps.Stop();
    }

    public void AllSetActiveFalse()
    {
        foreach (var particle in particles)
        {
            particle.gameObject.SetActive(false);
        }

        foreach (var rawImage in rawImages)
        {
            rawImage.gameObject.SetActive(false);
        }
    }
}

public enum Particle
{
    ChargeLight,
    IconWait,
    RareVFX,
    EpicVFX,
    UniqueVFX,
    LengendaryVFX,
}

public enum RenderRawImage
{
    ChargeLightTexture,
    IconWaitTexture, 
    RatingTexture,
}
