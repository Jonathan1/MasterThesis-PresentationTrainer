For this program to work, you need the Kinect V2.0 connected through USB 3.0.
In order for the Kinect and the program to work, you need to have the Kinect sdk, which is provided here: https://www.microsoft.com/en-us/kinectforwindows/develop/
For viewing and editing the code you need to open it in visual studio, which can be found here: https://www.visualstudio.com/en-us/downloads/download-visual-studio-vs.aspx

The program can be opened easily in visual studio by double clicking on the visual studio solution in the PresentationTrainerNET folder.

Structure and overview of the program:
--------------------------------------
Mainwindow-----> this class is the startup class and shows the background of the window. Through buttons from the main menu, the FreestyleMode can be started. 
MainMenu-----> this class shows the main menu, which is shown at the beginning. 
FreestyleMode-----> this class is called by the main menu and handles background processes and core things for the freestyle mode.
RulesAnalyzerFIFO-----> with a few modifications this class can be used. It feedbacks the first mistake it has on the list and is able to interrupt for big or many mistakes.
RulesAnalyzerImproved-----> this classed is used by default. It feedbacks mistakes that have the highest number of points and is able to track repetitions, corrections and meta repetitions for the corresponding mistake. In addition these mistakes are logged at the end of the session after pausing and quitting.
JudgementMaker-----> decides if a certain event is a mistake or not. This class calls the Handler and Pre-analysis classes.
Handler and Pre-analysis classes-----> these classes handle information streams and package them in usable formats for the Judgement Maker.
PresentationAction-----> this is the representation for an action a participant takes. For example a voice mistake is represented by an object of this class.
IndividualClasses-----> these classes are for tracking specific things like handmovement and dancing movements.
InterruptFeedback-----> is the class used for the interface when interrupting
PauseControl-----> is the class used for the interface when pausing
