Assumptions:

1.It is a Card game that can be player by 2 players of 4 players.
2.If One of the players is unable to Draw the number of cards as required in war as they do not have sufficient number of cards then the opponent player wins the Match by default.
3. When in War mode the battle will always start with the first player/player1.

Project Structure:
Audio - Contains all audio files
Code_Files - All the Codes
Images - All the image assets used in the game
Prefabs - All Prefabs in game
Scene - All Scene in the game

Code Structure:

Manager Classes - 
1.Manager UI : 
Handles all the screen transitions in the game, Contains 
Main Methods:
Show Screen() - Displays the screen passed as parameter.
Hide Screen() - Hides the current displayed screen.
Screen enums

2.Manager GamePlay :
Handles all the game player related operations
Main Methods:
SetPlayersTurn() - Sets the players turn.
SwitchPlayersTurn() - Switches the turn to the next player.
CheckForRoundWinner() - Checks if any player has won the round.
DeclareRoundWinner() - Declares the winner and check if match is over, if not loads the next round.
DeclareMatchWinner() - Declares the winner of the match and ends the game.
LoadNextRound() - Load the next round by setting up the previous rounds winner as next player to play the round.

UI Classes :
Base UI - Base class for all the UI screens.
Player Controler - To controle the players action on UI.
