namespace Damian.MWGK.GameStates
{
    enum GameState : byte
    {
        Menu = 1,
        Info = 2,
        Game = 3,
        Help = 4,

        ChangingToMenuFromInfoP1 = 5,
        ChangingToMenuFromInfoP2 = 6,
        ChangingToMenuFromGameP1 = 7,
        ChangingToMenuFromGameP2 = 8,
        ChangingToMenuFromHelpP1 = 9,
        ChangingToMenuFromHelpP2 = 10,

        ChangingToInfoFromMenuP1 = 11,
        ChangingToInfoFromMenuP2 = 12,

        ChangingToGameFromMenuP1 = 13,
        ChangingToGameFromMenuP2 = 14,

        ChangingToHelpFromMenuP1 = 15,
        ChangingToHelpFromMenuP2 = 16

        
    }
}