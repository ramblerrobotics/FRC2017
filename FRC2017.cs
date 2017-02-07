using System;
using System.Collections.Generic;
using System.Linq;
using WPILib;
using WPILib.SmartDashboard;

namespace FRC2017
{
    /// <summary>
    /// The VM is configured to automatically run this class, and to call the
    /// functions corresponding to each mode, as described in the IterativeRobot
    /// documentation. 
    /// </summary>
    public class FRC2017 : IterativeRobot
    {
        const string defaultAuto = "Default";
        const string customAuto = "My Auto";
        string autoSelected;
        SendableChooser chooser;
        bool elapsed;
        RobotDrive drive;
        Joystick stick;
        System.Timers.Timer time;
        VictorSP climber;
        /// <summary>
        /// This function is run when the robot is first started up and should be
        /// used for any initialization code.
        /// </summary>
        public override void RobotInit()
        {
            chooser = new SendableChooser();
            chooser.AddDefault("Default Auto", defaultAuto);
            chooser.AddObject("My Auto", customAuto);
            SmartDashboard.PutData("Chooser", chooser);
            //start cameras?
            CameraServer.Instance.StartAutomaticCapture(0);
            CameraServer.Instance.StartAutomaticCapture(1);
            //create joystick and robotdrive objects for joystick input and motor control
            stick = new Joystick(0);
            drive = new RobotDrive(0, 1, 2, 3);
            climber = new VictorSP(4);
        }

        // This autonomous (along with the sendable chooser above) shows how to select between
        // different autonomous modes using the dashboard. The senable chooser code works with
        // the Java SmartDashboard. If you prefer the LabVIEW Dashboard, remove all the chooser
        // code an uncomment the GetString code to get the uto name from the text box below
        // the gyro.
        //You can add additional auto modes by adding additional comparisons to the switch
        // structure below with additional strings. If using the SendableChooser
        // be sure to add them to the chooser code above as well.
        public override void AutonomousInit()
        {
            autoSelected = (string)chooser.GetSelected();
            //autoSelected = SmartDashboard.GetString("Auto Selector", defaultAuto);
            Console.WriteLine("Auto selected: " + autoSelected);
            time = new System.Timers.Timer(500);
            time.AutoReset = false;
            elapsed = false;
            time.Elapsed += TimeAlert;

        }
        private void TimeAlert(object source, System.Timers.ElapsedEventArgs e)
        {
            elapsed = true;
        }

        /// <summary>
        /// This function is called periodically during autonomous
        /// </summary>
        public override void AutonomousPeriodic()
        {
            switch (autoSelected)
            {
                case customAuto:
                    //Put custom auto code here
                    break;
                case defaultAuto:
                default:
                    if (!elapsed)
                    {
                        time.Enabled = true;
                        drive.TankDrive(-.6, -.6);
                    }
                    //Put default auto code here
                    break;
            }
        }

        /// <summary>
        /// This function is called periodically during operator control
        /// </summary>
        public override void TeleopPeriodic()
        {

            //while teleop is enabled, and the drivestation is enabled, run this code
            while(IsOperatorControl && IsEnabled)
            {

                //Cubing a decimal makes the decimal smaller, and the actual stick axis is a decimal between 0 and 1

                //these two doubles named "a" and "b" cube the actual raw stick value, and will make the controls 
                //much less sensitive, or at least until we test it, it should go from mach 8 to mach 2
                //we then pass these two doubles to the TankDrive method and voila, the robot drives.

                double a = Math.Pow(stick.GetRawAxis(1), 3);
                double b = Math.Pow(stick.GetRawAxis(5), 3);
                drive.TankDrive(a, b);
                //stick.GetRawButton(0) should be the "a" button
                //GetRawButton(1) should be "b"
                double speed = 0.0;
                speed += (stick.GetRawButton(0) ? 1.0 : 0.0);
                speed += (stick.GetRawButton(1) ? 1.0 : 0.0);
                climber.SetSpeed(speed);
                //create a delay of .1 second
                Timer.Delay(0.1);
            }
           
        }



        /// <summary>
        /// This function is called periodically during test mode
        /// </summary>
        public override void TestPeriodic()
        {

        }
    }
}
