using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sesion2_Lab01.com.isil.content;

using SharpDX.XAudio2;
using SharpDX.Multimedia;

namespace Sesion2_Lab01.com.isil.system.soundSystem {

    public class NSoundInstance {

        private XAudio2 mAudioEngine;
        private NSoundDevice mSoundDevice;

        private SourceVoice mSourceVoice;
        private SoundStream mSoundStream;
        private AudioBuffer mAudioBuffer;

        private bool mIsSoundFinished;
        private Action mOnFinishSound;

        public bool IsSoundFinished { get { return mIsSoundFinished; } }

        public NSoundInstance(NSoundDevice soundDevice) {
            mSoundDevice = soundDevice;
            mAudioEngine = soundDevice.AudioEngine;

            mIsSoundFinished = false;
        }

        public void BindDataBuffer(NSound soundNode) {
            this.BindDataBuffer(soundNode, 0);
        }

        public void BindDataBuffer(NSound soundNode, int loopCount) {
            mSoundStream = new SoundStream(soundNode.NativeFileStream);

            mAudioBuffer = new AudioBuffer(mSoundStream);
            mAudioBuffer.AudioBytes = (int)mSoundStream.Length;
            mAudioBuffer.Flags = BufferFlags.EndOfStream;
            mAudioBuffer.LoopCount = loopCount;

            mSourceVoice = new SourceVoice(mAudioEngine, mSoundStream.Format);
            mSourceVoice.SubmitSourceBuffer(mAudioBuffer, mSoundStream.DecodedPacketsInfo);
        }

        public void Play(Action onFinishSound) {
            mOnFinishSound = onFinishSound;

            mSourceVoice.Start();
        }

        public void Stop() {
            mSourceVoice.Stop();
        }

        public void Update(int dt) {
            if (!mIsSoundFinished) {
                if (mSourceVoice.State.BuffersQueued == 0) {
                    mIsSoundFinished = true;

                    // recycle the sound
                    this.Recycle();

                    if (mOnFinishSound != null) {
                        mOnFinishSound();
                        mOnFinishSound = null;
                    }
                }
            }
        }

        private void Recycle() {
            if (mAudioBuffer != null) {
                if (mAudioBuffer.Stream != null) {
                    mAudioBuffer.Stream.Dispose();
                }

                mAudioBuffer = null;
            }

            mSoundStream = null;

            if (mSourceVoice != null) {
                if (!mSourceVoice.IsDisposed) {
                    mSourceVoice.DestroyVoice();
                    mSourceVoice.Dispose();
                }

                mSourceVoice = null;
            }
        }
    }
}
