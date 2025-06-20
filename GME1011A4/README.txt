GME1011_A4_BastV

Instructions: In this game, you have to collect five wooden boxes while dodging metal boxes. The player can
move to left or right with the keys "A", and "D".

The player only has three lives, and the goal is to collect five wooden boxes.

Source of this game: This is my Assignment 5 from GME1003, but now it has a good implementation of objects,
inheritance, and polymorphism.

Use of AI: For this assignment, I had to use AI (chatgpt) for a specific problem. My idea is make the player
class responsible to  draw its three different sprites, depending on which direction the player is moving.
It is because of this, that I wanted to change the player sprite if a key was pressed or not, so it has
to be in update.

I asked to chatgpt how to update my player while also checking if a key was pressed or not, and the solution
was to update GameTime, and KeyboardState. The solution was helpful, because thanks to this, the player class
is responsible for the sprite and direction.