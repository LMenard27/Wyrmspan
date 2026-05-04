public class ApiResponse {
   GameBoard board;
   Player[] players;
   int activePlayer;
   GameStackFrame currFrame;


   /*
   Constructor for the ApiResponse class, which takes in a GameBoard, an array of Players, an integer representing the active player, and a GameStackFrame. It initializes the corresponding fields with the provided values.
   */
   public ApiResponse(GameBoard b, Player[] p, int activePlayer, GameStackFrame frame) {
       this.board = b;
       this.players = p;
       this.activePlayer = activePlayer;
       this.currFrame = frame;
   }


   /*
   Getter for the GameBoard field.
  
   Return:
    the GameBoard associated with this ApiResponse.
   */
   public GameBoard getGameBoard() {
       return this.board;
   }


   /*
   Getter for the array of players
  
   Return:
   the array of Players associated with this ApiResponse.
   */
   public Player[] getPlayers() {
       return this.players;
   }


   /*
   Getter for the active player.
  
   Return:
   the integer representing the active player.
   */
   public int getActivePlayer() {
       return this.activePlayer;
   }


   /*
   Getter for the current game stack frame.
  
   Return:
   the GameStackFrame associated with this ApiResponse.
   */
   public GameStackFrame getFrame() {
       return this.currFrame;
   }
}
