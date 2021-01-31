public static class OverallGameState
{
    public enum State { Ongoing, Win, Lose };
    public static State currGameState = State.Ongoing;
}