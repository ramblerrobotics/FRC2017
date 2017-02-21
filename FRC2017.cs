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
        const string straightAuto = "Straight";
        const string straight = "bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbxxxxxxxxxyyyyyyyyyyyyyyyyyxbbbbbbbbbb\0";
        string autoSelected;
        SendableChooser chooser;
        bool elapsed;
        RobotDrive drive;
        Joystick stick;
        System.Timers.Timer time;
        VictorSP climber;
        VictorSP getRope;
        char flag;
        System.Timers.Timer testTime;
        int elapsedTimes;
        /// <summary>
        /// This function is run when the robot is first started up and should be
        /// used for any initialization code.
        /// </summary>
        public override void RobotInit()
        {
            chooser = new SendableChooser();
            chooser.AddDefault("Default Auto", defaultAuto);
            chooser.AddObject("My Auto", straightAuto);
            SmartDashboard.PutData("Chooser", chooser);
            //start cameras?
            CameraServer.Instance.StartAutomaticCapture(0);
            CameraServer.Instance.StartAutomaticCapture(1);
            //create joystick and robotdrive objects for joystick input and motor control
            stick = new Joystick(0);
            drive = new RobotDrive(0, 1, 2, 3);
            climber = new VictorSP(4);
            getRope = new VictorSP(5);
            flag = '\0';
            testTime = new System.Timers.Timer(50);
            elapsed = true;
            elapsedTimes = 0;
            testTime.AutoReset = false;
            testTime.Elapsed += TimeAlert;
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
            autoSelected = SmartDashboard.GetString("Auto Selector", defaultAuto);
            Console.WriteLine("Auto selected: " + autoSelected);
            time = new System.Timers.Timer(50);
            time.AutoReset = true;
            elapsed = false;
            time.Elapsed += TimeAlert;
            time.Start();
        }
        private void TimeAlert(object source, System.Timers.ElapsedEventArgs e)
        {
            elapsed = true;
            elapsedTimes++;
        }
        /// <summary>
        /// This function is called periodically during autonomous
        /// </summary>
        public override void AutonomousPeriodic()
        {
            char letter;
            switch (autoSelected)
            {
                case straightAuto:
                    //Put custom auto code here
                    letter = straight[elapsedTimes];
                    if (letter == 'a')
                    {
                        drive.TankDrive(-.6, -.6);
                    }
                    else if (letter == 'b')
                    {
                        drive.TankDrive(.6, .6);
                    }
                    else if (letter == 'x')
                    {
                        drive.TankDrive(.6, -.6);
                    }
                    else if (letter == 'y')
                    {
                        drive.TankDrive(-.6, .6);
                    }else if (letter=='\0')
                    {
                        time.Stop();
                        drive.TankDrive(0f, 0f);
                    }
                    break;
                case defaultAuto:
                default:
                    letter = straight[elapsedTimes];
                    if (letter == 'a')
                    {
                        drive.TankDrive(-.6, -.6);
                    }
                    else if (letter == 'b')
                    {
                        drive.TankDrive(.6, .6);
                    }
                    else if (letter == 'x')
                    {
                        drive.TankDrive(.6, -.6);
                    }
                    else if (letter == 'y')
                    {
                        drive.TankDrive(-.6, .6);
                    }
                    else if (letter == '\0')
                    {
                        time.Stop();
                        drive.TankDrive(0f, 0f);
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

            while (IsOperatorControl && IsEnabled)
            {
                //clear out values for new run
                SmartDashboard.PutString("DB/String 3", "");


                if (stick.GetRawButton(5))
                {
                    drive.TankDrive(stick.GetRawAxis(5), stick.GetRawAxis(1));
                } else
                {
                    if (!stick.GetRawButton(6))
                    {
                        drive.TankDrive(stick.GetRawAxis(5) / 1.66666666, stick.GetRawAxis(1) / 1.66666666);
                    }else
                    {
                        drive.TankDrive(-stick.GetRawAxis(1) / 1.66666666, -stick.GetRawAxis(5) / 1.66666666);
                    }

                }

                

                SmartDashboard.PutNumber("DB/String 0", stick.GetRawAxis(5));
                SmartDashboard.PutNumber("DB/String 1", stick.GetRawAxis(1));




                //stick.GetRawButton(0) should be the "a" button
                //GetRawButton(1) should be "b"
                double speed = 0.0;
                if (stick.GetRawButton(8))
                {
                    speed += (stick.GetRawButton(1) ? 0.5 : 0.0);
                    speed += (stick.GetRawButton(2) ? 0.5 : 0.0);
                }
                climber.SetSpeed(-speed);

                getRope.SetSpeed(stick.GetRawAxis(2) - stick.GetRawAxis(3));
                //create a delay of .1 second
                Timer.Delay(0.1);
            }

        }



        /// <summary>
        /// This function is called periodically during test mode
        /// </summary>
        public override void TestPeriodic()
        {
            time.Stop();
            elapsedTimes = 0;
            if (elapsed)
            {
                if (stick.GetRawButton(1))
                {
                    SmartDashboard.PutString("DB/String 3", SmartDashboard.GetString("DB/String 3", "") + char.ToString('a'));
                    //Console.WriteLine('a');
                    testTime.Stop();
                    testTime.Start();
                    flag = 'a';
                    drive.TankDrive(-.6, -.6);
                }
                else if (stick.GetRawButton(2))
                {
                    SmartDashboard.PutString("DB/String 3", SmartDashboard.GetString("DB/String 3", "")+char.ToString('b'));
                    //Console.WriteLine('b');
                    testTime.Stop();
                    testTime.Start();
                    flag = 'b';
                    drive.TankDrive(.6, .6);
                }
                else if (stick.GetRawButton(3))
                {
                    SmartDashboard.PutString("DB/String 3", SmartDashboard.GetString("DB/String 3", "") + char.ToString('x'));
                    //Console.WriteLine('x');
                    testTime.Stop();
                    testTime.Start();
                    flag = 'x';
                    drive.TankDrive(.6, -.6);
                }
                else if (stick.GetRawButton(4))
                {
                    SmartDashboard.PutString("DB/String 3", SmartDashboard.GetString("DB/String 3", "") + char.ToString('y'));
                    //Console.WriteLine('y');
                    testTime.Stop();
                    testTime.Start();
                    flag = 'y';
                    drive.TankDrive(-.6, .6);
                }
                else
                {
                    drive.TankDrive(0f, 0f);
                }
            }

            else
            {
                if (flag == 'a')
                {
                    drive.TankDrive(-.6, -.6);
                }
                else if(flag == 'b')
                {
                    drive.TankDrive(.6, .6);
                }
                else if(flag == 'x')
                {
                    drive.TankDrive(.6, -.6);
                }
                else if(flag == 'y')
                {
                    drive.TankDrive(-.6, .6);
                }
            }
        }
    }
}
