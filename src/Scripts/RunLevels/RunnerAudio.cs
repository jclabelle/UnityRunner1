using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RunnerAudio : MonoBehaviour, IRunnerAudio
{
    [field: SerializeField] private AudioClip Damage { get; set; }
    [field: SerializeField] private AudioClip Idle { get; set; }
    [field: SerializeField] private AudioSource AudioSource { get; set; }
    
    
    // Start is called before the first frame update
    void Start()
    {
        AudioSource ??= GetComponent<AudioSource>();
        PlayIdle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public AudioClip Play(IRunnerAudio.EClip clip)
    {
        switch (clip)
        {
            case IRunnerAudio.EClip.Walk :
                return PlayWalk();
                break;
            
            case IRunnerAudio.EClip.Damage :
                return PlayDamage();
                break;
            
            case IRunnerAudio.EClip.Idle :
                return PlayIdle();
                break;
                
            case IRunnerAudio.EClip.Win : 
                return PlayWin();
                break;
            
            case IRunnerAudio.EClip.Lose : 
                return PlayLose();
                break;
            default:
                return Idle;
        }
    }

    private AudioClip PlayLose()
    {
        throw new System.NotImplementedException();
    }

    private AudioClip PlayWin()
    {
        throw new System.NotImplementedException();
    }

    private AudioClip PlayIdle()
    {
        if(Idle)
            AudioSource.PlayOneShot(Damage);

        return Idle;
    }

    private AudioClip PlayDamage()
    {
        AudioSource.PlayOneShot(Damage);
        return AudioSource.clip;
    }

    private AudioClip PlayWalk()
    {
        throw new System.NotImplementedException();
    }
}

public interface IRunnerAudio
{
    public enum EClip
    {
        Walk,
        Damage,
        Idle,
        Win,
        Lose
    }

    public AudioClip Play(IRunnerAudio.EClip clip);
}
