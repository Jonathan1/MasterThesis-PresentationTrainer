using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace PresentationTrainer
{
    public class PresentationAction
    {
        bool isMistake = true;
        public static double goodAverageVolume = 0.5;
        private double gravity = 0; //points at end of mistake. THis is so the mistake does not have to be calculated again at logging or reporting.
        public bool isCorrected = false;


        public enum GoodType
        {
            RESETPOSTURE
        };

        public GoodType myGoodie;

        public enum MistakeType
        {
            ARMSCROSSED,
            LEGSCROSSED,
            RIGHTHANDUNDERHIP,
            LEFTHANDUNDERHIP,
            //BOTHHANDUNDERHIP,
            RIGHTHANDBEHINDBACK,
            LEFTHANDBEHINDBACK,
            //BOTHHANDBEHINDBACK,
            HUNCHBACK,
            RIGHTLEAN,
            LEFTLEAN,
            RIGHTHANDNOTVISIBLE,
            LEFTHANDNOTVISIBLE,
            HANDS_NOT_MOVING,
            HANDS_MOVING_MUCH,
            DANCING,
            LONG_PAUSE,
            LONG_TALK,
            HIGH_VOLUME,
            LOW_VOLUME,
            LOW_MODULATION,
            HMMMM,
            NOMISTAKE
        };
        public MistakeType myMistake;

        public enum MetaType
        {
            NONE, VOLUME, CADENCE, POSTURE, HAND_MOVEMENT, PHONETIC_PAUSE, BODY_MOVEMENT
        };

        public MetaType meta;

        public bool hasFinished = false;
        public bool isVoiceAndMovementMistake = false;

        public double minVolume;
        public double maxVolume;
        public double averageVolume;
        public bool isSpeaking = false;

        public bool interrupt = false;

        public double totalHandMovement = 1000;
        public double averageHandMovement;

        public double timeStarted = 0;
        public double timeFinished;

        public ImageSource firstImage = null;
        public ImageSource lastImage = null;


        public string message = "";

        public double leftHandHipDistance;
        public double rightHandHipDistance;

        public PresentationAction()
        {

        }

        public PresentationAction(MistakeType myMistake)
        {
            this.myMistake = myMistake;
            determineMetaOfMistake();
            setMistakeDefaults();
            isMistake = true;
            timeStarted = DateTime.Now.TimeOfDay.TotalMilliseconds;
        }

        public PresentationAction(GoodType myGoodie)
        {
            this.myGoodie = myGoodie;
            setGoodieDefaults();
            isMistake = false;
            timeStarted = DateTime.Now.TimeOfDay.TotalMilliseconds;
        }

        //When a mistake ends, this method is called, for saving length and points.
        public void finishMistake(double gravity)
        {
            hasFinished = true;
            timeFinished = DateTime.Now.TimeOfDay.TotalMilliseconds;
            setGravity(gravity);
        }

        //clones the presentationAction object
        public PresentationAction Clone()
        {
            PresentationAction pa = new PresentationAction();

            pa.averageHandMovement = this.averageHandMovement;
            pa.averageVolume = this.averageVolume;
            if (firstImage != null)
            {
                pa.firstImage = this.firstImage.Clone();
            }

            pa.hasFinished = this.hasFinished;
            //pa.interrupt = this.interrupt;
            pa.isMistake = this.isMistake;
            pa.isSpeaking = this.isSpeaking;
            pa.isVoiceAndMovementMistake = this.isVoiceAndMovementMistake;
            if (lastImage != null)
            {
                pa.lastImage = this.lastImage.Clone();
            }

            pa.maxVolume = this.maxVolume;
            pa.message = this.message;
            pa.minVolume = this.minVolume;
            if (isMistake)
            {
                pa.myMistake = this.myMistake;
                pa.meta = this.meta;
            }
            else
            {
                pa.myGoodie = this.myGoodie;
            }
            pa.timeStarted = this.timeStarted;
            pa.timeFinished = this.timeFinished;
            pa.gravity = this.gravity;


            pa.totalHandMovement = this.totalHandMovement;

            return pa;
        }

        private void setGoodieDefaults()
        {
            switch (myGoodie)
            {
                case GoodType.RESETPOSTURE:
                    message = "Nice Posture";
                    break;
            }
        }

        public void setMistakeDefaults()
        {
            switch (myMistake)
            {
                case MistakeType.ARMSCROSSED:
                //case MistakeType.BOTHHANDBEHINDBACK:
                //case MistakeType.BOTHHANDUNDERHIP:
                case MistakeType.LEFTHANDBEHINDBACK:
                case MistakeType.LEFTHANDUNDERHIP:
                case MistakeType.LEGSCROSSED:
                case MistakeType.RIGHTHANDBEHINDBACK:
                case MistakeType.RIGHTHANDUNDERHIP:
                case MistakeType.HUNCHBACK:
                case MistakeType.RIGHTLEAN:
                case MistakeType.LEFTLEAN:
                    message = "Reset Posture";
                    break;
                case MistakeType.DANCING:
                    message = "Stand Still";
                    break;
                case MistakeType.HANDS_NOT_MOVING:
                    message = "Move Hands";
                    break;
                case MistakeType.HANDS_MOVING_MUCH:
                    message = "Stop Moving Hands";
                    break;
                case MistakeType.HIGH_VOLUME:
                    message = "Lower Volume";
                    break;
                case MistakeType.LOW_VOLUME:
                    message = "Increase Volume";
                    break;
                case MistakeType.LOW_MODULATION:
                    message = "Module Voice";
                    break;
                case MistakeType.LONG_PAUSE:
                    message = "Start Talking";
                    break;
                case MistakeType.LONG_TALK:
                    message = "Stop Talking";
                    break;
                case MistakeType.HMMMM:
                    message = "Stop HMMMMing";
                    break;
            }
        }

        //NEEDS goodietypes before it can work!
        private void determineMetaOfGoodie()
        {

        }

        //sets category of this mistake object
        private void determineMetaOfMistake()
        {
            switch (myMistake)
            {
                case MistakeType.ARMSCROSSED:    
                //case MistakeType.BOTHHANDBEHINDBACK:
                //case MistakeType.BOTHHANDUNDERHIP: 
                case MistakeType.LEFTHANDBEHINDBACK:
                case MistakeType.RIGHTHANDBEHINDBACK:                    
                case MistakeType.LEGSCROSSED:                    
                case MistakeType.RIGHTHANDUNDERHIP:
                case MistakeType.LEFTHANDUNDERHIP:                    
                case MistakeType.HUNCHBACK:
                case MistakeType.RIGHTLEAN:
                case MistakeType.LEFTLEAN:
                    meta = MetaType.POSTURE;
                    break;
                case MistakeType.DANCING:
                    meta = MetaType.BODY_MOVEMENT;
                    break;
                case MistakeType.HANDS_NOT_MOVING:
                case MistakeType.HANDS_MOVING_MUCH:
                    meta = MetaType.HAND_MOVEMENT;
                    break;
                case MistakeType.HIGH_VOLUME:
                case MistakeType.LOW_VOLUME:
                    meta = MetaType.VOLUME;
                    break;
                case MistakeType.LOW_MODULATION:
                case MistakeType.LONG_PAUSE:
                case MistakeType.LONG_TALK:
                    meta = MetaType.CADENCE;
                    break;
                case MistakeType.HMMMM:
                    meta = MetaType.PHONETIC_PAUSE;
                    break;
                default:
                    meta = MetaType.NONE;
                    break;
            }
        }
        
        public MetaType getMetaMistake()
        {
            return meta;
        }

        public static void setGoodAverageVolume(double gav)
        {
            goodAverageVolume = gav;
        }

        public double getMistakeMultiplier()
        {
            if (myMistake == MistakeType.HIGH_VOLUME || myMistake == MistakeType.LOW_VOLUME)
            {
                double x = Math.Abs(goodAverageVolume - averageVolume);
                return Math.Min(1, 5 * x);
            }
            else
                return 1;

        }

        public double lengthScore()
        {
            double points = DateTime.Now.TimeOfDay.TotalMilliseconds - timeStarted;
            return points;
        }

        public void setGravity(double ep)
        {
            gravity = ep;
        }

        public double getGravity()
        {
            return gravity;
        }

    }
}
