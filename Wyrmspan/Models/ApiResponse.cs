class ApiResponse {
    GameBoard board;
    Player[] players;
    int activePlayer;
    StackFrame currFrame;

    public ApiResponse(GameBoard b, Player[] p, int activePlayer, StackFrame frame) {
        this.board = b;
        this.players = p;
        this.activePlayer = activePlayer;
        this.currFrame = frame;
    }

    public GameBoard getGameBoard() {
        return this.board;
    }

    public Player[] getPlayers() {
        return this.players;
    }

    public int getActivePlayer() {
        return this.activePlayer;
    }

    public StackFrame getFrame() {
        return this.currFrame;
    }
}