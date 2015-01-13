using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class SoundManager2D
{ 
    //////////////////////////////////////////////////////////////////////////
    // CONSTANTS
    //////////////////////////////////////////////////////////////////////////
    
    private const int NUM_LOOPING_SOURCES = 32;

    private const int ONE_SHOT_PRIORITY = 0;

    private const int LOOP_BASE_PRIORITY = 1;

    //////////////////////////////////////////////////////////////////////////
    // STATIC DATA
    //////////////////////////////////////////////////////////////////////////


    //This game object is used to hold the AudioSource for one shot sound
    //effects.
    private static GameObject oneShotSoundObject;
    
    //This audio source is used to play one shot sound effects.
    private static AudioSource oneShotSoundSource;
    
    //This game object holds the audio sources used for looping sounds
    private static GameObject loopingSoundObject;
        
    //A collection of AudioSources reserved for looping that are unused.
    private static List<AudioSource> freeLoopingSources;

    //A map of channel identifiers with their audio source
    private static Dictionary<string,AudioSource> usedLoopingSources;

    //////////////////////////////////////////////////////////////////////////
    // CONSTRUCTOR
    //////////////////////////////////////////////////////////////////////////

    static SoundManager2D()
    {     
        //create object to hold one shot source
        oneShotSoundObject = new GameObject();

        oneShotSoundObject.name = "One Shot SFX Object";
        
        //mark object to not be destroyed on scene change
        MonoBehaviour.DontDestroyOnLoad(oneShotSoundObject);
        
        //create the one shot SFX source and store a reference to it
        oneShotSoundSource = 
            addAudioSource (oneShotSoundObject, ONE_SHOT_PRIORITY);

        //create game object to hold looping SFX sources
        loopingSoundObject = new GameObject ();

        loopingSoundObject.name = "Looping Sound Object";

        //mark object to not be destroyed on scene change
        MonoBehaviour.DontDestroyOnLoad (loopingSoundObject);

        //create free looping audio source list
        freeLoopingSources = new List<AudioSource> (NUM_LOOPING_SOURCES);

        //create looping Audio Sources and store then in free sources list
        for(int i = 0; i<NUM_LOOPING_SOURCES; i++)
        {
            AudioSource loopSource = 
                        addAudioSource(loopingSoundObject,LOOP_BASE_PRIORITY);

            freeLoopingSources.Add(loopSource);
        }

        //create the used looping sources dictionary
        usedLoopingSources = new Dictionary<string,AudioSource> ();
    }

    //////////////////////////////////////////////////////////////////////////
    // PUBLIC MEHTODS
    //////////////////////////////////////////////////////////////////////////
    
    /// <summary>
    /// Plays a sound effect that does not loop.  The system should be able
    /// to handle 32 sounds concurrently.
    /// </summary>
    /// <param name="clip">The AudioClip to play.</param>
    /// <param name="volume">The volume scale from 0 to 1.0f.</param>

    public static void playOneShotSound(AudioClip clip, float volume = 1.0F)
    {
        oneShotSoundSource.PlayOneShot(clip,volume);
    }

    /// <summary>
    /// Stops all one shot sounds.
    /// </summary>
    public static void stopAllOneShotSounds()
    {
        oneShotSoundSource.Stop ();
    }


    /// <summary>
    /// Plays a looping sound.  The AudioClip supplied will be assigned
    /// to its own AudioSource, identified by the soundID provided.
    /// 
    /// The priority value should be in the range 1-255.  0 is reserved for
    /// one shot sound effects.  The lower the value the less likely the
    /// sound will be mixed out.  A runtime error will be thrown if the
    /// priority value is outside the accepted range.
    /// 
    /// By default the system supports 32 looping sounds concurrently.  This
    /// value can be edited in SoundManager2D.cs - 
    /// private const int NUM_LOOPING_SOURCES
    /// </summary>
    /// <param name="clip">The AudioClip to play.</param>
    /// <param name="soundID">The ID of the looping audio.</param> 
    /// <param name="priority">The priority of the Audio Source.  Should
    /// be in the range 1-255.  0 is reserved for one shot sounds.</param>
    /// <param name="volume">The volume of the clip 0.0f to 1.0f.</param>
    public static void playLoopingSound(AudioClip clip, string soundID,
                                        int priority, 
                                        float volume = 1.0F)
    {

        //check that the input priority is within accepted range
        if(priority<1 || priority>255)
        {
            throw new System.Exception(
                "Looping Audio Source priority should be in the range 1-255");
        }

        //check if the soundID exists in the used looping sources dictionary
        if(usedLoopingSources.ContainsKey(soundID))
        {
            //Restart the existing source with the new values

            AudioSource existingSource = usedLoopingSources[soundID];

            startLoopingSource(existingSource,clip,priority,volume);
        }

        //Sound ID does not exist already
        else
        {
            //check that there is a free source
            if(freeLoopingSources.Count < 1)
            {
                throw new System.Exception(
                                     "All looping Audio Sources are in use.");
            }

            //take the first available free source
            AudioSource newSource = freeLoopingSources[0];

            //remove it from the free sources list
            freeLoopingSources.Remove(newSource);

            //add it to the used looping sources dictionary
            usedLoopingSources.Add(soundID,newSource);

            //start it playing
            startLoopingSource(newSource,clip,priority,volume);


        }

    }

    /// <summary>
    /// Stops a looping sound.  Throws an exception if the sound is not
    /// playing.
    /// </summary>
    /// <param name="soundID">The ID of the sound to stop.</param>
    public static void stopLoopingSound(string soundID)
    {
        if(!usedLoopingSources.ContainsKey(soundID))
        {
            throw new System.Exception(
                "Tried to stop looping sound \""+soundID+"\" while it was "+
                "not playing.");
        }

        //get the source to stop
        AudioSource sourceToStop = usedLoopingSources[soundID];

        //stop the audio
        sourceToStop.Stop();

        //remove source from used looping sources
        usedLoopingSources.Remove(soundID);

        //put source into free looping sources
        freeLoopingSources.Add(sourceToStop);

    }

    /// <summary>
    /// Stops all looping sounds.
    /// </summary>
    public static void stopAllLoopingSounds()
    {
        List<string> allKeys = new List<string> (usedLoopingSources.Keys);
        foreach (string soundID in allKeys) 
        {
            stopLoopingSound(soundID);
        }
    }

    //////////////////////////////////////////////////////////////////////////
    // PRIVATE METHODS
    //////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Adds an audio source to the game object supplied, sets its priority
    /// and then returns it.
    /// </summary>
    /// <param name="gameObject">The game object to add the audio source to.
    /// </param>
    /// <param name="priority">The priority to set the Audio Source 
    /// to.  Should be an int between 0 and 255.  Lower number means higher
    /// priority.</param>
    private static AudioSource addAudioSource(GameObject gameObject,
                                              int priority)
    {
        //check that the input priority is within accepted range
        if(priority<0 || priority>255)
        {
            throw new System.Exception(
                "Audio Source priority should be in the range 0-255");
        }

        AudioSource audioSource = gameObject.AddComponent<AudioSource> ();

        audioSource.priority = priority;

        return audioSource;
    }

    /// <summary>
    /// Starts the looping source.
    /// </summary>
    /// <param name="source">The AudioSource to use.</param>
    /// <param name="clip">The AudioClip to play.</param>
    /// <param name="priority">The priority of the AudioSource.</param>
    /// <param name="volume">The volume to play the sound at.</param>
    private static void startLoopingSource(AudioSource source,
                                           AudioClip clip, 
                                           int priority,
                                           float volume)
    {
        source.loop = true;
        source.clip = clip;
        source.priority = priority;
        source.volume = volume;
        
        source.Play();
    }
    
}
