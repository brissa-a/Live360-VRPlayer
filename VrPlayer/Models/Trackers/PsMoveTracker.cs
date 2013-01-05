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
                IsEnabled = true;
                this.PositionScaleFactor = 0.01;
                Init();
            }
            catch (Exception exc)
            {
                IsEnabled = false;
            }  
        }

        private void Init()
        {
            MoveWrapper.init();

            int moveCount = MoveWrapper.getMovesCount();

            if (moveCount > 0)
            {
				MoveWrapper.setRumble(0, 255);
                Thread.Sleep(40);
                MoveWrapper.setRumble(0, 0);

                MoveWrapper.subscribeMoveUpdate(
                    MoveUpdateCallback,
                    MoveKeyDownCallback,
                    MoveKeyUpCallback,
                    NavUpdateCallback,
                    NavKeyDownCallback,
                    NavKeyUpCallback
                );
            }   
        }

        void MoveUpdateCallback(int id, MoveWrapper.Vector3 position, MoveWrapper.Quaternion orientation, int trigger)
        {
            Rotation = new Quaternion(
                orientation.x, 
                -orientation.y, 
                orientation.z, 
                -orientation.w);

            Vector3D scaledPos = new Vector3D(
                position.x * PositionScaleFactor,
                position.y * PositionScaleFactor,
                position.z * PositionScaleFactor);

            if (MoveWrapper.getButtonState(0, MoveButton.B_START))
            {
                this.BasePosition = -scaledPos;
            }

            this.Position = BasePosition + scaledPos;
        }

    	void MoveKeyUpCallback(int id, int keyCode)
        {
        }

    	void MoveKeyDownCallback(int id, int keyCode)
        {
        }

    	void NavUpdateCallback(int id, int trigger1, int trigger2, int stickX, int stickY)
        {
        }

    	void NavKeyUpCallback(int id, int keyCode)
        {
        }

    	void NavKeyDownCallback(int id, int keyCode)
        {
        }

        public override void Dispose()
        {
            MoveWrapper.unsubscribeMove();
        }
    }
}
