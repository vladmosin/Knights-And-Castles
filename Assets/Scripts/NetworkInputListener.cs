using UnityEngine.UI;

namespace Assets.Scripts
{
    public class NetworkInputListener : InputListener
    {
        private MultiplayerController multiplayerController;

        public Text logText;

        private int boardWidth;
        private int boardHeight;
        
        public void Init(int boardWidth, int boardHeight)
        {
            this.boardWidth = boardWidth;
            this.boardHeight = boardHeight;
            multiplayerController = MultiplayerController.GetInstance();
            multiplayerController.OnMessageReceived += ProcessNetworkData;
        }

        //'M' -- move (otherwise ignore this message), followed by one of:
        //'B' -- button click, followed by x and y
        //'F' -- finish turn
        //'S' -- split (TODO:)
        private void ProcessNetworkData()
        {
            byte[] message = multiplayerController.lastMessage;
            if (message[0] != 'M')
            {
                return;
            }

            if (message[1] == 'B')
            {
                int x = boardWidth - message[2] + 1;
                int y = boardHeight - message[3] + 1;
                logText.text += "Receive:" + x + " " + y + "\n";
                base.ProcessBoardClick(x, y);
            }
            else if (message[1] == 'F')
            {
                logText.text += "Receive finish turn" + "\n";
                base.ProcessFinishTurnClick();
            }
        }

        public override void ProcessBoardClick(int x, int y)
        {
            byte[] message = {(byte)'M', (byte)'B', (byte) x, (byte) y};
            logText.text += "Send:" + x + " " + y + "\n";
            multiplayerController.SendMessage(message);
            base.ProcessBoardClick(x, y);
        }

        public override void ProcessFinishTurnClick()
        {
            byte[] message = {(byte) 'M', (byte) 'F'};
            logText.text += "Send finish turn" + "\n";
            multiplayerController.SendMessage(message);
            base.ProcessFinishTurnClick();
        }
    }
}