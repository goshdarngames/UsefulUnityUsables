using UnityEngine;
using System.Collections;

public class DemoGUI : MonoBehaviour {


    /*************************************************************************
     * ONE SHOT SOUND EFFECTS
     ************************************************************************/

    public AudioClip whizzClip;

    public AudioClip carHornClip;

    public AudioClip codeRed;

    /*************************************************************************
     * LOOPING SOUNDS
     ************************************************************************/

    public AudioClip squeakyBedLoop;

    public AudioClip bellLoop;

    public AudioClip funkLoop;

    void OnGUI()
    {

        //Title
        GUI.Box (new Rect (10, 10, 780, 25), "Sound Manager 2D Demo");

        //SFX BOX
        GUI.Box (new Rect (10, 40, 780, 200), "One Shot Sound Effects");

        //Loop Box
        GUI.Box (new Rect (10, 245, 780, 200), "Looping Sounds");

        //One shot sound effects

        if(GUI.Button(new Rect(30,60,230,170),"Whizz"))
        {
            SoundManager2D.playOneShotSound(whizzClip);
        }

        if(GUI.Button (new Rect(280,60,230,170),"Car Horn"))
        {
            SoundManager2D.playOneShotSound(carHornClip);
        }

        if(GUI.Button (new Rect(530,60,230,170),"Code Red"))
        {
            SoundManager2D.playOneShotSound(codeRed);
        }

        //looping sounds

        if(GUI.Button (new Rect(30,265,230,80),"Start Squeak"))
        {
            SoundManager2D.playLoopingSound(squeakyBedLoop,"squeaky",1);
        }

        if(GUI.Button (new Rect(30,355,230,80),"Stop Squeak"))
        {
            SoundManager2D.stopLoopingSound("squeaky");
        }

        if(GUI.Button(new Rect(280,265,230,80),"Start Bell"))
        {
            SoundManager2D.playLoopingSound(bellLoop,"bell",2);
        }

        if(GUI.Button (new Rect(280,355,230,80),"Stop Bell"))
        {
            SoundManager2D.stopLoopingSound("bell");
        }

        if(GUI.Button(new Rect(530,265,230,80),"Start Funk"))
        {
            SoundManager2D.playLoopingSound(funkLoop,"funk",3);
        }

        if(GUI.Button(new Rect(530,355,230,80),"Stop Funk"))
        {
            SoundManager2D.stopLoopingSound("funk");
        }

        if(GUI.Button(new Rect(10,455,380,100),"Stop All Loops"))
        {
            SoundManager2D.stopAllLoopingSounds();
        }

        if(GUI.Button (new Rect(410,455,380,100),"Stop All One Shot Sounds"))
        {
            SoundManager2D.stopAllOneShotSounds();
        }
        
    }
}
