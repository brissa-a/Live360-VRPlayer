using System;
using System.Threading;
using System.Windows.Media.Media3D;

using MoveFramework_CS;

namespace VrPlayer.Models.Trackers
{
    public class PsMoveTracker : TrackerBase, ITracker
    {
        public PsMoveTracker()
        {
            try
            {
                init();
            }
            catch (Exception exc)
            {
                throw new Exception("Error while initializing PsMove tracker.", exc);
            }
        }

        public void init()
        {
            MoveWrapper.init();

            int moveCount = MoveWrapper.getMovesCount();

            if (moveCount > 0)
            {
				/*
                bool buttonX = MoveWrapper.getButtonState(0, MoveButton.B_CROSS);
                MoveWrapper.Quaternion q = MoveWrapper.getOrientation(0);
                MoveWrapper.Vector3 v = MoveWrapper.getPosition(0);
                int trigger = MoveWrapper.getTriggerValue(0);
                */
				MoveWrapper.setRumble(0, 255);
                Thread.Sleep(40);
                MoveWrapper.setRumble(0, 0);
            }
            MoveWrapper.subscribeMoveUpdate(
				MoveUpdateCallback, 
				MoveKeyDownCallback, 
				MoveKeyUpCallback, 
				NavUpdateCallback, 
				NavKeyDownCallback, 
				NavKeyUpCallback
			);
        }

        void MoveUpdateCallback(int id, MoveWrapper.Vector3 position, MoveWrapper.Quaternion orientation, int trigger)
        {
            Quaternion = new Quaternion(orientation.x, orientation.y, orientation.z, orientation.w);
            Position = new Vector3D(position.x, position.y, position.z);
        }

    	static void MoveKeyUpCallback(int id, int keyCode)
        {
        }

    	static void MoveKeyDownCallback(int id, int keyCode)
        {
         }

    	static void NavUpdateCallback(int id, int trigger1, int trigger2, int stickX, int stickY)
        {
        }

    	static void NavKeyUpCallback(int id, int keyCode)
        {
        }

    	static void NavKeyDownCallback(int id, int keyCode)
        {
        }

        public override void Dispose()
        {
            MoveWrapper.unsubscribeMove();
        }
    }
}
