namespace Damian.MWGK.GameStates
{
    enum GameState : byte
    {
        Menu = 1,
        Credits = 2, //and info
        Game = 3,
        ChangingToMenuFromCredtis = 4,
        ChangingToMenuFromGame = 5,
        ChangingToCreditsFromMenu=6
    }
}