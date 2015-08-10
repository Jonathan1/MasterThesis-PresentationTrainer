using Microsoft.Kinect;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationTrainer
{
    public class RulesAnalyzerImproved
    {
        MainWindow parent;

        bool noMistake = true;
        bool freeStyle = true;
        bool interruptionNeeded = false;

        public bool interruption = true;
        public bool interrupted = false;

        BodyFramePreAnalysis bfpa;
        AudioPreAnalysis apa;
        Body oldBody;
        JudgementMaker myJudgementMaker;
        PresentationAction previousAction;

        public String userName = null;
        public int session = 1;

        public delegate void FeedBackEvent(object sender, PresentationAction x);
        public event FeedBackEvent feedBackEvent;

        public delegate void CorrectionEvent(object sender, PresentationAction x);
        public event CorrectionEvent correctionEvent;

        public delegate void InterruptionEvent(object sender, PresentationAction[] x);
        public event InterruptionEvent myInterruptionEvent;

        PresentationAction[] sentMistakes;
        PresentationAction[] crossedArms;
        PresentationAction[] handsUnderHips;
        PresentationAction[] handsBehindBack;
        PresentationAction[] hunchPosture;
        PresentationAction[] leaningPosture;
        PresentationAction[] highVolumes;
        PresentationAction[] lowVolumes;
        PresentationAction[] longPauses;
        PresentationAction[] longTalks;
        PresentationAction[] periodicMovements;
        PresentationAction[] hmmms;
        PresentationAction[] legsCrossed;
        PresentationAction[] handsNotMoving;
        PresentationAction[] handsMovingMuch;
        PresentationAction[] noModulation;

        ArrayList feedbackedMistakes;
        ArrayList mistakes;
        ArrayList interruptionsList;

        ArrayList crossedArmsList;
        ArrayList handsUnderHipsList;
        ArrayList handsBehindBackList;
        ArrayList hunchPostureList;
        ArrayList leaningPostureList;
        ArrayList highVolumesList;
        ArrayList lowVolumesList;
        ArrayList longPausesList;
        ArrayList longTalksList;
        ArrayList periodicMovementsList;
        ArrayList hmmmsList;
        ArrayList legsCrossedList;
        ArrayList handsNotMovingList;
        ArrayList handsMovingMuchList;
        ArrayList noModulationList;

        int crossedArmsMistakes = 0;
        int handsUnderHipsMistakes = 0;
        int handsBehindBackMistakes = 0;
        int hunchPostureMistakes = 0;
        int leaningPostureMistakes = 0;
        int highVolumesMistakes = 0;
        int lowVolumesMistakes = 0;
        int longPausesMistakes = 0;
        int longTalksMistakes = 0;
        int periodicMovementsMistakes = 0;
        int hmmmsMistakes = 0;
        int legsCrossedMistakes = 0;
        int handsNotMovingMistakes = 0;
        int handsMovingMuchMistakes = 0;
        int noModulationMistakes = 0;

        int crossedArmsCorrections = 0;
        int handsUnderHipsCorrections = 0;
        int handsBehindBackCorrections = 0;
        int hunchPostureCorrections = 0;
        int leaningPostureCorrections = 0;
        int highVolumesCorrections = 0;
        int lowVolumesCorrections = 0;
        int longPausesCorrections = 0;
        int longTalksCorrections = 0;
        int periodicMovementsCorrections = 0;
        int hmmmsCorrections = 0;
        int legsCrossedCorrections = 0;
        int handsNotMovingCorrections = 0;
        int handsMovingMuchCorrections = 0;
        int noModulationCorrections = 0;

        double previouscrossedArmsMistakes = 0;
        double previoushandsUnderHipsMistakes = 0;
        double previoushandsBehindBackMistakes = 0;
        double previoushunchPostureMistakes = 0;
        double previousleaningPostureMistakes = 0;
        double previoushighVolumesMistakes = 0;
        double previouslowVolumesMistakes = 0;
        double previouslongPausesMistakes = 0;
        double previouslongTalksMistakes = 0;
        double previousperiodicMovementsMistakes = 0;
        double previoushmmmsMistakes = 0;
        double previouslegsCrossedMistakes = 0;
        double previoushandsNotMovingMistakes = 0;
        double previoushandsMovingMuchMistakes = 0;
        double previousnoModulationMistakes = 0;

        double previouscrossedArmsCorrections = 0;
        double previoushandsUnderHipsCorrections = 0;
        double previoushandsBehindBackCorrections = 0;
        double previoushunchPostureCorrections = 0;
        double previousleaningPostureCorrections = 0;
        double previoushighVolumesCorrections = 0;
        double previouslowVolumesCorrections = 0;
        double previouslongPausesCorrections = 0;
        double previouslongTalksCorrections = 0;
        double previousperiodicMovementsCorrections = 0;
        double previoushmmmsCorrections = 0;
        double previouslegsCrossedCorrections = 0;
        double previoushandsNotMovingCorrections = 0;
        double previoushandsMovingMuchCorrections = 0;
        double previousnoModulationCorrections = 0;

        public double lastFeedbackTime = 0;
        public double timeBetweenFeedbacks = 3500;
        public bool noInterrupt = true;

        private int interruptionThreshold = 40000;
        private PresentationAction.MistakeType feedbackMistakeType;
        private int feedbackEntry;
        private double feedbackGravity;

        public string mostRepeatedMistake = "Default string";
        public string mostRepeatedMetaMistake = "Default string";
        public string mostRepeatedCorrection = "Default string";
        public string mostRepeatedMetaCorrection = "Default string";

        public RulesAnalyzerImproved(MainWindow parent)
        {
            this.parent = parent;
            myJudgementMaker = new JudgementMaker(parent);
            feedbackedMistakes = new ArrayList();
            mistakes = new ArrayList();
            interruptionsList = new ArrayList();

            crossedArms = new PresentationAction[interruptionThreshold];
            handsUnderHips = new PresentationAction[interruptionThreshold];
            handsBehindBack = new PresentationAction[interruptionThreshold];
            hunchPosture = new PresentationAction[interruptionThreshold];
            leaningPosture = new PresentationAction[interruptionThreshold];
            highVolumes = new PresentationAction[interruptionThreshold];
            lowVolumes = new PresentationAction[interruptionThreshold];
            longPauses = new PresentationAction[interruptionThreshold];
            longTalks = new PresentationAction[interruptionThreshold];
            periodicMovements = new PresentationAction[interruptionThreshold];
            hmmms = new PresentationAction[interruptionThreshold];
            legsCrossed = new PresentationAction[interruptionThreshold];
            handsNotMoving = new PresentationAction[interruptionThreshold];
            handsMovingMuch = new PresentationAction[interruptionThreshold];
            noModulation = new PresentationAction[interruptionThreshold];

            crossedArmsList = new ArrayList();
            handsUnderHipsList = new ArrayList();
            handsBehindBackList = new ArrayList();
            hunchPostureList = new ArrayList();
            leaningPostureList = new ArrayList();
            highVolumesList = new ArrayList();
            lowVolumesList = new ArrayList();
            longPausesList = new ArrayList();
            longTalksList = new ArrayList();
            periodicMovementsList = new ArrayList();
            hmmmsList = new ArrayList();
            legsCrossedList = new ArrayList();
            handsNotMovingList = new ArrayList();
            handsMovingMuchList = new ArrayList();
            noModulationList = new ArrayList();

            previousAction = new PresentationAction();
            previousAction.myMistake = PresentationAction.MistakeType.NOMISTAKE;
        }

        public void setFreeStyle(bool fs)
        {
            freeStyle = fs;
        }

        #region analysisCycle

        public void AnalyseRules()
        {
            if (parent.freestyleMode != null)
            {
                if (parent.freestyleMode.myState == PresentationTrainer.FreestyleMode.currentState.play)
                {                    

                    myJudgementMaker.analyze();

                    //update lists
                    if (myJudgementMaker.mistakeList.Count > 0)
                    {
                        updateMistakeList();
                    }
                    else
                    {

                        //REPLACED with new function, so it actually saves all mistakes. 
                        saveAndClear();
                    }

                    if (checkTimeToGiveFeedback() == true)
                    {
                        bool didIGiveFeedback = false;

                        //give correction when no mistakes and previous mistake is not NOMISTAKE. Add the previous mistake to feedback list.
                        if (mistakes.Count == 0 && previousAction.myMistake != PresentationAction.MistakeType.NOMISTAKE)
                        {
                            feedbackedMistakes.Add(previousAction);
                            doGoodEventStuff();//plays correctionevent

                            //put previous mistake to no Mistake
                            //start timer
                        }

                        //First: if feedback mistake is no longer in the list, it should be removed, and the correction event played. The mistake is then saved in the feedback mistake list.
                        //ELSE If previousaction was noMistake -> Do formula call and feedback the chosen mistake
                        //<Also, below are 2 if statements, which can be replaced with if-else>
                        //If we need to interrupt, do it here.
                        if (mistakes.Count > 0)
                        {
                            if (previousAction.myMistake != PresentationAction.MistakeType.NOMISTAKE)
                            {
                                bool previousMistakeStillHere = false;
                                for (int i = 0; i < mistakes.Count; i++)
                                {
                                    if (((PresentationAction)mistakes[i]).myMistake == previousAction.myMistake)
                                    {
                                        previousMistakeStillHere = true;
                                        break;
                                    }
                                }
                                if (!previousMistakeStillHere)
                                {
                                    feedbackedMistakes.Add(previousAction);
                                    doGoodEventStuff();
                                    // mistakes.Clear();
                                    didIGiveFeedback = true;
                                }
                            }


                            if (didIGiveFeedback == false)
                            {
                                doFeedbackEventStuff();
                            }
                        }


                    }
                }



            }
        }        

        //If threshold is reached, next feedback can be given
        public bool checkTimeToGiveFeedback()
        {
            bool giveFeedback = false;

            double currentTime = DateTime.Now.TimeOfDay.TotalMilliseconds;

            if (currentTime - lastFeedbackTime > timeBetweenFeedbacks)
            {
                giveFeedback = true;
            }

            return giveFeedback;
        }

        #endregion

        // update, lists add mistakes, delete no longer mistakes
        #region manageLists

        //Update lists, when mistakes are removed, add them to the corresponding list
        //If no new mistake of same meta category: add to corresponding correction
        private void updateMistakeList()
        {
            if (mistakes.Count > 0)
            {
                deleteNoLongerMistakes();
            }

            

            addNewMistakes();
           // handleBothHandMistakes();
        }

        //Adds mistakes from judgementmaker if they are not present in rulesanalyzer yet.
        private void addNewMistakes()
        {
            foreach (PresentationAction ba in myJudgementMaker.mistakeList)
            {
                bool isAlreadyThere = false;
                foreach (PresentationAction a in mistakes)
                {
                    if (ba.myMistake == a.myMistake)
                    {
                        isAlreadyThere = true;
                        break;
                    }
                }
                if (isAlreadyThere == false)
                {
                    mistakes.Add(ba);
                }
            }   
        }

        

        //saves the removed mistakes to corresponding list
        private void deleteNoLongerMistakes()
        {
            int[] nolongerErrors = new int[mistakes.Count];
            int i = 0;            
            foreach (PresentationAction a in mistakes)
            {               
                    foreach (PresentationAction ba in myJudgementMaker.mistakeList)//judgementMaker.mistakeList
                    {
                        if (a.myMistake == ba.myMistake)
                        {
                            nolongerErrors[i] = 1;
                            
                            break;
                        }
                    }               
                i++;
            }

            for (int j = nolongerErrors.Length; j > 0; j--)
            {
                if (nolongerErrors[j - 1] == 0)
                {
                    PresentationAction x = (PresentationAction)mistakes[j - 1];

                    bool checkIfCorrected = true;
                    foreach (PresentationAction ba in myJudgementMaker.mistakeList)
                    {
                        if (ba.meta == x.meta)
                        {
                            checkIfCorrected = false;
                            break;
                        }
                    }
                    if (checkIfCorrected)
                    {
                        x.isCorrected = true;
                        increaseCorrections(x);
                    }
                        
                    x.finishMistake(getGravity(x));
                    checkWhereToPutMistake(x); //repetitions is increased here                    

                    mistakes.RemoveAt(j - 1);
                }
            }
            
        }

        //Increases corrections for given mistake
        private void increaseCorrections(PresentationAction temp)
        {
            switch (temp.myMistake)
            {
                case PresentationAction.MistakeType.ARMSCROSSED:
                    crossedArmsCorrections++;
                    break;
                case PresentationAction.MistakeType.HANDS_MOVING_MUCH:
                    handsMovingMuchCorrections++;
                    break;
                case PresentationAction.MistakeType.HANDS_NOT_MOVING:
                    handsNotMovingCorrections++;
                    break;
                case PresentationAction.MistakeType.HIGH_VOLUME:
                    highVolumesCorrections++;
                    break;
                case PresentationAction.MistakeType.HMMMM:
                    hmmmsCorrections++;
                    break;
                case PresentationAction.MistakeType.HUNCHBACK:
                    hunchPostureCorrections++;
                    break;
                //case PresentationAction.MistakeType.BOTHHANDBEHINDBACK:
                case PresentationAction.MistakeType.LEFTHANDBEHINDBACK:
                case PresentationAction.MistakeType.RIGHTHANDBEHINDBACK:
                    handsBehindBackCorrections++;
                    break;
                //case PresentationAction.MistakeType.BOTHHANDUNDERHIP:
                case PresentationAction.MistakeType.LEFTHANDUNDERHIP:
                case PresentationAction.MistakeType.RIGHTHANDUNDERHIP:
                    handsUnderHipsCorrections++;
                    break;
                case PresentationAction.MistakeType.LONG_PAUSE:
                    longPausesCorrections++;
                    break;
                case PresentationAction.MistakeType.LONG_TALK:
                    longTalksCorrections++;
                    break;
                case PresentationAction.MistakeType.LOW_MODULATION:
                    noModulationCorrections++;
                    break;
                case PresentationAction.MistakeType.LOW_VOLUME:
                    lowVolumesCorrections++;
                    break;
                case PresentationAction.MistakeType.LEFTLEAN:
                case PresentationAction.MistakeType.RIGHTLEAN:
                    leaningPostureCorrections++;
                    break;
                case PresentationAction.MistakeType.DANCING:
                    periodicMovementsCorrections++;
                    break;
                default:
                    break;
            }
        }

        //Increases repetitions for given mistake
        private void increaseRepetitions(PresentationAction temp)
        {
            switch (temp.myMistake)
            {
                case PresentationAction.MistakeType.ARMSCROSSED:
                    crossedArmsMistakes++;
                    break;
                case PresentationAction.MistakeType.HANDS_MOVING_MUCH:
                    handsMovingMuchMistakes++;
                    break;
                case PresentationAction.MistakeType.HANDS_NOT_MOVING:
                    handsNotMovingMistakes++;
                    break;
                case PresentationAction.MistakeType.HIGH_VOLUME:
                    highVolumesMistakes++;
                    break;
                case PresentationAction.MistakeType.HMMMM:
                    hmmmsMistakes++;
                    break;
                case PresentationAction.MistakeType.HUNCHBACK:
                    hunchPostureMistakes++;
                    break;
                //case PresentationAction.MistakeType.BOTHHANDBEHINDBACK:
                case PresentationAction.MistakeType.LEFTHANDBEHINDBACK:
                case PresentationAction.MistakeType.RIGHTHANDBEHINDBACK:
                    handsBehindBackMistakes++;
                    break;
                //case PresentationAction.MistakeType.BOTHHANDUNDERHIP:
                case PresentationAction.MistakeType.LEFTHANDUNDERHIP:
                case PresentationAction.MistakeType.RIGHTHANDUNDERHIP:
                    handsUnderHipsMistakes++;
                    break;
                case PresentationAction.MistakeType.LONG_PAUSE:
                    longPausesMistakes++;
                    break;
                case PresentationAction.MistakeType.LONG_TALK:
                    longTalksMistakes++;
                    break;
                case PresentationAction.MistakeType.LOW_MODULATION:
                    noModulationMistakes++;
                    break;
                case PresentationAction.MistakeType.LOW_VOLUME:
                    lowVolumesMistakes++;
                    break;
                case PresentationAction.MistakeType.LEFTLEAN:
                case PresentationAction.MistakeType.RIGHTLEAN:
                    leaningPostureMistakes++;
                    break;
                case PresentationAction.MistakeType.DANCING:
                    periodicMovementsMistakes++;
                    break;
                default:
                    break;
            }
        }

        //saves mistake in corresponding array
        private void saveAndClear()
        {
            foreach (PresentationAction a in mistakes)
            {
                a.finishMistake(getGravity(a));
                checkWhereToPutMistake(a); //this method still needs parameter requirement
            }

            mistakes.Clear();
        }

        #endregion

        //sending feedbacks, corrections, and interruptions
        #region sendFeedbacks

        //Plays correction event and updates stuff for feedback
        private void doGoodEventStuff()
        {
            correctionEvent(this, previousAction);

            lastFeedbackTime = DateTime.Now.TimeOfDay.TotalMilliseconds;
            previousAction.myMistake = PresentationAction.MistakeType.NOMISTAKE;
            myJudgementMaker.clearLists();

        }

        //This method plays an event and handles getting the correct images for the given mistake entry.
        private void doFeedbackEventStuff()
        {
            double currentTime = DateTime.Now.TimeOfDay.TotalMilliseconds;


            // decision formula returns integer of the place of the mistake to be feedbacked within the mistakes list.
            int feedbackMistakeEntry = 0;
            feedbackMistakeEntry = decisionFormula();
            ((PresentationAction)mistakes[feedbackMistakeEntry]).timeFinished = currentTime;


            if (previousAction.myMistake != ((PresentationAction)mistakes[feedbackMistakeEntry]).myMistake)
            {
                if (((PresentationAction)mistakes[feedbackMistakeEntry]).myMistake == PresentationAction.MistakeType.HANDS_NOT_MOVING)
                {
                    ((PresentationAction)mistakes[feedbackMistakeEntry]).firstImage = myJudgementMaker.myVoiceAndMovementObject.firstImage;
                    ((PresentationAction)mistakes[feedbackMistakeEntry]).lastImage = myJudgementMaker.myVoiceAndMovementObject.lastImage;
                }
                feedBackEvent(this, (PresentationAction)mistakes[feedbackMistakeEntry]); //Somehow this gives null reference!?
                //checkWhereToPutMistake(); 
                if (noInterrupt == true)
                {
                    previousAction = (PresentationAction)mistakes[feedbackMistakeEntry];
                }

            }
            else if (((PresentationAction)mistakes[feedbackMistakeEntry]).interrupt == true)
            {
                //checkWhereToPutMistake(); 
                bigMistakeInterruption(feedbackMistakeEntry);

            }

        }

        //Calculates the points for a mistake and returns integer of the entry of this mistake in the mistake list.
        private int decisionFormula()
        {
            feedbackMistakeType = PresentationAction.MistakeType.NOMISTAKE;
            feedbackGravity = 0;
            feedbackEntry = 0;

            // Go through all current mistakes and calculate points
            // -> this means get repetition and length of mistake as points of mistake
            // -> keep track of biggest mistake
            // return mistake with highest points

            for (int i = 0; i < mistakes.Count; i++)
            {
                PresentationAction temp = (PresentationAction)mistakes[i];
                double gravity = getGravity(temp);
                if (gravity > feedbackGravity)
                {
                    feedbackGravity = gravity;
                    feedbackEntry = i;
                    feedbackMistakeType = temp.myMistake;
                }
            }

            return feedbackEntry;
        }

        //Calculates the gravity (points) of the given mistake
        private double getGravity(PresentationAction temp)
        {
            double timePoints = 0.5; //every x seconds counts as a mistake.
            double metaMultiplier = 0.5; //how important a meta repetition is.
            double correctionMultiplier = 0.5; //How important a correction is.

            double gravity = 0;
            double lengthPoints = (temp.lengthScore()/1000) / timePoints; //lengthScore() gives back time of mistake in milliseconds

            int tempRep = getRepetition(temp) + 1; // can be zero since repetition is only updated after mistake ended.
            int tempMetaRep = getMetaRepetition(temp) + 1 - tempRep;//  Dont count the repetition of current mistake in meta Repetition

            double metaPoints = ((tempMetaRep / getMetaValue(temp)) * metaMultiplier);
            double tempCorr = getCorrections(temp) * correctionMultiplier;

            double repPoints = tempRep + metaPoints;
            gravity = lengthPoints + repPoints - tempCorr;

            return gravity;
        }

        //Gets the repetition of the metamistake the given mistake belongs to.
        private int getMetaRepetition(PresentationAction temp)
        {
            int previousMistakes = 0;
            switch (temp.meta)
            {
                case PresentationAction.MetaType.POSTURE:
                    previousMistakes = (int) (previousleaningPostureMistakes + previoushunchPostureMistakes + previoushandsUnderHipsMistakes + previoushandsBehindBackMistakes + previouscrossedArmsMistakes);
                    return leaningPostureMistakes + hunchPostureMistakes + handsUnderHipsMistakes + handsBehindBackMistakes + crossedArmsMistakes + previousMistakes;

                case PresentationAction.MetaType.HAND_MOVEMENT:
                    previousMistakes = (int) (previoushandsNotMovingMistakes + previoushandsMovingMuchMistakes);
                    return handsNotMovingMistakes + handsMovingMuchMistakes + previousMistakes;

                case PresentationAction.MetaType.VOLUME:
                    previousMistakes = (int) (previouslowVolumesMistakes + previoushighVolumesMistakes);
                    return lowVolumesMistakes + highVolumesMistakes + previousMistakes;

                case PresentationAction.MetaType.PHONETIC_PAUSE:
                    previousMistakes = (int) previoushmmmsMistakes;
                    return hmmmsMistakes + previousMistakes;

                case PresentationAction.MetaType.CADENCE:
                    previousMistakes = (int) (previousnoModulationMistakes + previouslongPausesMistakes + previouslongTalksMistakes);
                    return noModulationMistakes + longPausesMistakes + longTalksMistakes + previousMistakes;

                case PresentationAction.MetaType.BODY_MOVEMENT:
                    previousMistakes = (int) previousperiodicMovementsMistakes;
                    return periodicMovementsMistakes + previousMistakes;

                default:
                    return 0;
            }
        }

        //MetaValue is how many lists are used under this meta category in this class.        
        private int getMetaValue(PresentationAction temp)
        {
            switch (temp.meta)
            {
                case PresentationAction.MetaType.POSTURE:
                    return 5;

                case PresentationAction.MetaType.HAND_MOVEMENT:
                    return 2;

                case PresentationAction.MetaType.VOLUME:
                    return 2;

                case PresentationAction.MetaType.PHONETIC_PAUSE:
                    return 1;

                case PresentationAction.MetaType.CADENCE:
                    return 3;

                case PresentationAction.MetaType.BODY_MOVEMENT:
                    return 1;

                default:
                    return 1;
            }
        }

        //Gets the number of repetitions of the given mistake.
        private int getRepetition(PresentationAction temp)
        {
            int previousMistakes = 0;
            switch (temp.myMistake)
            {
                case PresentationAction.MistakeType.ARMSCROSSED:
                    previousMistakes = (int)previouscrossedArmsMistakes;
                    return crossedArmsMistakes + previousMistakes;
                case PresentationAction.MistakeType.HANDS_MOVING_MUCH:
                    previousMistakes = (int)previoushandsMovingMuchMistakes;
                    return handsMovingMuchMistakes + previousMistakes;
                case PresentationAction.MistakeType.HANDS_NOT_MOVING:
                    previousMistakes = (int)previoushandsNotMovingMistakes;
                    return handsNotMovingMistakes + previousMistakes;
                case PresentationAction.MistakeType.HIGH_VOLUME:
                    previousMistakes = (int)previoushighVolumesMistakes;
                    return highVolumesMistakes + previousMistakes;
                case PresentationAction.MistakeType.HMMMM:
                    previousMistakes = (int)previoushmmmsMistakes;
                    return hmmmsMistakes + previousMistakes;
                case PresentationAction.MistakeType.HUNCHBACK:
                    previousMistakes = (int)previoushunchPostureMistakes;
                    return hunchPostureMistakes + previousMistakes;
                //case PresentationAction.MistakeType.BOTHHANDBEHINDBACK:
                case PresentationAction.MistakeType.LEFTHANDBEHINDBACK:
                case PresentationAction.MistakeType.RIGHTHANDBEHINDBACK:
                    previousMistakes = (int)previoushandsBehindBackMistakes;
                    return handsBehindBackMistakes + previousMistakes;
                //case PresentationAction.MistakeType.BOTHHANDUNDERHIP:
                case PresentationAction.MistakeType.LEFTHANDUNDERHIP:
                case PresentationAction.MistakeType.RIGHTHANDUNDERHIP:
                    previousMistakes = (int)previoushandsUnderHipsMistakes;
                    return handsUnderHipsMistakes + previousMistakes;
                case PresentationAction.MistakeType.LONG_PAUSE:
                    previousMistakes = (int)previouslongPausesMistakes;
                    return longPausesMistakes + previousMistakes;
                case PresentationAction.MistakeType.LONG_TALK:
                    previousMistakes = (int)previouslongTalksMistakes;
                    return longTalksMistakes + previousMistakes;
                case PresentationAction.MistakeType.LOW_MODULATION:
                    previousMistakes = (int)previousnoModulationMistakes;
                    return noModulationMistakes + previousMistakes;
                case PresentationAction.MistakeType.LOW_VOLUME:
                    previousMistakes = (int)previouslowVolumesMistakes;
                    return lowVolumesMistakes + previousMistakes;
                case PresentationAction.MistakeType.LEFTLEAN:
                case PresentationAction.MistakeType.RIGHTLEAN:
                    previousMistakes = (int)previousleaningPostureMistakes;
                    return leaningPostureMistakes + previousMistakes;
                case PresentationAction.MistakeType.DANCING:
                    previousMistakes = (int)previousperiodicMovementsMistakes;
                    return periodicMovementsMistakes + previousMistakes;
                default:
                    return 0;
            }
        }

        //Gets the number of corrections of the given mistake.
        private int getCorrections(PresentationAction temp)
        {
            int previousCorrections = 0;
            switch (temp.myMistake)
            {
                case PresentationAction.MistakeType.ARMSCROSSED:
                    previousCorrections = (int)previouscrossedArmsCorrections;
                    return crossedArmsCorrections + previousCorrections;
                case PresentationAction.MistakeType.HANDS_MOVING_MUCH:
                    previousCorrections = (int)previoushandsMovingMuchCorrections;
                    return handsMovingMuchCorrections + previousCorrections;
                case PresentationAction.MistakeType.HANDS_NOT_MOVING:
                    previousCorrections = (int)previoushandsNotMovingCorrections;
                    return handsNotMovingCorrections + previousCorrections;
                case PresentationAction.MistakeType.HIGH_VOLUME:
                    previousCorrections = (int)previoushighVolumesCorrections;
                    return highVolumesCorrections + previousCorrections;
                case PresentationAction.MistakeType.HMMMM:
                    previousCorrections = (int)previoushmmmsCorrections;
                    return hmmmsCorrections + previousCorrections;
                case PresentationAction.MistakeType.HUNCHBACK:
                    previousCorrections = (int)previoushunchPostureCorrections;
                    return hunchPostureCorrections + previousCorrections;
                //case PresentationAction.MistakeType.BOTHHANDBEHINDBACK:
                case PresentationAction.MistakeType.LEFTHANDBEHINDBACK:
                case PresentationAction.MistakeType.RIGHTHANDBEHINDBACK:
                    previousCorrections = (int)previoushandsBehindBackCorrections;
                    return handsBehindBackCorrections + previousCorrections;
                //case PresentationAction.MistakeType.BOTHHANDUNDERHIP:
                case PresentationAction.MistakeType.LEFTHANDUNDERHIP:
                case PresentationAction.MistakeType.RIGHTHANDUNDERHIP:
                    previousCorrections = (int)previoushandsUnderHipsCorrections;
                    return handsUnderHipsCorrections + previousCorrections;
                case PresentationAction.MistakeType.LONG_PAUSE:
                    previousCorrections = (int)previouslongPausesCorrections;
                    return longPausesCorrections + previousCorrections;
                case PresentationAction.MistakeType.LONG_TALK:
                    previousCorrections = (int)previouslongTalksCorrections;
                    return longTalksCorrections + previousCorrections;
                case PresentationAction.MistakeType.LOW_MODULATION:
                    previousCorrections = (int)previousnoModulationCorrections;
                    return noModulationCorrections + previousCorrections;
                case PresentationAction.MistakeType.LOW_VOLUME:
                    previousCorrections = (int)previouslowVolumesCorrections;
                    return lowVolumesCorrections + previousCorrections;
                case PresentationAction.MistakeType.LEFTLEAN:
                case PresentationAction.MistakeType.RIGHTLEAN:
                    previousCorrections = (int)previousleaningPostureCorrections;
                    return leaningPostureCorrections + previousCorrections;
                case PresentationAction.MistakeType.DANCING:
                    previousCorrections = (int)previousperiodicMovementsCorrections;
                    return periodicMovementsCorrections + previousCorrections;
                default:
                    return 0;
            }
        }

        //Triggers interruption event
        public void doInterruptionThing()
        {
            noInterrupt = false;
            myInterruptionEvent(this, sentMistakes);

            // doGoodEventStuff();

            //   myJudgementMaker.mistakeList = new ArrayList();
            //   myJudgementMaker.tempMistakeList = new ArrayList();
            // // myJudgementMaker.audioMovementMistakeTempList = new ArrayList();
        }

        //Resets mistakes and judgementmaker after a pause
        public void resetAfterPause()
        {
            //resetAllMistakeArrays(); 
            mistakes = new ArrayList();
            previousAction.myMistake = PresentationAction.MistakeType.NOMISTAKE;
            myJudgementMaker = new JudgementMaker(parent);
            noInterrupt = true;
        }

        //Resets mistakes and judgementmaker after a pause
        public void resetAfterInterruption(PresentationAction mistakeX)
        {
            resetMistakeArray(mistakeX);
            mistakes = new ArrayList();
            previousAction.myMistake = PresentationAction.MistakeType.NOMISTAKE;
            myJudgementMaker = new JudgementMaker(parent);
            noInterrupt = true;
        }

        #endregion

        #region handleInterruptions
        //Handles interruption for a mistake that reached size threshold
        private void bigMistakeInterruption(int feedbackMistake)
        {
            switch (((PresentationAction)mistakes[feedbackMistake]).myMistake)
            {
                case PresentationAction.MistakeType.ARMSCROSSED:

                    sentMistakes = crossedArms;
                    break;
               // case PresentationAction.MistakeType.BOTHHANDBEHINDBACK:
                case PresentationAction.MistakeType.LEFTHANDBEHINDBACK:
                case PresentationAction.MistakeType.RIGHTHANDBEHINDBACK:
                    sentMistakes = handsBehindBack;
                    break;
                case PresentationAction.MistakeType.LEGSCROSSED:
                    sentMistakes = legsCrossed;
                    break;
                //case PresentationAction.MistakeType.BOTHHANDUNDERHIP:
                case PresentationAction.MistakeType.RIGHTHANDUNDERHIP:
                case PresentationAction.MistakeType.LEFTHANDUNDERHIP:
                    sentMistakes = handsUnderHips;
                    break;
                case PresentationAction.MistakeType.HUNCHBACK:
                    sentMistakes = hunchPosture;
                    break;
                case PresentationAction.MistakeType.RIGHTLEAN:
                case PresentationAction.MistakeType.LEFTLEAN:
                    sentMistakes = leaningPosture;
                    break;
                case PresentationAction.MistakeType.DANCING:
                    sentMistakes = periodicMovements;
                    break;
                case PresentationAction.MistakeType.HANDS_NOT_MOVING:
                    sentMistakes = handsNotMoving;
                    break;
                case PresentationAction.MistakeType.HANDS_MOVING_MUCH:
                    sentMistakes = handsMovingMuch;
                    break;
                case PresentationAction.MistakeType.HIGH_VOLUME:
                    sentMistakes = highVolumes;
                    break;
                case PresentationAction.MistakeType.LOW_VOLUME:
                    sentMistakes = lowVolumes;
                    break;
                case PresentationAction.MistakeType.LOW_MODULATION:
                    sentMistakes = noModulation;
                    break;
                case PresentationAction.MistakeType.LONG_PAUSE:
                    sentMistakes = longPauses;
                    break;
                case PresentationAction.MistakeType.LONG_TALK:
                    sentMistakes = longTalks;
                    break;
                case PresentationAction.MistakeType.HMMMM:
                    sentMistakes = hmmms;
                    break;
            }
            interruptionsList.Add(mistakes[feedbackMistake]);
            doInterruptionThing();
        }
        
        //Resets the mistake lists after an interruption.
        private void resetMistakeArray(PresentationAction mistakeX)
        {
            switch (((PresentationAction)mistakeX).myMistake)
            {
                case PresentationAction.MistakeType.ARMSCROSSED:
                    crossedArmsList.AddRange(crossedArms);
                    crossedArms = new PresentationAction[interruptionThreshold];
                    //crossedArmsMistakes++;
                    break;
                //case PresentationAction.MistakeType.BOTHHANDBEHINDBACK:
                case PresentationAction.MistakeType.LEFTHANDBEHINDBACK:
                case PresentationAction.MistakeType.RIGHTHANDBEHINDBACK:
                    handsBehindBackList.AddRange(handsBehindBack);
                    handsBehindBack = new PresentationAction[interruptionThreshold];
                    //handsBehindBackMistakes++;
                    break;
                case PresentationAction.MistakeType.LEGSCROSSED:
                    legsCrossedList.AddRange(legsCrossed);
                    legsCrossed = new PresentationAction[interruptionThreshold];
                    //legsCrossedMistakes++;
                    break;
                //case PresentationAction.MistakeType.BOTHHANDUNDERHIP:
                case PresentationAction.MistakeType.RIGHTHANDUNDERHIP:
                case PresentationAction.MistakeType.LEFTHANDUNDERHIP:
                    handsUnderHipsList.AddRange(handsUnderHips);
                    handsUnderHips = new PresentationAction[interruptionThreshold];
                    //handsUnderHipsMistakes++;
                    break;
                case PresentationAction.MistakeType.HUNCHBACK:
                    hunchPostureList.AddRange(hunchPosture);
                    hunchPosture = new PresentationAction[interruptionThreshold];
                    //hunchPostureMistakes++;
                    break;
                case PresentationAction.MistakeType.RIGHTLEAN:
                case PresentationAction.MistakeType.LEFTLEAN:
                    leaningPostureList.AddRange(leaningPosture);
                    leaningPosture = new PresentationAction[interruptionThreshold];
                    //leaningPostureMistakes++;
                    break;
                case PresentationAction.MistakeType.DANCING:
                    periodicMovementsList.AddRange(periodicMovements);
                    periodicMovements = new PresentationAction[interruptionThreshold];
                    //periodicMovementsMistakes++;
                    break;
                case PresentationAction.MistakeType.HANDS_NOT_MOVING:
                    handsNotMovingList.AddRange(handsNotMoving);
                    handsNotMoving = new PresentationAction[interruptionThreshold];
                    //handsNotMovingMistakes++;
                    break;
                case PresentationAction.MistakeType.HANDS_MOVING_MUCH:
                    handsMovingMuchList.AddRange(handsMovingMuch);
                    handsMovingMuch = new PresentationAction[interruptionThreshold];
                    //handsMovingMuchMistakes++;
                    break;
                case PresentationAction.MistakeType.HIGH_VOLUME:
                    highVolumesList.AddRange(highVolumes);
                    highVolumes = new PresentationAction[interruptionThreshold];
                    //highVolumesMistakes++;
                    break;
                case PresentationAction.MistakeType.LOW_VOLUME:
                    lowVolumesList.AddRange(lowVolumes);
                    lowVolumes = new PresentationAction[interruptionThreshold];
                    //lowVolumesMistakes++;
                    break;
                case PresentationAction.MistakeType.LOW_MODULATION:
                    noModulationList.AddRange(noModulation);
                    noModulation = new PresentationAction[interruptionThreshold];
                    //noModulationMistakes++;
                    break;
                case PresentationAction.MistakeType.LONG_PAUSE:
                    longPausesList.AddRange(longPauses);
                    longPauses = new PresentationAction[interruptionThreshold];
                    //longPausesMistakes++;
                    break;
                case PresentationAction.MistakeType.LONG_TALK:
                    longTalksList.AddRange(longTalks);
                    longTalks = new PresentationAction[interruptionThreshold];
                    //longTalksMistakes++;
                    break;
                case PresentationAction.MistakeType.HMMMM:
                    hmmmsList.AddRange(hmmms);
                    hmmms = new PresentationAction[interruptionThreshold];
                    //hmmmsMistakes++;
                    break;
            }
        }
       
        //Here mistakes are added to their respective arrays. If the number threshold is reached, an interruption is made
        private void checkWhereToPutMistake(PresentationAction mistakeX)
        {
            int i = 0;
            //int x = 0;
            // sentMistakes;
            PresentationAction temp = new PresentationAction();
            temp = ((PresentationAction)mistakeX).Clone();

            switch (temp.myMistake)
            {
                case PresentationAction.MistakeType.ARMSCROSSED:
                    crossedArmsMistakes++;
                    for (i = 0; i < interruptionThreshold; i++)
                    {
                        if (crossedArms[i] == null)
                        {
                            crossedArms[i] = temp;
                            crossedArms[i].myMistake = PresentationAction.MistakeType.ARMSCROSSED;
                            break;
                        }
                    }
                    sentMistakes = crossedArms;
                    break;
                //case PresentationAction.MistakeType.BOTHHANDBEHINDBACK:
                //    handsBehindBackMistakes++;
                //    for (i = 0; i < interruptionThreshold; i++)
                //    {
                //        if (handsBehindBack[i] == null)
                //        {
                //            handsBehindBack[i] = temp;
                //            handsBehindBack[i].myMistake = PresentationAction.MistakeType.BOTHHANDBEHINDBACK;
                //            break;
                //        }
                //    }
                //    sentMistakes = handsBehindBack;
                //    break;
                case PresentationAction.MistakeType.LEFTHANDBEHINDBACK:
                    handsBehindBackMistakes++;
                    for (i = 0; i < interruptionThreshold; i++)
                    {
                        if (handsBehindBack[i] == null)
                        {
                            handsBehindBack[i] = temp;
                            handsBehindBack[i].myMistake = PresentationAction.MistakeType.LEFTHANDBEHINDBACK;
                            break;
                        }
                    }
                    sentMistakes = handsBehindBack;
                    break;
                case PresentationAction.MistakeType.RIGHTHANDBEHINDBACK:
                    handsBehindBackMistakes++;
                    for (i = 0; i < interruptionThreshold; i++)
                    {
                        if (handsBehindBack[i] == null)
                        {
                            handsBehindBack[i] = temp;
                            handsBehindBack[i].myMistake = PresentationAction.MistakeType.RIGHTHANDBEHINDBACK;
                            break;
                        }
                    }
                    sentMistakes = handsBehindBack;
                    break;
                case PresentationAction.MistakeType.LEGSCROSSED:
                    legsCrossedMistakes++;
                    for (i = 0; i < interruptionThreshold; i++)
                    {
                        if (legsCrossed[i] == null)
                        {
                            legsCrossed[i] = temp;
                            legsCrossed[i].myMistake = PresentationAction.MistakeType.LEGSCROSSED;
                            break;
                        }
                    }
                    sentMistakes = legsCrossed;
                    break;
                //case PresentationAction.MistakeType.BOTHHANDUNDERHIP:
                //    handsUnderHipsMistakes++;
                //    for (i = 0; i < interruptionThreshold; i++)
                //    {
                //        if (handsUnderHips[i] == null)
                //        {
                //            handsUnderHips[i] = temp;
                //            handsUnderHips[i].myMistake = PresentationAction.MistakeType.BOTHHANDUNDERHIP;
                //            break;
                //        }
                //    }
                //    sentMistakes = handsUnderHips;
                //    break;
                case PresentationAction.MistakeType.RIGHTHANDUNDERHIP:
                    handsUnderHipsMistakes++;
                    for (i = 0; i < interruptionThreshold; i++)
                    {
                        if (handsUnderHips[i] == null)
                        {
                            handsUnderHips[i] = temp;
                            handsUnderHips[i].myMistake = PresentationAction.MistakeType.RIGHTHANDUNDERHIP;
                            break;
                        }
                    }
                    sentMistakes = handsUnderHips;
                    break;
                case PresentationAction.MistakeType.LEFTHANDUNDERHIP:
                    handsUnderHipsMistakes++;
                    for (i = 0; i < interruptionThreshold; i++)
                    {
                        if (handsUnderHips[i] == null)
                        {
                            handsUnderHips[i] = temp;
                            handsUnderHips[i].myMistake = PresentationAction.MistakeType.LEFTHANDUNDERHIP;
                            break;
                        }
                    }
                    sentMistakes = handsUnderHips;
                    break;
                case PresentationAction.MistakeType.HUNCHBACK:
                    hunchPostureMistakes++;
                    for (i = 0; i < interruptionThreshold; i++)
                    {
                        if (hunchPosture[i] == null)
                        {
                            hunchPosture[i] = temp;
                            hunchPosture[i].myMistake = PresentationAction.MistakeType.HUNCHBACK;
                            break;
                        }
                    }
                    sentMistakes = hunchPosture;
                    break;
                case PresentationAction.MistakeType.RIGHTLEAN:
                case PresentationAction.MistakeType.LEFTLEAN:
                    leaningPostureMistakes++;
                    for (i = 0; i < interruptionThreshold; i++)
                    {
                        if (leaningPosture[i] == null)
                        {
                            leaningPosture[i] = temp;
                            leaningPosture[i].myMistake = PresentationAction.MistakeType.RIGHTLEAN;
                            break;
                        }
                    }
                    sentMistakes = leaningPosture;
                    break;
                case PresentationAction.MistakeType.DANCING:
                    periodicMovementsMistakes++;
                    for (i = 0; i < interruptionThreshold; i++)
                    {
                        if (periodicMovements[i] == null)
                        {
                            periodicMovements[i] = temp;
                            periodicMovements[i].myMistake = PresentationAction.MistakeType.DANCING;
                            break;
                        }
                    }
                    sentMistakes = periodicMovements;
                    break;
                case PresentationAction.MistakeType.HANDS_NOT_MOVING:
                    handsNotMovingMistakes++;
                    for (i = 0; i < interruptionThreshold; i++)
                    {
                        if (handsNotMoving[i] == null)
                        {
                            // System.Windows.Media.ImageSource im = parent.videoHandler.kinectImage.Source;  
                            handsNotMoving[i] = temp;
                            handsNotMoving[i].myMistake = PresentationAction.MistakeType.HANDS_NOT_MOVING;
                            // handsNotMoving[i].lastImage = im.CloneCurrentValue();

                            break;
                        }
                    }
                    sentMistakes = handsNotMoving;
                    break;
                case PresentationAction.MistakeType.HANDS_MOVING_MUCH:
                    handsMovingMuchMistakes++;
                    for (i = 0; i < interruptionThreshold; i++)
                    {
                        if (handsMovingMuch[i] == null)
                        {
                            handsMovingMuch[i] = temp;
                            handsMovingMuch[i].myMistake = PresentationAction.MistakeType.HANDS_MOVING_MUCH;

                            break;
                        }
                    }
                    sentMistakes = handsMovingMuch;
                    break;
                case PresentationAction.MistakeType.HIGH_VOLUME:
                    highVolumesMistakes++;
                    for (i = 0; i < interruptionThreshold; i++)
                    {
                        if (highVolumes[i] == null)
                        {
                            highVolumes[i] = temp;
                            highVolumes[i].myMistake = PresentationAction.MistakeType.HIGH_VOLUME;
                            break;
                        }
                    }
                    sentMistakes = highVolumes;
                    break;
                case PresentationAction.MistakeType.LOW_VOLUME:
                    lowVolumesMistakes++;
                    for (i = 0; i < interruptionThreshold; i++)
                    {
                        if (lowVolumes[i] == null)
                        {
                            lowVolumes[i] = temp;
                            lowVolumes[i].myMistake = PresentationAction.MistakeType.LOW_VOLUME;
                            break;
                        }
                    }
                    sentMistakes = lowVolumes;
                    break;
                case PresentationAction.MistakeType.LOW_MODULATION:
                    noModulationMistakes++;
                    for (i = 0; i < interruptionThreshold; i++)
                    {
                        if (noModulation[i] == null)
                        {
                            noModulation[i] = temp;
                            noModulation[i].myMistake = PresentationAction.MistakeType.LOW_MODULATION;
                            break;
                        }
                    }
                    sentMistakes = noModulation;
                    break;
                case PresentationAction.MistakeType.LONG_PAUSE:
                    longPausesMistakes++;
                    for (i = 0; i < interruptionThreshold; i++)
                    {
                        if (longPauses[i] == null)
                        {
                            longPauses[i] = temp;
                            longPauses[i].myMistake = PresentationAction.MistakeType.LONG_PAUSE;
                            break;
                        }
                    }
                    sentMistakes = longPauses;
                    break;
                case PresentationAction.MistakeType.LONG_TALK:
                    longTalksMistakes++;
                    for (i = 0; i < interruptionThreshold; i++)
                    {
                        if (longTalks[i] == null)
                        {
                            longTalks[i] = temp;
                            longTalks[i].myMistake = PresentationAction.MistakeType.LONG_TALK;
                            break;
                        }
                    }
                    sentMistakes = longTalks;
                    break;
                case PresentationAction.MistakeType.HMMMM:
                    hmmmsMistakes++;
                    for (i = 0; i < interruptionThreshold; i++)
                    {
                        if (hmmms[i] == null)
                        {
                            hmmms[i] = temp;
                            hmmms[i].myMistake = PresentationAction.MistakeType.HMMMM;
                            break;
                        }
                    }
                    sentMistakes = hmmms;
                    break;
            }
            if (i >= interruptionThreshold-1)
            {
                interruptionsList.Add(temp);
                doInterruptionThing();
            }
        }

        //This method makes a log of all repetitions, corrections and metarepetitions that were made. In addition, the standard deviation of the totals of these are calculated.
        public void makeLog()
        {
            saveAllArrays();

            int nrOfVolumeMistakes = highVolumesMistakes + lowVolumesMistakes;
            int nrOfCadenceMistakes = longPausesMistakes + longTalksMistakes + noModulationMistakes;
            int nrOfPostureMistakes = crossedArmsMistakes + handsUnderHipsMistakes + handsBehindBackMistakes + hunchPostureMistakes + leaningPostureMistakes + legsCrossedMistakes;
            int nrOfHandMovementMistakes = handsNotMovingMistakes + handsMovingMuchMistakes;
            int nrOfMistakes = nrOfVolumeMistakes + nrOfCadenceMistakes + nrOfPostureMistakes + nrOfHandMovementMistakes + hmmmsMistakes + periodicMovementsMistakes;

            int nrOfVolumeCorrections = highVolumesCorrections + lowVolumesCorrections;
            int nrOfCadenceCorrections = longPausesCorrections + longTalksCorrections + noModulationCorrections;
            int nrOfPostureCorrections = crossedArmsCorrections + handsUnderHipsCorrections + handsBehindBackCorrections + hunchPostureCorrections + leaningPostureCorrections + legsCrossedCorrections;
            int nrOfHandMovementCorrections = handsNotMovingCorrections + handsMovingMuchCorrections;
            int nrOfCorrections = nrOfVolumeCorrections + nrOfCadenceCorrections + nrOfPostureCorrections + nrOfHandMovementCorrections + hmmmsCorrections + periodicMovementsCorrections;

            int nrOfInterruptions = interruptionsList.Count;
            int volumeInterruptions = 0;
            int cadenceInterruptions = 0;
            int postureInterruptions = 0;
            int handMovementInterruptions = 0;
            int hmmmsInterruptions = 0;
            int periodicMovementsInterruptions = 0;

            foreach (PresentationAction pa in interruptionsList)
            {
                switch (pa.getMetaMistake())
                {
                    case PresentationAction.MetaType.VOLUME: volumeInterruptions++; break;
                    case PresentationAction.MetaType.CADENCE: cadenceInterruptions++; break;
                    case PresentationAction.MetaType.POSTURE: postureInterruptions++; break;
                    case PresentationAction.MetaType.HAND_MOVEMENT: handMovementInterruptions++; break;
                    case PresentationAction.MetaType.PHONETIC_PAUSE: hmmmsInterruptions++; break;
                    case PresentationAction.MetaType.BODY_MOVEMENT: periodicMovementsInterruptions++; break;
                }
            }

            int[] all = null;
            all = allToArray();
            int entry = getMostRepeatedMistake(all);
            mostRepeatedMistake = getMistakeTypeOfEntry(entry);

            int[] allCorrections = null;
            allCorrections = allCorrectionsToArray();
            int entryC = getMostRepeatedMistake(allCorrections);
            mostRepeatedCorrection = getMistakeTypeOfEntry(entryC);

            int temp = 0;
            if (nrOfVolumeMistakes > temp)
            {
                temp = nrOfVolumeMistakes;
                mostRepeatedMetaMistake = "Volume Mistakes";
            }           
            if (nrOfCadenceMistakes > temp)
            {
                temp = nrOfCadenceMistakes;
                mostRepeatedMetaMistake = "Cadence Mistakes";
            }
            if (nrOfPostureMistakes > temp)
            {
                temp = nrOfPostureMistakes;
                mostRepeatedMetaMistake = "Posture Mistakes";
            }
            if (nrOfHandMovementMistakes > temp)
            {
                temp = nrOfHandMovementMistakes;
                mostRepeatedMetaMistake = "Hand Movement Mistakes";
            }
            if (hmmmsMistakes > temp)
            {
                temp = hmmmsMistakes;
                mostRepeatedMetaMistake = "Phonetic Pause Mistakes";
            }
            if (periodicMovementsMistakes > temp)
            {
                temp = periodicMovementsMistakes;
                mostRepeatedMetaMistake = "Body Movement Mistakes";
            }

            int tempC = 0;
            if (nrOfVolumeCorrections > tempC)
            {
                tempC = nrOfVolumeCorrections;
                mostRepeatedMetaCorrection = "Volume Corrections";
            }
            if (nrOfCadenceCorrections > tempC)
            {
                tempC = nrOfCadenceCorrections;
                mostRepeatedMetaCorrection = "Cadence Corrections";
            }
            if (nrOfPostureCorrections > tempC)
            {
                tempC = nrOfPostureCorrections;
                mostRepeatedMetaCorrection = "Posture Corrections";
            }
            if (nrOfHandMovementCorrections > tempC)
            {
                tempC = nrOfHandMovementCorrections;
                mostRepeatedMetaCorrection = "Hand Movement Corrections";
            }
            if (hmmmsCorrections > tempC)
            {
                tempC = hmmmsCorrections;
                mostRepeatedMetaCorrection = "Phonetic Pause Corrections";
            }
            if (periodicMovementsCorrections > tempC)
            {
                tempC = periodicMovementsCorrections;
                mostRepeatedMetaCorrection = "Body Movement Corrections";
            }

            //each mistake has its own points when it is ended, therefore only need to request mistake points.
            //go through all lists, make sum and divide by nr of mistakes.
            double sum = 0;
            double average = 0;
            double sdSum = 0; //sum for standard deviation
            double sd = 0; //standard deviation
            foreach (PresentationAction a in crossedArmsList)
            {
                sum += a.getGravity();
            }
            foreach (PresentationAction a in handsUnderHipsList)
            {
                sum += a.getGravity();
            }
            foreach (PresentationAction a in handsBehindBackList)
            {
                sum += a.getGravity();
            }
            foreach (PresentationAction a in hunchPostureList)
            {
                sum += a.getGravity();
            }
            foreach (PresentationAction a in leaningPostureList)
            {
                sum += a.getGravity();
            }
            foreach (PresentationAction a in legsCrossedList)
            {
                sum += a.getGravity();
            }
            foreach (PresentationAction a in handsMovingMuchList)
            {
                sum += a.getGravity();
            }
            foreach (PresentationAction a in handsNotMovingList)
            {
                sum += a.getGravity();
            }
            foreach (PresentationAction a in highVolumesList)
            {
                sum += a.getGravity();
            }
            foreach (PresentationAction a in lowVolumesList)
            {
                sum += a.getGravity();
            }
            foreach (PresentationAction a in longPausesList)
            {
                sum += a.getGravity();
            }
            foreach (PresentationAction a in longTalksList)
            {
                sum += a.getGravity();
            }
            foreach (PresentationAction a in noModulationList)
            {
                sum += a.getGravity();
            }

            average = sum / nrOfMistakes;
            //go through all lists, making sdSum, which is (sdSum += Math.pow(pointsOfMistake- sum, 2)). Divide this sum by nr of mistakes, = sd.
            foreach (PresentationAction a in crossedArmsList)
            {
                sdSum += Math.Pow(a.getGravity() - average,2);
            }
            foreach (PresentationAction a in handsUnderHipsList)
            {
                sdSum += Math.Pow(a.getGravity() - average, 2);
            }
            foreach (PresentationAction a in handsBehindBackList)
            {
                sdSum += Math.Pow(a.getGravity() - average, 2);
            }
            foreach (PresentationAction a in hunchPostureList)
            {
                sdSum += Math.Pow(a.getGravity() - average, 2);
            }
            foreach (PresentationAction a in leaningPostureList)
            {
                sdSum += Math.Pow(a.getGravity() - average, 2);
            }
            foreach (PresentationAction a in legsCrossedList)
            {
                sdSum += Math.Pow(a.getGravity() - average, 2);
            }
            foreach (PresentationAction a in handsMovingMuchList)
            {
                sdSum += Math.Pow(a.getGravity() - average, 2);
            }
            foreach (PresentationAction a in handsNotMovingList)
            {
                sdSum += Math.Pow(a.getGravity() - average, 2);
            }
            foreach (PresentationAction a in highVolumesList)
            {
                sdSum += Math.Pow(a.getGravity() - average, 2);
            }
            foreach (PresentationAction a in lowVolumesList)
            {
                sdSum += Math.Pow(a.getGravity() - average, 2);
            }
            foreach (PresentationAction a in longPausesList)
            {
                sdSum += Math.Pow(a.getGravity() - average, 2);
            }
            foreach (PresentationAction a in longTalksList)
            {
                sdSum += Math.Pow(a.getGravity() - average, 2);
            }
            foreach (PresentationAction a in noModulationList)
            {
                sdSum += Math.Pow(a.getGravity() - average, 2);
            }

            sd = Math.Sqrt(sdSum / nrOfMistakes);

            DateTime endingTime = DateTime.Now;
            string start = parent.freestyleMode.startTimeOfFreestyle.TimeOfDay.ToString().Remove(8);
            string end = endingTime.TimeOfDay.ToString().Remove(8);
            int difference = (int) (endingTime.TimeOfDay.TotalSeconds - parent.freestyleMode.startTimeOfFreestyle.TimeOfDay.TotalSeconds);

            String[] report = new String[25];
            //meta mistakes
            report[0] = "Total number of mistakes: " + nrOfMistakes + ".| Total number of Corrections: " + nrOfCorrections + ".| Total number of Interruptions: " + nrOfInterruptions;
            report[1] = "Most repeated mistake is " + mostRepeatedMistake + " with a count of " + all[entry] + " mistakes.";
            report[1] = "Volume Mistakes: " + nrOfVolumeMistakes + ".| Volume Corrections: " + nrOfVolumeCorrections + ".| Volume interruptions: " + volumeInterruptions;
            report[2] = "Cadence Mistakes: " + nrOfCadenceMistakes + ".| Cadence Corrections: " + nrOfCadenceCorrections + ".| Cadence interruptions: " +cadenceInterruptions;
            report[3] = "Posture Mistakes: " + nrOfPostureMistakes + ".| Posture Corrections: " + nrOfPostureCorrections + ".| Posture interruptions: " + postureInterruptions;
            report[4] = "Hand Movement Mistakes: " + nrOfHandMovementMistakes + ".| Hand Movement Corrections: " + nrOfHandMovementCorrections + ".| Hand Movement interruptions: " + handMovementInterruptions;
            report[5] = "Phonetic Pause Mistakes: " + hmmmsMistakes + ".| Phonetic Pause Corrections: " + hmmmsCorrections + ".| Phonetic Pause interruptions: " + hmmmsInterruptions;
            report[6] = "Body Movement Mistakes: " + periodicMovementsMistakes + ".| Body Movement Corrections: " + periodicMovementsCorrections + ".| Body Movement interruptions: " + periodicMovementsInterruptions;
            report[7] = "Average Mistake Points: " + average + ".| Standard Deviation of Average Mistake Points: " + sd;
            //individual mistakes
            report[8] = "High volume mistakes: " + highVolumesMistakes + ".| High volume Corrections: " + highVolumesCorrections;
            report[9] = "Low Volume mistakes: " + lowVolumesMistakes + ".| Low volume corrections: " + lowVolumesCorrections;
            report[10] = "Long Pause mistakes: " + longPausesMistakes + ".| Long Pause corrections: " + longPausesCorrections;
            report[11] = "Long talking mistakes: " + longTalksMistakes + ".| Long Talking corrections: " + longTalksCorrections;
            report[12] = "No Modulation mistakes: " + noModulationMistakes + ".| No Modulation corrections: " + noModulationCorrections;
            report[13] = "Crossed arms mistakes: " + crossedArmsMistakes + ".| Crossed arms corrections: " + crossedArmsCorrections;
            report[14] = "Hands below hips mistakes: " + handsUnderHipsMistakes + ".| Hands under hips corrections: " + handsUnderHipsCorrections;
            report[15] = "Hands behind back mistakes: " + handsBehindBackMistakes + ".| Hands behind back corrections: " + handsBehindBackCorrections;
            report[16] = "Hunched posture mistakes: " + hunchPostureMistakes + ".| Hunched posture corrections: " + hunchPostureCorrections;
            report[17] = "Leaning posture mistakes: " + leaningPostureMistakes + ".| Leaning posture corrections: " + leaningPostureCorrections;
            report[18] = "Legs Crossed mistakes: " + legsCrossedMistakes + ".| Legs Crossed corrections: " + legsCrossedCorrections;
            report[19] = "Not moving hands mistakes: " + handsNotMovingMistakes + ".| Not moving hands corrections: " + handsNotMovingCorrections;
            report[20] = "Moving hands too much mistakes: " + handsMovingMuchMistakes + ".| Moving hands too much corrections: " + handsMovingMuchCorrections;
            report[21] = "Phonetic pause mistakes: " + hmmmsMistakes + ".| Phonetic pause corrections: " + hmmmsCorrections;
            report[22] = "Body movement mistakes: " + periodicMovementsMistakes + ".| Body movement corrections: " + periodicMovementsCorrections;
            report[23] = "Starting time: " + start + " | Ending Time: " + end + " | Total Time: " + difference;
            if (userName != null)
            {
                report[24] = userName;
            }
            else
            {
                report[24] = "default";
            }
            

            string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Presentation Trainer Testing";
            string x = DateTime.Now.TimeOfDay.ToString();
            string tempString = x.Replace(":", "-").Remove(8);

            string fileName = @"\" + DateTime.Now.Date.ToLongDateString() + " " + tempString + ".txt";

            System.IO.File.WriteAllLines(mydocpath + fileName, report);
        }
        
        //this method will save all arrays to their respective lists. Needed for logging.
        private void saveAllArrays()
        {
            // go through each array, if i'th entry != null, then add it to its respective list.
            for (int i = 0; i < interruptionThreshold; i++)
            {
                if (crossedArms[i] != null)
                {
                    crossedArmsList.Add(crossedArms[i]);
                }
                if (legsCrossed[i] != null)
                {
                    legsCrossedList.Add(legsCrossed[i]);
                }
                if (handsBehindBack[i] != null)
                {
                    handsBehindBackList.Add(handsBehindBack[i]);
                }
                if (handsUnderHips[i] != null)
                {
                    handsUnderHipsList.Add(handsUnderHips[i]);
                }
                if (hunchPosture[i] != null)
                {
                    hunchPostureList.Add(hunchPosture[i]);
                }
                if (leaningPosture[i] != null)
                {
                    leaningPostureList.Add(leaningPosture[i]);
                }
                if (periodicMovements[i] != null)
                {
                    periodicMovementsList.Add(periodicMovements[i]);
                }
                if (handsNotMoving[i] != null)
                {
                    handsNotMovingList.Add(handsNotMoving[i]);
                }
                if (handsMovingMuch[i] != null)
                {
                    handsMovingMuchList.Add(handsMovingMuch[i]);
                }
                if (highVolumes[i] != null)
                {
                    highVolumesList.Add(highVolumes[i]);
                }
                if (lowVolumes[i] != null)
                {
                    lowVolumesList.Add(lowVolumes[i]);
                }
                if (noModulation[i] != null)
                {
                    noModulationList.Add(noModulation[i]);
                }
                if (longPauses[i] != null)
                {
                    longPausesList.Add(longPauses[i]);
                }
                if (longTalks[i] != null)
                {
                    longTalksList.Add(longTalks[i]);
                }
                if (hmmms[i] != null)
                {
                    hmmmsList.Add(hmmms[i]);
                }                
            }
        }

        
        //Puts all mistake ints, in an array
        private int[] allToArray()
        {
            int[] all = new int[15];
            all[0] = highVolumesMistakes;
            all[1] = lowVolumesMistakes;
            all[2] = longPausesMistakes;
            all[3] = longTalksMistakes;
            all[4] = noModulationMistakes;
            all[5] = crossedArmsMistakes;
            all[6] = handsUnderHipsMistakes;
            all[7] = handsBehindBackMistakes;
            all[8] = hunchPostureMistakes;
            all[9] = leaningPostureMistakes;
            all[10] = legsCrossedMistakes;
            all[11] = handsNotMovingMistakes;
            all[12] = handsMovingMuchMistakes;
            all[13] = hmmmsMistakes;
            all[14] = periodicMovementsMistakes;

            return all;
        }

        //Puts all correction ints, in an array
        private int[] allCorrectionsToArray()
        {
            int[] all = new int[15];
            all[0] = highVolumesCorrections;
            all[1] = lowVolumesCorrections;
            all[2] = longPausesCorrections;
            all[3] = longTalksCorrections;
            all[4] = noModulationCorrections;
            all[5] = crossedArmsCorrections;
            all[6] = handsUnderHipsCorrections;
            all[7] = handsBehindBackCorrections;
            all[8] = hunchPostureCorrections;
            all[9] = leaningPostureCorrections;
            all[10] = legsCrossedCorrections;
            all[11] = handsNotMovingCorrections;
            all[12] = handsMovingMuchCorrections;
            all[13] = hmmmsCorrections;
            all[14] = periodicMovementsCorrections;

            return all;
        }

        //Reverts array entry to its category.
        private string getMistakeTypeOfEntry(int entry)
        {
            switch (entry)
            {
                case 0: return "High volume";
                case 1: return "Low volume";
                case 2: return "Long Pause";
                case 3: return "Long Talk";
                case 4: return "No Modulation";
                case 5: return "Crossed Arms";
                case 6: return "Hands Below Hips";
                case 7: return "Hands Behind Back";
                case 8: return "Hunched Posture";
                case 9: return "Leaning Posture";
                case 10: return "Crossed Legs";
                case 11: return "Hands Not Moving";
                case 12: return "Hands Moving Too Much";
                case 13: return "Phonetic Pauses";
                case 14: return "Body Movement";
                default: return "Posture Mistake";
            }
        }
         
        //returns entry of biggest number in the array
        private int getMostRepeatedMistake(int[] all)
        {
            int biggest = 0;
            int entry = 0;
            for (int i = 0; i < all.Length; i++)
            {
                if (all[i] > biggest)
                {
                    biggest = all[i];
                    entry = i;
                }
            }
            return entry;
        }

        //check for folder with username, if not exists, create one.
        //Create file with session number and date
        //Log all mistakes with all of their attributes
        public void makeUserProfileLog()
        {
            if (userName == null)
            {
                return;
            }
            //Checking or creating a directory
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Presentation Trainer Testing";
            string directoryString = System.IO.Path.Combine(path, userName);

            if (!System.IO.Directory.Exists(directoryString))
            {
                System.IO.Directory.CreateDirectory(directoryString);
            }

            //important things first to log: repetitions and corrections for each list.
            string[] report = new string[33];
            report[0] = userName;
            report[1] = "-start of list entries-";
            report[2] = "hvm" + highVolumesMistakes;
            report[3] = "lvm" + lowVolumesMistakes;
            report[4] = "lpm" + longPausesMistakes;
            report[5] = "ltm" + longTalksMistakes;
            report[6] = "nmm" + noModulationMistakes;
            report[7] = "cam" + crossedArmsMistakes;
            report[8] = "hum" + handsUnderHipsMistakes;
            report[9] = "hbm" + handsBehindBackMistakes;
            report[10] = "hpm" + hunchPostureMistakes;
            report[11] = "plm" + leaningPostureMistakes;
            report[12] = "lcm" + legsCrossedMistakes;
            report[13] = "hnm" + handsNotMovingMistakes;
            report[14] = "htm" + handsMovingMuchMistakes;
            report[15] = "hmm" + hmmmsMistakes;
            report[16] = "pmm" + periodicMovementsMistakes;

            report[17] = "hvc" + highVolumesCorrections;
            report[18] = "lvc" + lowVolumesCorrections;
            report[19] = "lpc" + longPausesCorrections;
            report[20] = "ltc" + longTalksCorrections;
            report[21] = "nmc" + noModulationCorrections;
            report[22] = "cac" + crossedArmsCorrections;
            report[23] = "huc" + handsUnderHipsCorrections;
            report[24] = "hbc" + handsBehindBackCorrections;
            report[25] = "hpc" + hunchPostureCorrections;
            report[26] = "plc" + leaningPostureCorrections;
            report[27] = "lcc" + legsCrossedCorrections;
            report[28] = "hnc" + handsNotMovingCorrections;
            report[29] = "htc" + handsMovingMuchCorrections;
            report[30] = "hmc" + hmmmsCorrections;
            report[31] = "pmc" + periodicMovementsCorrections;
            report[32] = "-end of list entries-";

            //All separate mistakeLists 
            //interruptionsList
            //feedbackedMistakeList           

            string x = DateTime.Now.TimeOfDay.ToString();
            string temp = x.Replace(":", "-").Remove(8);

            string fileName = @"\" + session + " " + DateTime.Now.Date.ToLongDateString() + " " + temp + ".txt";

            System.IO.File.WriteAllLines(directoryString + fileName, report);

        }

        //This method finds the user logs in a specified folder.
        public void findUserLogs()
        {
            if (userName == null)
            {
                return;
            }

            //Checking directory
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Presentation Trainer Testing";
            string directoryString = System.IO.Path.Combine(path, userName);

            if (!System.IO.Directory.Exists(directoryString))
            {
                //System.Windows.MessageBox.Show("no such directory");
                return;
            }

            string[] allFiles = System.IO.Directory.GetFiles(directoryString);

            if (allFiles == null)
            {
                return;
            }

            for (int i = allFiles.Length -1; i >=0; i--)
            {       
                string filePath = allFiles[i];
                //System.Windows.MessageBox.Show("Succeeded! " + filePath);
                readUserLog(filePath);
            }           

        }

        //this method reads the user logs found with the findUserLogs() method
        private void readUserLog(string filePath)
        {
            //session number is first character of file name. 
            // if (file session >= session) session = file session +1;
            string fileName = System.IO.Path.GetFileName(filePath);
            //System.Windows.MessageBox.Show("Name: " + fileName);
            char[] charFileName = fileName.ToCharArray();
            int tempSession = (int) char.GetNumericValue(charFileName[0]);
            if (tempSession >= session) 
                session = tempSession + 1;

            double modifier = 1;
            modifier = 1.0/(Math.Abs(session - tempSession) +1);
            //System.Windows.MessageBox.Show("modifier: " + modifier);
            //remember: string starts at index 0
            string[] allLines = System.IO.File.ReadAllLines(filePath);
            foreach (string line in allLines)
            {
                if (line.StartsWith("-") || line.StartsWith(userName))
                {
                    //do nothing
                }
                else if (line.StartsWith("hvm"))
                {
                    string sub = line.Substring(3);
                    int tempMistakesNumber = Convert.ToInt32(sub);
                    previoushighVolumesMistakes += modifier * tempMistakesNumber;
                    //System.Windows.MessageBox.Show("Succeeded with reading! " + tempMistakesNumber);
                }
                else if (line.StartsWith("lvm"))
                {
                    string sub = line.Substring(3);
                    int tempMistakesNumber = Convert.ToInt32(sub);
                    previouslowVolumesMistakes += modifier * tempMistakesNumber;
                }
                else if (line.StartsWith("lpm"))
                {
                    string sub = line.Substring(3);
                    int tempMistakesNumber = Convert.ToInt32(sub);
                    previouslongPausesMistakes += modifier * tempMistakesNumber;
                }
                else if (line.StartsWith("ltm"))
                {
                    string sub = line.Substring(3);
                    int tempMistakesNumber = Convert.ToInt32(sub);
                    previouslongTalksMistakes += modifier * tempMistakesNumber;
                }
                else if (line.StartsWith("nmm"))
                {
                    string sub = line.Substring(3);
                    int tempMistakesNumber = Convert.ToInt32(sub);
                    previousnoModulationMistakes += modifier * tempMistakesNumber;
                }
                else if (line.StartsWith("cam"))
                {
                    string sub = line.Substring(3);
                    int tempMistakesNumber = Convert.ToInt32(sub);
                    previouscrossedArmsMistakes += modifier * tempMistakesNumber;
                }
                else if (line.StartsWith("hum"))
                {
                    string sub = line.Substring(3);
                    int tempMistakesNumber = Convert.ToInt32(sub);
                    previoushandsUnderHipsMistakes += modifier * tempMistakesNumber;
                }
                else if (line.StartsWith("hbm"))
                {
                    string sub = line.Substring(3);
                    int tempMistakesNumber = Convert.ToInt32(sub);
                    previoushandsBehindBackMistakes += modifier * tempMistakesNumber;
                }
                else if (line.StartsWith("hpm"))
                {
                    string sub = line.Substring(3);
                    int tempMistakesNumber = Convert.ToInt32(sub);
                    previoushunchPostureMistakes += modifier * tempMistakesNumber;
                }
                else if (line.StartsWith("lpm"))
                {
                    string sub = line.Substring(3);
                    int tempMistakesNumber = Convert.ToInt32(sub);
                    previousleaningPostureMistakes += modifier * tempMistakesNumber;
                }
                else if (line.StartsWith("lcm"))
                {
                    string sub = line.Substring(3);
                    int tempMistakesNumber = Convert.ToInt32(sub);
                    previouslegsCrossedMistakes += modifier * tempMistakesNumber;
                }
                else if (line.StartsWith("hnm"))
                {
                    string sub = line.Substring(3);
                    int tempMistakesNumber = Convert.ToInt32(sub);
                    previoushandsNotMovingMistakes += modifier * tempMistakesNumber;
                }
                else if (line.StartsWith("htm"))
                {
                    string sub = line.Substring(3);
                    int tempMistakesNumber = Convert.ToInt32(sub);
                    previoushandsMovingMuchMistakes += modifier * tempMistakesNumber;
                }
                else if (line.StartsWith("hmm"))
                {
                    string sub = line.Substring(3);
                    int tempMistakesNumber = Convert.ToInt32(sub);
                    previoushmmmsMistakes += modifier * tempMistakesNumber;
                }
                else if (line.StartsWith("pmm"))
                {
                    string sub = line.Substring(3);
                    int tempMistakesNumber = Convert.ToInt32(sub);
                    previousperiodicMovementsMistakes += modifier * tempMistakesNumber;
                }
                else if (line.StartsWith("hvc"))
                {
                    string sub = line.Substring(3);
                    int tempMistakesNumber = Convert.ToInt32(sub);
                    previoushighVolumesCorrections += modifier * tempMistakesNumber;
                    //System.Windows.MessageBox.Show("Succeeded with reading! " + tempMistakesNumber);
                }
                else if (line.StartsWith("lvc"))
                {
                    string sub = line.Substring(3);
                    int tempMistakesNumber = Convert.ToInt32(sub);
                    previouslowVolumesCorrections += modifier * tempMistakesNumber;
                }
                else if (line.StartsWith("lpc"))
                {
                    string sub = line.Substring(3);
                    int tempMistakesNumber = Convert.ToInt32(sub);
                    previouslongPausesCorrections += modifier * tempMistakesNumber;
                }
                else if (line.StartsWith("ltc"))
                {
                    string sub = line.Substring(3);
                    int tempMistakesNumber = Convert.ToInt32(sub);
                    previouslongTalksCorrections += modifier * tempMistakesNumber;
                }
                else if (line.StartsWith("nmc"))
                {
                    string sub = line.Substring(3);
                    int tempMistakesNumber = Convert.ToInt32(sub);
                    previousnoModulationCorrections += modifier * tempMistakesNumber;
                }
                else if (line.StartsWith("cac"))
                {
                    string sub = line.Substring(3);
                    int tempMistakesNumber = Convert.ToInt32(sub);
                    previouscrossedArmsCorrections += modifier * tempMistakesNumber;
                }
                else if (line.StartsWith("huc"))
                {
                    string sub = line.Substring(3);
                    int tempMistakesNumber = Convert.ToInt32(sub);
                    previoushandsUnderHipsCorrections += modifier * tempMistakesNumber;
                }
                else if (line.StartsWith("hbc"))
                {
                    string sub = line.Substring(3);
                    int tempMistakesNumber = Convert.ToInt32(sub);
                    previoushandsBehindBackCorrections += modifier * tempMistakesNumber;
                }
                else if (line.StartsWith("hpc"))
                {
                    string sub = line.Substring(3);
                    int tempMistakesNumber = Convert.ToInt32(sub);
                    previoushunchPostureCorrections += modifier * tempMistakesNumber;
                }
                else if (line.StartsWith("lpc"))
                {
                    string sub = line.Substring(3);
                    int tempMistakesNumber = Convert.ToInt32(sub);
                    previousleaningPostureCorrections += modifier * tempMistakesNumber;
                }
                else if (line.StartsWith("lcc"))
                {
                    string sub = line.Substring(3);
                    int tempMistakesNumber = Convert.ToInt32(sub);
                    previouslegsCrossedCorrections += modifier * tempMistakesNumber;
                }
                else if (line.StartsWith("hnc"))
                {
                    string sub = line.Substring(3);
                    int tempMistakesNumber = Convert.ToInt32(sub);
                    previoushandsNotMovingCorrections += modifier * tempMistakesNumber;
                }
                else if (line.StartsWith("htc"))
                {
                    string sub = line.Substring(3);
                    int tempMistakesNumber = Convert.ToInt32(sub);
                    previoushandsMovingMuchCorrections += modifier * tempMistakesNumber;
                }
                else if (line.StartsWith("hmc"))
                {
                    string sub = line.Substring(3);
                    int tempMistakesNumber = Convert.ToInt32(sub);
                    previoushmmmsCorrections += modifier * tempMistakesNumber;
                }
                else if (line.StartsWith("pmc"))
                {
                    string sub = line.Substring(3);
                    int tempMistakesNumber = Convert.ToInt32(sub);
                    previousperiodicMovementsCorrections += modifier * tempMistakesNumber;
                }
            }            
            
        }

        //this method resets everything of this class. Used for going back to mainmenu
        public void resetAll()
        {
            feedbackedMistakes = new ArrayList();
            mistakes = new ArrayList();
            interruptionsList = new ArrayList();

            crossedArms = new PresentationAction[interruptionThreshold];
            handsUnderHips = new PresentationAction[interruptionThreshold];
            handsBehindBack = new PresentationAction[interruptionThreshold];
            hunchPosture = new PresentationAction[interruptionThreshold];
            leaningPosture = new PresentationAction[interruptionThreshold];
            highVolumes = new PresentationAction[interruptionThreshold];
            lowVolumes = new PresentationAction[interruptionThreshold];
            longPauses = new PresentationAction[interruptionThreshold];
            longTalks = new PresentationAction[interruptionThreshold];
            periodicMovements = new PresentationAction[interruptionThreshold];
            hmmms = new PresentationAction[interruptionThreshold];
            legsCrossed = new PresentationAction[interruptionThreshold];
            handsNotMoving = new PresentationAction[interruptionThreshold];
            handsMovingMuch = new PresentationAction[interruptionThreshold];
            noModulation = new PresentationAction[interruptionThreshold];

            crossedArmsList = new ArrayList();
            handsUnderHipsList = new ArrayList();
            handsBehindBackList = new ArrayList();
            hunchPostureList = new ArrayList();
            leaningPostureList = new ArrayList();
            highVolumesList = new ArrayList();
            lowVolumesList = new ArrayList();
            longPausesList = new ArrayList();
            longTalksList = new ArrayList();
            periodicMovementsList = new ArrayList();
            hmmmsList = new ArrayList();
            legsCrossedList = new ArrayList();
            handsNotMovingList = new ArrayList();
            handsMovingMuchList = new ArrayList();
            noModulationList = new ArrayList();

            previousAction = new PresentationAction();
            previousAction.myMistake = PresentationAction.MistakeType.NOMISTAKE;

            crossedArmsMistakes = 0;
            handsUnderHipsMistakes = 0;
            handsBehindBackMistakes = 0;
            hunchPostureMistakes = 0;
            leaningPostureMistakes = 0;
            highVolumesMistakes = 0;
            lowVolumesMistakes = 0;
            longPausesMistakes = 0;
            longTalksMistakes = 0;
            periodicMovementsMistakes = 0;
            hmmmsMistakes = 0;
            legsCrossedMistakes = 0;
            handsNotMovingMistakes = 0;
            handsMovingMuchMistakes = 0;
            noModulationMistakes = 0;

            crossedArmsCorrections = 0;
            handsUnderHipsCorrections = 0;
            handsBehindBackCorrections = 0;
            hunchPostureCorrections = 0;
            leaningPostureCorrections = 0;
            highVolumesCorrections = 0;
            lowVolumesCorrections = 0;
            longPausesCorrections = 0;
            longTalksCorrections = 0;
            periodicMovementsCorrections = 0;
            hmmmsCorrections = 0;
            legsCrossedCorrections = 0;
            handsNotMovingCorrections = 0;
            handsMovingMuchCorrections = 0;
            noModulationCorrections = 0;

            previouscrossedArmsMistakes = 0;
            previoushandsUnderHipsMistakes = 0;
            previoushandsBehindBackMistakes = 0;
            previoushunchPostureMistakes = 0;
            previousleaningPostureMistakes = 0;
            previoushighVolumesMistakes = 0;
            previouslowVolumesMistakes = 0;
            previouslongPausesMistakes = 0;
            previouslongTalksMistakes = 0;
            previousperiodicMovementsMistakes = 0;
            previoushmmmsMistakes = 0;
            previouslegsCrossedMistakes = 0;
            previoushandsNotMovingMistakes = 0;
            previoushandsMovingMuchMistakes = 0;
            previousnoModulationMistakes = 0;

            previouscrossedArmsCorrections = 0;
            previoushandsUnderHipsCorrections = 0;
            previoushandsBehindBackCorrections = 0;
            previoushunchPostureCorrections = 0;
            previousleaningPostureCorrections = 0;
            previoushighVolumesCorrections = 0;
            previouslowVolumesCorrections = 0;
            previouslongPausesCorrections = 0;
            previouslongTalksCorrections = 0;
            previousperiodicMovementsCorrections = 0;
            previoushmmmsCorrections = 0;
            previouslegsCrossedCorrections = 0;
            previoushandsNotMovingCorrections = 0;
            previoushandsMovingMuchCorrections = 0;
            previousnoModulationCorrections = 0;

            userName = null;
            session = 1;


            myJudgementMaker = new JudgementMaker(parent);
            noInterrupt = true;
        }

        //Makes the log file with the timestamps for all mistakes
        public void makeAllMistakeLogFiles()
        {
            string[] report = new string[16];
            if (userName != null)
            {
                report[0] = userName;
            }
            else
            {
                report[0] = "default";
            }
            
            report[0] += " | session started: " + (int) parent.freestyleMode.startTimeOfFreestyle.TimeOfDay.TotalSeconds;

            ArrayList easyAccessList = null;

            easyAccessList = highVolumesList;
            report[1] = "high volume mistakes: " + easyAccessList.Count + "| Timestamps: ";
            for (int i = 0; i < easyAccessList.Count; i++)
            {
                PresentationAction pa = (PresentationAction)easyAccessList[i];
                int ts = (int)(pa.timeStarted / 1000);
                int tf = (int)(pa.timeFinished / 1000); 
                report[1] += ts + " - " + tf + " | ";
            }

            easyAccessList = lowVolumesList;
            report[2] = "low volume mistakes: " + easyAccessList.Count + "| Timestamps: ";
            for (int i = 0; i < easyAccessList.Count; i++)
            {
                PresentationAction pa = (PresentationAction)easyAccessList[i];
                int ts = (int) (pa.timeStarted / 1000);
                int tf = (int) (pa.timeFinished / 1000);                
                report[2] += ts + " - " + tf + " | ";
            }

            easyAccessList = longPausesList;
            report[3] = "long pauses mistakes: " + easyAccessList.Count + "| Timestamps: ";
            for (int i = 0; i < easyAccessList.Count; i++)
            {
                PresentationAction pa = (PresentationAction)easyAccessList[i];
                int ts = (int)(pa.timeStarted / 1000);
                int tf = (int)(pa.timeFinished / 1000); 
                report[3] += ts + " - " + tf + " | ";
            }

            easyAccessList = longTalksList;
            report[4] = "long talks mistakes: " + easyAccessList.Count + "| Timestamps: ";
            for (int i = 0; i < easyAccessList.Count; i++)
            {
                PresentationAction pa = (PresentationAction)easyAccessList[i];
                int ts = (int)(pa.timeStarted / 1000);
                int tf = (int)(pa.timeFinished / 1000); 
                report[4] += ts + " - " + tf + " | ";
            }

            easyAccessList = noModulationList;
            report[5] = "no modulation mistakes: " + easyAccessList.Count + "| Timestamps: ";
            for (int i = 0; i < easyAccessList.Count; i++)
            {
                PresentationAction pa = (PresentationAction)easyAccessList[i];
                int ts = (int)(pa.timeStarted / 1000);
                int tf = (int)(pa.timeFinished / 1000); 
                report[5] += ts + " - " + tf + " | ";
            }

            easyAccessList = hmmmsList;
            report[6] = "phonetic pause mistakes: " + easyAccessList.Count + "| Timestamps: ";
            for (int i = 0; i < easyAccessList.Count; i++)
            {
                PresentationAction pa = (PresentationAction)easyAccessList[i];
                int ts = (int)(pa.timeStarted / 1000);
                int tf = (int)(pa.timeFinished / 1000); 
                report[6] += ts + " - " + tf + " | ";
            }

            easyAccessList = periodicMovementsList;
            report[7] = "body movement mistakes: " + easyAccessList.Count + "| Timestamps: ";
            for (int i = 0; i < easyAccessList.Count; i++)
            {
                PresentationAction pa = (PresentationAction)easyAccessList[i];
                int ts = (int)(pa.timeStarted / 1000);
                int tf = (int)(pa.timeFinished / 1000); 
                report[7] += ts + " - " + tf + " | ";
            }

            easyAccessList = handsMovingMuchList;
            report[8] = "hands moving much mistakes: " + easyAccessList.Count + "| Timestamps: ";
            for (int i = 0; i < easyAccessList.Count; i++)
            {
                PresentationAction pa = (PresentationAction)easyAccessList[i];
                int ts = (int)(pa.timeStarted / 1000);
                int tf = (int)(pa.timeFinished / 1000); 
                report[8] += ts + " - " + tf + " | ";
            }

            easyAccessList = handsNotMovingList;
            report[9] = "hands not moving mistakes: " + easyAccessList.Count + "| Timestamps: ";
            for (int i = 0; i < easyAccessList.Count; i++)
            {
                PresentationAction pa = (PresentationAction)easyAccessList[i];
                int ts = (int)(pa.timeStarted / 1000);
                int tf = (int)(pa.timeFinished / 1000); 
                report[9] += ts + " - " + tf + " | ";
            }

            easyAccessList = crossedArmsList;
            report[10] = "crossed arms mistakes: " + easyAccessList.Count + "| Timestamps: ";
            for (int i = 0; i < easyAccessList.Count; i++)
            {
                PresentationAction pa = (PresentationAction)easyAccessList[i];
                int ts = (int)(pa.timeStarted / 1000);
                int tf = (int)(pa.timeFinished / 1000); 
                report[10] += ts + " - " + tf + " | ";
            } 
            
            easyAccessList = handsBehindBackList;
            report[11] = "hands behind back mistakes: " + easyAccessList.Count + "| Timestamps: ";
            for (int i = 0; i < easyAccessList.Count; i++)
            {
                PresentationAction pa = (PresentationAction)easyAccessList[i];
                int ts = (int)(pa.timeStarted / 1000);
                int tf = (int)(pa.timeFinished / 1000); 
                report[11] += ts + " - " + tf + " | ";
            } 
            
            easyAccessList = handsUnderHipsList;
            report[12] = "hands under hips mistakes: " + easyAccessList.Count + "| Timestamps: ";
            for (int i = 0; i < easyAccessList.Count; i++)
            {
                PresentationAction pa = (PresentationAction)easyAccessList[i];
                int ts = (int)(pa.timeStarted / 1000);
                int tf = (int)(pa.timeFinished / 1000); 
                report[12] += ts + " - " + tf + " | ";
            }

            easyAccessList = hunchPostureList;
            report[13] = "hunch posture mistakes: " + easyAccessList.Count + "| Timestamps: ";
            for (int i = 0; i < easyAccessList.Count; i++)
            {
                PresentationAction pa = (PresentationAction)easyAccessList[i];
                int ts = (int)(pa.timeStarted / 1000);
                int tf = (int)(pa.timeFinished / 1000); 
                report[13] += ts + " - " + tf + " | ";
            }

            easyAccessList = leaningPostureList;
            report[14] = "leaning posture mistakes: " + easyAccessList.Count + "| Timestamps: ";
            for (int i = 0; i < easyAccessList.Count; i++)
            {
                PresentationAction pa = (PresentationAction)easyAccessList[i];
                int ts = (int)(pa.timeStarted / 1000);
                int tf = (int)(pa.timeFinished / 1000); 
                report[14] += ts + " - " + tf + " | ";
            }

            easyAccessList = legsCrossedList;
            report[15] = "legs crossed mistakes: " + easyAccessList.Count + "| Timestamps: ";
            for (int i = 0; i < easyAccessList.Count; i++)
            {
                PresentationAction pa = (PresentationAction)easyAccessList[i];
                int ts = (int)(pa.timeStarted / 1000);
                int tf = (int)(pa.timeFinished / 1000); 
                report[15] += ts + " - " + tf + " | ";
            }

            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Presentation Trainer Testing\Timestamp Logs";
            

            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }

            string x = DateTime.Now.TimeOfDay.ToString();
            string temp = x.Replace(":", "-").Remove(8);

            string fileName = @"\" + DateTime.Now.Date.ToLongDateString() + " " + temp + ".txt";

            System.IO.File.WriteAllLines(path + fileName, report);

        }      
        
    }

        #endregion

    
}
