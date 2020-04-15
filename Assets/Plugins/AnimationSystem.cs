using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Kultie.Animation
{
    public class AnimationSystem
    {
        Sprite[] frames;
        bool loop;
        float spf;
        float time;
        int index;
        public AnimationSystem(Sprite[] frames, bool loop, float spf = 0.015f)
        {
            this.loop = loop;
            this.frames = frames;
            this.spf = spf;
            time = 0;
            index = 0;
        }

        public void ResetIndex() {
            index = 0;
        }

        public void Update(float dt)
        {
            time += dt;
            if (time >= spf)
            {
                index += 1;
                time = 0;
                if (index > frames.Length - 1)
                {
                    if (loop)
                    {
                        index = 0;
                    }
                    else
                    {
                        index = frames.Length - 1;
                    }
                }
            }
        }

        public void SetLoop(bool value) {
            loop = value;
        }

        public void SetSpf(float value)
        {
            spf = value;
        }

        public void SetFrames(Sprite[] frames)
        {
            this.frames = frames;
        }

        public Sprite Frame()
        {
            return frames[index];
        }

        public Sprite[] Frames()
        {
            return frames;
        }

        public bool IsFinished()
        {
            return !loop && index == frames.Length - 1;
        }

        public void ChangeSpeed(float value)
        {
            spf = spf / value;
        }
    }
}